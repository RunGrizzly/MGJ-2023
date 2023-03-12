using UnityEngine;


public class GameStateController: MonoBehaviour
{
    public GameState currentState = GameState.Waiting;
}


public enum GameState
{
    Waiting,
    Playing,
    Finished
}