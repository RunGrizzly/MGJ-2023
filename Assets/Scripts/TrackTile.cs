using UnityEngine;

public abstract class TrackTile: MonoBehaviour
{
    public Vector2Int m_position;
    [SerializeField]protected bool m_isReplaceable;
    [SerializeField]protected Color m_mesh;
    [SerializeField] public Orientation orientation = Orientation.Horizontal;
    protected TrackTile()
    {
        //Using -1 -1 to say we're "off-grid"
        m_position = new Vector2Int(-1, -1);
    }

    private void Start()
    {
        SetTileMesh();
        StartAnimation();
    }

    private void StartAnimation()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.25f, 0.75f).setEase(LeanTweenType.punch);
    }

    public void SetState(Vector2Int position)
    {
        m_position = position;
    }

    protected void SetTileMesh()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = m_mesh;
    }
}

public enum Orientation
{
    Vertical,
    Horizontal
}