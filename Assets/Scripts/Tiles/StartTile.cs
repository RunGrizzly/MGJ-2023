using UnityEngine;

namespace Tiles
{
    public class StartTile: TrackTile
    {
        private StartTile()
        { }

        public override bool CanApproach(Direction direction)
        {
            return false;
        }
    }
}