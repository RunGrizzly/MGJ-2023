using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTurnTile : TrackTile
{
    public override bool CanApproach(Direction direction)
    {
        switch (m_orientation)
        {
            case Direction.North:
               return direction == Direction.North || direction == Direction.West;
            case Direction.East:
                return direction == Direction.East || direction == Direction.North;
            case Direction.South:
                return direction == Direction.South || direction == Direction.East;
            case Direction.West:
                return direction == Direction.West || direction == Direction.South;
        }

        return false;
    }
}
