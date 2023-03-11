using UnityEngine;
using Nakama;
using System.Linq;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameConnection _connection;
    private TrackGrid _grid;

    private List<Train> _trains;

    // Start is called before the first frame update
    protected async void Start()
    {
        Brain.ins.EventManager.gridCompleted.AddListener((grid) =>
        {
            _grid = grid;
        });
        Brain.ins.EventManager.battleReady.AddListener(userIds =>
        {
            _trains = new List<Train>();
            foreach (var userId in userIds)
            {
                var train = _grid.SpawnTrain();
                train.UserId = userId;
                _trains.Add(train);
            }
            StartBattle();
        });
        Brain.ins.EventManager.trainDestinationUpdate.AddListener(train =>
        {
            AskPlayerForTrack(train);
        });
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
