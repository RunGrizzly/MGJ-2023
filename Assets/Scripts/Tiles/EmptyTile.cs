using UnityEngine;

namespace Tiles
{
    public class EmptyTile: TrackTile
    {
        private EmptyTile()
        { }

        public override bool CanApproach(Direction direction)
        {
            return false;
        }
    }
}