using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTile: MonoBehaviour
{
    private TileState m_state;
    private Vector2Int m_position;

    public TrackTile()
    {
        m_state = TileState.Empty;
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

    public void SetState(TileState tileState, Vector2Int position)
    {
        if (!Equals(m_state, tileState))
        {
            m_state = tileState;
            SetTileMesh();
        }

        m_position = position;
    }

    private void SetTileMesh()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        switch (m_state)
        {
            case TileState.Empty:
                renderer.material.color = Color.white;
                break;
            case TileState.Track:
                renderer.material.color = Color.black;
                break;
        }
    }
}

public enum TileState {
    Empty,
    Track
}