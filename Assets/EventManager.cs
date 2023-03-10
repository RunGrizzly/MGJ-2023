using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleStartedEvent : UnityEvent { }

//The battle has ended - pass through the winning train
public class BattleEndedEvent : UnityEvent<(Train, bool)> { }
public class GridCompletedEvent : UnityEvent<TrackGrid> { }
public class GridInitialisedEvent : UnityEvent<TrackGrid> { }
public class BattleReadyEvent : UnityEvent<List<string>> { }
public class TrainDestinationUpdateEvent : UnityEvent<Train> { }

public class EventManager : MonoBehaviour
{
    public BattleStartedEvent battleStarted;
    public BattleEndedEvent battleEnded;
    public GridCompletedEvent gridCompleted;
    public GridInitialisedEvent gridInitialised;
    public BattleReadyEvent battleReady;
    public TrainDestinationUpdateEvent trainDestinationUpdate;

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

        if (battleReady == null)
        {
            battleReady = new BattleReadyEvent();
        }

        if (trainDestinationUpdate == null)
        {
            trainDestinationUpdate = new TrainDestinationUpdateEvent();
        }
    }
}
