using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGrid : MonoBehaviour
{
    [SerializeField] private TrainColorSet m_trainColorSet = null;
    private List<ColorSet> m_availableColorSets = null;
    [field: SerializeField] public Vector2Int GridDims = Vector2Int.zero;
    List<GameObject> m_gridTiles = new List<GameObject>();
    [SerializeField] private SerializableDictionary<string, GameObject> TileSet;
    [SerializeField] private float m_generationSpeed = 0.2f;
    [SerializeField] private GameObject Train;
    [SerializeField] private Transform m_tileHolder;

    public List<Train> Trains = new List<Train>();

    public Limits limits = new Limits();

    public void Start()
    {
        StartCoroutine(GenerateGrid());
        m_availableColorSets = new List<ColorSet>(m_trainColorSet.ColorSets);

    }

    public IEnumerator GenerateGrid()
    {
        if (m_gridTiles.Count > 0)
        {
            ClearTiles();
        }

        Brain.ins.EventManager.gridInitialised.Invoke(this);

        for (int x = 0; x < GridDims.x + 1; x++)
        {
            for (int y = 0; y < GridDims.y + 1; y++)
            {
                var tileType = "Empty";
                var orientation = Direction.North;
                
                if (x == 0 || y == 0 || x == GridDims.x || y == GridDims.y)
                {
                    tileType = "Blank";
                }
                
                TrackTile tile = Instantiate(GetTile(tileType), new Vector3(x, 0, y), Quaternion.identity).GetComponent<TrackTile>();
                tile.transform.SetParent(m_tileHolder, true);
                tile.SetState(new Vector2Int(x, y), orientation);

                if (x == 1 && y == 1)
                {
                    limits.BottomLeft = tile.transform;
                }
                else if (x == GridDims.x && y == GridDims.y)
                {
                    limits.TopRight = tile.transform;
                }
                m_gridTiles.Add(tile.gameObject);

                yield return new WaitForSeconds(m_generationSpeed);
            }

        }

        //The grid has finished generating - lets the event manager know
        Brain.ins.EventManager.gridCompleted.Invoke(this);

        // //Test spawn multiple trains
        //Trains.Add(SpawnTrain());
        // yield return new WaitForSeconds(3);
        // Trains.Add(SpawnTrain());
        // yield return new WaitForSeconds(3);
        // Trains.Add(SpawnTrain());
        // yield return new WaitForSeconds(3);


    }

    public Train SpawnTrain()
    {
        var spawnLocation = m_gridTiles.Find(go => go.GetComponent<TrackTile>().m_position == new Vector2Int(1, 1));
        var firstLocation = m_gridTiles.Find(go => go.GetComponent<TrackTile>().m_position == new Vector2Int(2, 1));

        var startBlock = Instantiate(GetTile("Start"), spawnLocation.transform.position, Quaternion.identity);
        startBlock.GetComponent<TrackTile>().SetState(new Vector2Int(1, 1), Direction.East);
        var straightBlock = Instantiate(GetTile("Straight"), firstLocation.transform.position, Quaternion.identity);
        straightBlock.GetComponent<TrackTile>().SetState(new Vector2Int(2, 1), Direction.East);

        m_gridTiles.Remove(firstLocation);
        m_gridTiles.Remove(spawnLocation);
        Destroy(firstLocation);
        Destroy(spawnLocation);
        m_gridTiles.Add(startBlock);
        m_gridTiles.Add(straightBlock);

        var train = Instantiate(Train, new Vector3(1, 0.8f, 1), Quaternion.identity).GetComponent<Train>();
        train.transform.SetParent(transform);
        train.SetDestination(firstLocation.transform.position);

        ColorSet newColorSet = m_availableColorSets[UnityEngine.Random.Range(0, m_availableColorSets.Count)];

        train.Decorate(newColorSet);
        m_availableColorSets.Remove(newColorSet);

        return train;
    }

    public void SpawnTile(Lookahead lookahead, string type)
    {
        var currentTile = m_gridTiles.Find(tile => tile.GetComponent<TrackTile>().m_position.Equals(lookahead.postition));
        var tile = GetTile(type);

        if (!currentTile.name.Contains("Empty"))
        {
            return;
        }

        var newTile = Instantiate(tile, currentTile.transform.position, Quaternion.identity);
        m_gridTiles.Remove(currentTile);
        Destroy(currentTile);
        newTile.GetComponent<TrackTile>().SetState(new Vector2Int((int)currentTile.transform.position.x, (int)currentTile.transform.position.z), lookahead.direction);
        m_gridTiles.Add(newTile);
    }

    private void ClearTiles()
    {
        foreach (var mGridTile in m_gridTiles)
        {
            Destroy(mGridTile);
        }

        m_gridTiles.RemoveAll(t => t == null);
    }

    //Temporary - will need to decide player spawns based on numberPlayers / patterns or something
    private bool IsCorner(int x, int y)
    {
        int maxX = GridDims.x - 1;
        int maxY = GridDims.y - 1;

        if (x == 0 && y == 0 || x == 0 && y == maxY || x == maxX && y == 0 || x == maxX && y == maxY)
        {
            return true;
        }

        return false;
    }

    private GameObject GetTile(string tileName)
    {
        GameObject tile = null;
        TileSet.TryGetValue(tileName, out tile);

        return tile;
    }

    public (GameObject, Direction) GetNextTile(Vector3 position, Direction currentDirection)
    {
        var trainDirection = SelectNextDirection(position, currentDirection);
        GameObject gridTile = null;
        switch (trainDirection)
        {
            case Direction.East:
                gridTile = m_gridTiles.Find(x =>
                    x.transform.position.x == position.x + 1 && x.transform.position.z == position.z);
                break;
            case Direction.North:
                gridTile = m_gridTiles.Find(x =>
                    x.transform.position.x == position.x && x.transform.position.z == position.z + 1);
                break;
            case Direction.South:
                gridTile = m_gridTiles.Find(x =>
                    x.transform.position.x == position.x && x.transform.position.z == position.z - 1);
                break;
            case Direction.West:
                gridTile = m_gridTiles.Find(x =>
                    x.transform.position.x == position.x - 1 && x.transform.position.z == position.z);
                break;
        }

        //return (new Vector3(gridTile.transform.position.x, 0.8f, gridTile.transform.position.z), trainDirection);
        return (gridTile, trainDirection);
    }

    public TrackTile GetTileByVector3(Vector3 vector)
    {
        var checkVector = new Vector3(vector.x, 0.0f, vector.z);
        return m_gridTiles.Find(gT => gT.transform.position.Equals(checkVector)).GetComponent<TrackTile>();
    }
    //TODO: Fix when train goes in to reversed turn
    public Direction SelectNextDirection(Vector3 position, Direction currentDirection)
    {
        var tile = m_gridTiles.Find(go =>
            go.transform.position.x == position.x && go.transform.position.z == position.z);

        if (tile.name.Contains("Left"))
        {
            switch (currentDirection)
            {
                case Direction.East:
                    return Direction.North;
                case Direction.North:
                    return Direction.West;
                case Direction.South:
                    return Direction.East;
                case Direction.West:
                    return Direction.South;
            }
        }
        else if (tile.name.Contains("Right"))
        {
            switch (currentDirection)
            {
                case Direction.East:
                    return Direction.South;
                case Direction.North:
                    return Direction.East;
                case Direction.South:
                    return Direction.West;
                case Direction.West:
                    return Direction.North;
            }
        }

        return currentDirection;
    }
}
public struct Limits
{
    public Transform BottomLeft;
    public Transform TopRight;
}