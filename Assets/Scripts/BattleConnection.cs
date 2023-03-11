using System.Collections;
using System.Collections.Generic;
using Nakama;
using UnityEngine;

public class BattleConnection
{
    public string MatchId { get; set; }
    public string HostId { get; set; }
    public List<IUserPresence> Users { get; set; }
    public IMatchmakerMatched Matched { get; set; }

    public BattleConnection(IMatchmakerMatched matched)
    {
        Matched = matched;
    }

    public GameStateManager GameStateManager { get; internal set; }
}