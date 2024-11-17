public abstract class BaseGameState
{
    public BaseGameState(GameStateManager gameStateManager, GameData gameData, GameStateType gameStateType)
    {
        b_GameStateManager = gameStateManager;
        b_GameData = gameData;
        StateType = gameStateType;
    }

    protected GameStateManager b_GameStateManager;
    protected GameData b_GameData;
    protected string b_Message;

    public GameStateType StateType { get; protected set; }

    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void ProcessNodeClick(Node node);

    public abstract void ProcessNodeHover(bool isEnter, Node node);

    public abstract void SwitchState();
}

public enum GameStateType { None = 0, Placing = 1, Moving = 2, Removing = 3, }