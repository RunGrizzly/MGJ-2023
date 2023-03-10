using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGrid : MonoBehaviour
{
    [SerializeField] private Vector2 m_gridDims = Vector2.zero;
    TrackTile[,] m_gridTiles = new TrackTile[,] { };
    [SerializeField] private Transform m_tileHolder = null;

    private void GenerateGrid(Vector2 dimensions)
    {
        //Figure out if we have manually passed in a new dimension or if we wanna use the inspector value 
        Vector2 newDimensions = dimensions == null ? m_gridDims : dimensions;

    }





}
