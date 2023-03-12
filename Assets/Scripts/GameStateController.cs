namespace DefaultNamespace
{
    public class GameStateController
    {
        public GameState CurrentState = GameState.Waiting;
    }
}

public enum GameState
{
    Waiting,
    Playing,
    Finished
}