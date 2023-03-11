using UnityEngine;

public abstract class TrackTile: MonoBehaviour
{
    public Vector2Int m_position;
    [SerializeField]protected bool m_isReplaceable;
    [SerializeField] public Direction m_orientation = Direction.North;
    protected TrackTile()
    {
        //Using -1 -1 to say we're "off-grid"
        m_position = new Vector2Int(-1, -1);
    }

    private void Start()
    {
        StartAnimation();
    }

    private void StartAnimation()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.25f, 0.75f).setEase(LeanTweenType.punch);
    }

    public void SetState(Vector2Int position, Direction orientation = Direction.North)
    {
        m_position = position;
        m_orientation = orientation;
        switch (m_orientation)
        {
            case Direction.East:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
                break;
            case Direction.West:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90.0f, transform.eulerAngles.z);
                break;
            case Direction.South:
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180.0f, transform.eulerAngles.z);
                break;
        }
    }
}
