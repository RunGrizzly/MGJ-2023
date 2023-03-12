using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTurnTile : TrackTile
{
    public override bool CanApproach(Direction direction)
    {
        switch (m_orientation)
        {
            case Direction.North:
                return direction == Direction.North || direction == Direction.East;
            case Direction.East:
                return direction == Direction.East || direction == Direction.South;
            case Direction.South:
                return direction == Direction.South || direction == Direction.West;
            case Direction.West:
                return direction == Direction.West || direction == Direction.North;
        }

        return false;
    }
}
