using UnityEngine;
using UnityEngine.Events;

public class BattleStartedEvent : UnityEvent { }

//The battle has ended - pass through the winning train
public class BattleEndedEvent : UnityEvent<Train> { }
public class GridCompletedEvent : UnityEvent<TrackGrid> { }
public class GridInitialisedEvent : UnityEvent<TrackGrid> { }

public class EventManager : MonoBehaviour
{
    public BattleStartedEvent battleStarted;
    public BattleEndedEvent battleEnded;
    public GridCompletedEvent gridCompleted;
    public GridInitialisedEvent gridInitialised;

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

        if (gridInitialised == null)
        {
            gridInitialised = new GridInitialisedEvent();
        }

    }
}
