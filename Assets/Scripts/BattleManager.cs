using UnityEngine;
using Nakama;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameConnection _connection;
    private TrackGrid _grid;
    private GameStateManager _stateManager;

    private List<Train> _trains;

    // Start is called before the first frame update
    protected async void Start()
    {
        Brain.ins.EventManager.gridCompleted.AddListener(async (grid) =>
        {
            _grid = grid;
            await SearchMatch();
        });
        Brain.ins.EventManager.battleReady.AddListener(async userIds =>
        {
            _trains = new List<Train>();
            foreach (var userId in userIds)
            {
                var train = _grid.SpawnTrain();
                train.UserId = userId;
                _trains.Add(train);
                var presence = _connection.BattleConnection.Users.Find(p => p.UserId == userId);
                await _stateManager.SendMatchStateMessage(MatchMessageType.StartGame, new MatchMessageStartGame(train.TrainName), new List<IUserPresence> { presence });
            }
            StartBattle();
        });
        Brain.ins.EventManager.trainDestinationUpdate.AddListener(train =>
        {
            AskPlayerForTrack(train);
        });
        Brain.ins.EventManager.battleEnded.AddListener(async (tuple) =>
        {
            var presence = _connection.BattleConnection.Users.Find(p => p.UserId == tuple.Item1.UserId);
            await _stateManager.SendMatchStateMessage(MatchMessageType.GameEnded, new MatchMessageGameEnd(tuple.Item2), new List<IUserPresence> { presence });

            var remainingTrains = _trains.Where(t => t != null);
            if (remainingTrains.Count() == 1)
            {
                var winner = _connection.BattleConnection.Users.Find(p => p.UserId == remainingTrains.First().UserId);
                await _stateManager.SendMatchStateMessage(MatchMessageType.GameEnded, new MatchMessageGameEnd(true), new List<IUserPresence> { winner });

                // RESET GAME STATE
            }
        });
    }
    private async Task SearchMatch()
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
        _connection.BattleConnection.Users = matched.Users.Select(u => u.Presence).ToList();
        _stateManager = new GameStateManager(_connection);
        _connection.BattleConnection.GameStateManager = _stateManager;

        Brain.ins.EventManager.battleReady.Invoke(matched.Users.Where(user => user.Presence.UserId != matched.Self.Presence.UserId).Select(user => user.Presence.UserId).ToList());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartBattle()
    {
        var sm = _connection.BattleConnection.GameStateManager;
        sm.OnTrackSelected += OnTrackSelected;

        // Start the trains

        // Ask players for first Lookahead
        _trains.ForEach(train => AskPlayerForTrack(train));
    }

    private async void AskPlayerForTrack(Train train)
    {
        var presence = _connection.BattleConnection.Users.Find(u => u.UserId == train.UserId);
        await _connection.BattleConnection.GameStateManager.SendMatchStateMessage(MatchMessageType.RequestTrack, new MatchMessageRequestTrack(), new List<IUserPresence> { presence });
    }

    private void OnTrackSelected(MatchMessageTrackSelected message)
    {
        Train playersTrain = _trains.Find(t => t.UserId == message.userId);
        _grid.SpawnTile(playersTrain.LookAheadTile, message.trackId);
        var pos = playersTrain.LookAheadTile.postition;
        playersTrain.SetLookahead(new Vector3(pos.x, 0, pos.y), playersTrain.LookAheadTile.direction);
    }
}
