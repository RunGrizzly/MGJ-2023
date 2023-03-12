using UnityEngine;

namespace Tiles
{
    public class BlankTile: TrackTile
    {
        public override bool CanApproach(Direction direction)
        {
            return false;
        }
    }
}