using Nakama;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama.TinyJson;

public class MatchMessageTrackSelected : MatchMessage<MatchMessageTrackSelected>
{
    public readonly string trackId;
    public readonly string userId;

    public MatchMessageTrackSelected(string trackId, string userId)
    {
        this.trackId = trackId;
        this.userId = userId;
    }
}
