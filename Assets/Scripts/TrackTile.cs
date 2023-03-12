using UnityEngine;

public abstract class TrackTile : MonoBehaviour
{
    public Vector2Int m_position;
    [SerializeField] protected bool m_isReplaceable;
    [SerializeField] public Direction m_orientation = Direction.North;
    [SerializeField] private float m_popHeight = 0.2f;
    [SerializeField] private LeanTweenType m_popEase = LeanTweenType.notUsed;
    [SerializeField] private LeanTweenType m_scaleEase = LeanTweenType.notUsed;
    [SerializeField] private float m_scaleSpeed = 0.45f;
    [SerializeField] private float m_popSpeed = 0.75f;

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
        LeanTween.scale(gameObject, Vector3.one * 1.25f, m_scaleSpeed).setEase(m_scaleEase);
        LeanTween.moveY(gameObject, m_popHeight, m_popSpeed).setEase(m_popEase);
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

    public abstract bool CanApproach(Direction direction);

}
