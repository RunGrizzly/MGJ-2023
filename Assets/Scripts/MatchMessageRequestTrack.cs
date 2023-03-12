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

public class MatchMessageGameEnd : MatchMessage<MatchMessageGameEnd>
{
    public readonly bool isWinner;

    public MatchMessageGameEnd(bool isWinner)
    {
        this.isWinner = isWinner;
    }
}