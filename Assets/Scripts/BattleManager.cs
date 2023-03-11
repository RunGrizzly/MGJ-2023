using UnityEngine;
using Nakama;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameConnection _connection;

    private GameStateManager _stateManager;
    private bool _acceptingPlayers = false;

    // Start is called before the first frame update
    protected async void Start()
    {
        //IMatch match = await _connection.Socket.

        //_connection.BattleConnection.MatchId = match.Id;

        _stateManager = new GameStateManager(_connection);
        _connection.Socket.ReceivedPartyPresence += Socket_ReceivedPartyPresence;
    }

    private void Socket_ReceivedPartyPresence(IPartyPresenceEvent e)
    {
        //Debug.Log("Player joined party making it " + _connection.Party.Presences.)
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void StartBattle()
    {
        if (_connection.Party.Presences.Count() < 3)
        {
            return;
        }
        _connection.Socket.ReceivedMatchmakerMatched += async matched => await _connection.Socket.JoinMatchAsync(matched);
        var ticket = await _connection.Socket.AddMatchmakerPartyAsync(_connection.Party.Id, "*", 3, GameConstants.MaxPlayerCount + 1);

    }
}
