using UnityEngine;
using Nakama;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameConnection _connection;

    // Start is called before the first frame update
    protected async void Start()
    {
        IMatch match = await _connection.Socket.CreateMatchAsync(GameConstants.MatchName);

        _connection.BattleConnection.MatchId = match.Id;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
