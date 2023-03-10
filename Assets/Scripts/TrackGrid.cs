using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int m_gridDims = Vector2Int.zero;
    List<GameObject> m_gridTiles =  new List<GameObject>();
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private float m_generationSpeed = 0.2f;

    public void Start()
    {
        StartCoroutine(GenerateGrid());
    }

    public IEnumerator GenerateGrid()
    {
        if (m_gridTiles.Count > 0)
        {
            ClearTiles();
        }

        var counter = 0;
        for (int x = 0; x < m_gridDims.x; x++)
        {
            for (int y = 0; y < m_gridDims.y; y++)
            {
                TrackTile tile = Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity).GetComponent<TrackTile>();

                m_gridTiles.Add(tile.gameObject);
                tile.SetState(counter % 2 == 0 ? TileState.Track : TileState.Empty, new Vector2Int(x, y));
                
                counter++;
                yield return new WaitForSeconds(m_generationSpeed);
            }
        }
    }

    private void ClearTiles()
    {
        foreach (var mGridTile in m_gridTiles)
        {
            Destroy(mGridTile);
        }
        
        m_gridTiles.RemoveAll(t => t == null);

    }
}
