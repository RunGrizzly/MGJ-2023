using UnityEngine;
using UnityEngine.Events;

public class BattleStartedEvent : UnityEvent { }
public class BattleEndedEvent : UnityEvent { }
public class GridCompletedEvent : UnityEvent<TrackGrid> { }

public class EventManager : MonoBehaviour
{
    public BattleStartedEvent battleStarted;
    public BattleEndedEvent battleEnded;
    public GridCompletedEvent gridCompleted;

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

        if (gridCompleted == null)
        {
            gridCompleted = new GridCompletedEvent();
        }

    }
}
