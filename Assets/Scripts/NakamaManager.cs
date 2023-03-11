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
        await StartBattle();
    }

    private async Task StartBattle()
    {
        _connection.Socket.ReceivedMatchmakerMatched += ReceivedMatchmakerMatched;
        _connection.Socket.ReceivedError += error =>
        {
            Debug.LogError("Received error on socket " + error.Message);
        };
        var ticket = await _connection.Socket.AddMatchmakerAsync("*", 2, 3, new Dictionary<string, string> { { "type", "host" } });
    }


    private async void ReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        Debug.Log("We have been match made");
        _connection.BattleConnection = new BattleConnection(matched);
        var match = await _connection.Socket.JoinMatchAsync(matched);
        Debug.Log("We have joined match");
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
            Debug.LogError("Error connecting socket: " + e.Message);
        }
    }

}
