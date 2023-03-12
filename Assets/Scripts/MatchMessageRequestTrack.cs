using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageRequestTrack : MatchMessage<MatchMessageRequestTrack>
{

}

public class MatchMessageStartGame : MatchMessage<MatchMessageStartGame>
{
    public readonly string name;

    public MatchMessageStartGame(string name)
    {
        this.name = name;
    }
}