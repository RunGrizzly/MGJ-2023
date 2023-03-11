using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Linq;
using System.Threading.Tasks;

public class GameStateManager
{
    private GameConnection _connection;

    public event Action<IUserPresence> OnPlayerAttemptJoin;
    public event Action<IUserPresence> OnPlayerLeave;

    public event Action<MatchMessageTrackSelected> OnTrackSelected;

    public GameStateManager(GameConnection connection)
    {
        _connection = connection;

        _connection.Socket.ReceivedMatchPresence += OnMatchPresence;
        _connection.Socket.ReceivedMatchState += ReceiveMatchStateMessage;
    }

    public async Task SendMatchStateMessage<T>(MatchMessageType opCode, T message, IEnumerable<IUserPresence> presences) where T : MatchMessage<T>
    {
        try
        {
            //Packing MatchMessage object to json
            string json = MatchMessage<T>.ToJson(message);

            //Sending match state json along with opCode needed for unpacking message to server.
            //Then server sends it to other players
            await _connection.Socket.SendMatchStateAsync(_connection.BattleConnection.MatchId, (long)opCode, json, presences);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending match state: " + e.Message);
        }
    }

    private void OnMatchPresence(IMatchPresenceEvent e)
    {
        if (e.Joins.Count() > 0)
        {
            foreach (var join in e.Joins)
            {
                OnPlayerAttemptJoin?.Invoke(join);
            }
        }

        if (e.Leaves.Count() > 0)
        {
            foreach (var leave in e.Leaves)
            {
                OnPlayerLeave?.Invoke(leave);
            }
        }
    }

    private void ReceiveMatchStateMessage(IMatchState matchState)
    {
        var userSessionId = matchState.UserPresence.SessionId;
        string messageJson = System.Text.Encoding.UTF8.GetString(matchState.State);

        if (string.IsNullOrEmpty(messageJson))
        {
            return;
        }

        switch ((MatchMessageType)matchState.OpCode)
        {
            case MatchMessageType.TrackSelected:
                var message = MatchMessageTrackSelected.Parse(messageJson);
                message = new MatchMessageTrackSelected(message.trackId, matchState.UserPresence.UserId);
                OnTrackSelected?.Invoke(message);
                break;
            default:
                break;
        }

    }
}
