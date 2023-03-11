using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMessageTrackSelected : MatchMessage<MatchMessageTrackSelected>
{
    public readonly int TrackId;

    public MatchMessageTrackSelected(int trackId)
    {
        TrackId = trackId;
    }
}
