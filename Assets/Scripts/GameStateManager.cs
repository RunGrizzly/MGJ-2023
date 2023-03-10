using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;

public class GameStateManager : MonoBehaviour
{
    private GameConnection _connection;

    public event Action<MatchMessageTrackSelected> OnTrackSelected;

    public GameStateManager(GameConnection connection)
    {
        _connection = connection;

        _connection.Socket.ReceivedMatchPresence += OnMatchPresence;
        _connection.Socket.ReceivedMatchState += ReceiveMatchStateMessage;
    }

    public void ReceiveMatchStateHandle(string messageJSON)
    {
        MatchMessageTrackSelected matchMessageTrackSelected = MatchMessageTrackSelected.Parse(messageJSON);
        OnTrackSelected?.Invoke(matchMessageTrackSelected);
    }

    private void OnMatchPresence(IMatchPresenceEvent e)
    {

    }

    private void ReceiveMatchStateMessage(IMatchState matchState)
    {
        string messageJson = System.Text.Encoding.UTF8.GetString(matchState.State);

        if (string.IsNullOrEmpty(messageJson))
        {
            return;
        }

        ReceiveMatchStateHandle(messageJson);
    }
}
