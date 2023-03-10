using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public static Brain ins;

    private void Awake()
    {
        ins = this;
    }

    [SerializeField] private SceneLoader m_sceneLoader = null;


}
