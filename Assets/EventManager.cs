using UnityEngine;
using UnityEngine.Events;

public class BattleStartedEvent : UnityEvent { }
public class BattleEndedEvent : UnityEvent { }

public class EventManager : MonoBehaviour
{
    public UnityEvent battleStarted;
    public UnityEvent battleEnded;

    private void Awake()
    {
        if (battleStarted == null)
        {
            battleStarted = new BattleStartedEvent();
        }

        if (battleEnded == null)
        {
            battleEnded = new BattleEndedEvent();
        }
    }
}
