using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System;
using System.Threading.Tasks;
using System.Linq;

public class NakamaManager : MonoBehaviour
{
    [SerializeField]
    private GameConnection _connection;

    private async void Start()
    {
        if (_connection.Session == null)
        {
            string deviceId = GetDeviceId();
            if (!string.IsNullOrEmpty(deviceId))
            {
                PlayerPrefs.SetString(GameConstants.DeviceIdKey, deviceId);
            }

            await InitializeGame(deviceId);
        }
    }

    private async Task InitializeGame(string deviceId)
    {
        var client = new Client("http", "127.0.0.1", 7350, "defaultkey");
        client.Timeout = 5;

        var socket = client.NewSocket(useMainThread: true);

        string authToken = PlayerPrefs.GetString(GameConstants.AuthTokenKey, null);
        bool isAuthToken = !string.IsNullOrEmpty(authToken);

        string refreshToken = PlayerPrefs.GetString(GameConstants.RefreshTokenKey, null);

        ISession session = null;

        if (isAuthToken)
        {
            session = Session.Restore(authToken, refreshToken);

            if (session.HasExpired(DateTime.UtcNow.AddDays(1)))
            {
                try
                {
                    session = await client.SessionRefreshAsync(session);
                }
                catch (ApiResponseException)
                {
                    session = await client.AuthenticateDeviceAsync(deviceId);
                    PlayerPrefs.SetString(GameConstants.RefreshTokenKey, session.RefreshToken);
                }

                PlayerPrefs.SetString(GameConstants.AuthTokenKey, session.AuthToken);
            }
        }
        else
        {
            session = await client.AuthenticateDeviceAsync(deviceId);
            PlayerPrefs.SetString(GameConstants.AuthTokenKey, session.AuthToken);
            PlayerPrefs.SetString(GameConstants.RefreshTokenKey, session.RefreshToken);
        }

        socket.Closed += () => Connect(socket, session);

        Connect(socket, session);

        IApiAccount account = null;

        try
        {
            account = await client.GetAccountAsync(session);
        }
        catch (ApiResponseException e)
        {
            Debug.LogError("Error getting user account: " + e.Message);

        }

        _connection.Init(client, socket, account, session);

        socket.Connected += OnSocketConnected;
    }

    private async void OnSocketConnected()
    {
        var party = await _connection.Socket.CreatePartyAsync(true, GameConstants.MaxPlayerCount + 1);
        _connection.Party = party;
        _connection.Socket.ReceivedPartyJoinRequest += OnReceivedPartyJoinRequest;

    }

    private async void OnReceivedPartyJoinRequest(IPartyJoinRequest request)
    {
        foreach (var presence in request.Presences)
        {
            await _connection.Socket.AcceptPartyMemberAsync(request.PartyId, presence);
            _connection.Party.Presences.Append(presence);
        }

        if (_connection.Party.Presences.Count() > 2)
        {
            await StartBattle();
        }
    }

    private async Task StartBattle()
    {
        _connection.Socket.ReceivedMatchmakerMatched += ReceivedMatchmakerMatched;

        await _connection.Socket.AddMatchmakerPartyAsync(_connection.Party.Id, "*", 3, GameConstants.MaxPlayerCount + 1);
    }


    private async void ReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        _connection.BattleConnection.Matched = matched;
        var match = await _connection.Socket.JoinMatchAsync(matched);
        _connection.BattleConnection.MatchId = match.Id;
        _connection.BattleConnection.PlayerIds = match.Presences.Select(p => p.UserId).ToList();

    }

    private string GetDeviceId()
    {
        string deviceId = "";

        deviceId = PlayerPrefs.GetString(GameConstants.DeviceIdKey);

        if (string.IsNullOrWhiteSpace(deviceId))
        {
            deviceId = Guid.NewGuid().ToString();
        }

        return deviceId;
    }

    private async void OnApplicationQuit()
    {
        await _connection.Socket.CloseAsync();
    }

    private async void Connect(ISocket socket, ISession session)
    {
        try
        {
            if (!socket.IsConnected)
            {
                await socket.ConnectAsync(session);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error connecting socket: " + e.Message);
        }
    }

}
