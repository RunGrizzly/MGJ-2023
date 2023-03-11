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

    [field: SerializeField] public SceneLoader SceneLoader { get; set; } = null;
    [field: SerializeField] public EventManager EventManager { get; set; } = null;
    [field: SerializeField] public BattleManager BattleManager { get; set; } = null;


}
