using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int m_gridDims = Vector2Int.zero;
    List<GameObject> m_gridTiles;
    [SerializeField] private GameObject TilePrefab;

    private void Start()
    {
        m_gridTiles = new List<GameObject>();
        GenerateGrid();
    }

    private void GenerateGrid(Vector2Int? dimensions = null)
    {
        //Figure out if we have manually passed in a new dimension or if we wanna use the inspector value
        var newDimensions = dimensions ?? m_gridDims;

        var counter = 0;
        for (int x = 0; x < newDimensions.x; x++)
        {
            for (int y = 0; y < newDimensions.y; y++)
            {
                var tile = Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                
                m_gridTiles.Add(tile);
                tile.GetComponent<TrackTile>().SetState(counter % 2 == 0 ? TileState.Track : TileState.Empty, new Vector2Int(x, y));

                counter++;
            }
        }
    }
}
