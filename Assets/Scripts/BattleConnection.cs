using System.Collections;
using System.Collections.Generic;
using Nakama;
using UnityEngine;

namespace Assets.Scripts
{
    public class BattleConnection
    {
        public string MatchId { get; set; }
        public string HostId { get; set; }
        public List<string> PlayerIds { get; set; }
        public IMatchmakerMatched Matched { get; set; }

        public BattleConnection(IMatchmakerMatched matched)
        {
            Matched = matched;
        }
    }
}