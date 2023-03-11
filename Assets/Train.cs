using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private Material m_material;
    [SerializeField] private Renderer m_renderer;
    public Direction m_direction = Direction.East;

    public Transform startMarker;

    public Vector3 endMarker = Vector3.negativeInfinity;

    public float speed = 1.0f;

    public Lookahead LookAheadTile = new Lookahead();

    private TrackGrid _trackGrid;
    public string UserId;

    private void Start()
    {
        _trackGrid = GameObject.Find("TrackGrid").GetComponent<TrackGrid>();
    }

    public void SetDestination(Vector3 end, Direction direction = Direction.East)
    {
        m_direction = direction;
        endMarker = end;

        var trueEnd = new Vector3(end.x, transform.position.y, end.z);
        endMarker = trueEnd;
        startMarker = transform;

        SetRotation(direction);

        SetLookahead(end, direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (endMarker != Vector3.negativeInfinity)
        {
            var position = transform.position;
            if (Vector3.Distance(position, endMarker) >= 0.02)
            {
                switch (m_direction)
                {
                    case Direction.East:
                        position = new Vector3(position.x + 0.5f * Time.deltaTime, position.y, position.z);
                        break;
                    case Direction.North:
                        position = new Vector3(position.x, position.y, position.z + 0.5f * Time.deltaTime);
                        break;
                    case Direction.West:
                        position = new Vector3(position.x - 0.5f * Time.deltaTime, position.y, position.z);
                        break;
                    case Direction.South:
                        position = new Vector3(position.x, position.y, position.z - 0.5f * Time.deltaTime);
                        break;
                }

                transform.position = position;
            }
            else
            {
                //snap to destination
                transform.position = new Vector3(endMarker.x, position.y, endMarker.z);
                var nextDirection = ChooseNextDirection();
                endMarker = nextDirection.Item1;
                m_direction = nextDirection.Item2;
                SetRotation(m_direction);
                Brain.ins.EventManager.trainDestinationUpdate?.Invoke(this);
            }
        }
    }

    private (Vector3, Direction) ChooseNextDirection()
    {
        return _trackGrid.GetNextTile(transform.position, m_direction);
    }

    public void Decorate(ColorSet newColorSet)
    {
        Material newMaterial = new Material(m_material);

        newMaterial.SetColor("_MainColor", newColorSet.MainColor);
        newMaterial.SetColor("_SecColor", newColorSet.SecColor);
        newMaterial.SetColor("_TertiaryColor", newColorSet.TertiaryColor);

        m_renderer.material = newMaterial;
    }

    private void SetRotation(Direction direction)
    {
        switch (m_direction)
        {
            case Direction.East:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
                break;
            case Direction.North:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
                break;
            case Direction.West:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270.0f, transform.eulerAngles.z);
                break;
            case Direction.South:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180.0f, transform.eulerAngles.z);
                break;
        }

    }

    public void SetLookahead(Vector3 position, Direction direction)
    {
        var lookaheadTile = GameObject.Find("TrackGrid").GetComponent<TrackGrid>().GetNextTile(position, m_direction);
        LookAheadTile = new Lookahead(new Vector2Int((int)lookaheadTile.Item1.x, (int)lookaheadTile.Item1.z),
            direction);
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}

public struct Lookahead
{
    public Lookahead(Vector2Int postition, Direction direction)
    {
        this.postition = postition;
        this.direction = direction;
    }
    public Vector2Int postition;
    public Direction direction;
}
