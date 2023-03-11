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

    private float m_startTime;

    private float m_journeyLength;

    private float timeElapsed;




    public void SetDestination(Vector3 end)
    {
        m_direction = Direction.East;
        endMarker = end;

        var trueEnd = new Vector3(end.x, transform.position.y, end.z);
        //endMarker.position = new Vector3(position.x, 0.8f, position.z);
        endMarker = trueEnd;
        startMarker = transform;

        m_startTime = Time.time;

        m_journeyLength = Vector3.Distance(transform.position, endMarker);
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
            }
        }
    }

    private (Vector3, Direction) ChooseNextDirection()
    {
        return GameObject.Find("TrackGrid").GetComponent<TrackGrid>().GetNextTile(transform.position, m_direction);
        // var position = transform.position;
        //
        // switch (m_direction)
        // {
        //     case Direction.East:
        //         return new Vector3(position.x + 1, position.y, position.z);
        //     case Direction.North:
        //         return new Vector3(position.x, position.y, position.z + 1);
        //     case Direction.West:
        //         return new Vector3(position.x - 1, position.y, position.z);
        //     case Direction.South:
        //         return new Vector3(position.x, position.y, position.z - 1);
        // }
        //
        // return Vector3.forward;
    }


    public void Decorate(ColorSet newColorSet)
    {
        Material newMaterial = new Material(m_material);

        newMaterial.SetColor("_MainColor", newColorSet.MainColor);
        newMaterial.SetColor("_SecColor", newColorSet.SecColor);
        newMaterial.SetColor("_TertiaryColor", newColorSet.TertiaryColor);

        m_renderer.material = newMaterial;
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}
