using System.Collections;
using UnityEngine;
using Nakama;

[CreateAssetMenu(fileName = "GameConnection", menuName = "TRAIN/GameConnection")]
public class GameConnection : ScriptableObject
{
    private IClient _client;
    public ISession Session { get; set; }
    public IApiAccount Account { get; set; }
    private ISocket _socket;
    public IClient Client => _client;
    public ISocket Socket => _socket;
    public BattleConnection BattleConnection { get; set; }
    public IParty Party { get; set; }

    public void Init(IClient client, ISocket socket, IApiAccount account, ISession session)
    {
        _client = client;
        _socket = socket;
        Account = account;
        Session = session;
    }
}
