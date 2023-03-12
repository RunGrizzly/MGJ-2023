using UnityEngine;

namespace Tiles
{
    public class StraightTile: TrackTile
    {
        private StraightTile()
        { }
        
        public override bool CanApproach(Direction direction)
        {
            switch (m_orientation)
            {
                case Direction.North:
                case Direction.South:
                    return direction == Direction.North || direction == Direction.South;
                case Direction.East:
                case Direction.West:
                    return direction == Direction.East || direction == Direction.West;
            }

            return false;
        }
    }
}