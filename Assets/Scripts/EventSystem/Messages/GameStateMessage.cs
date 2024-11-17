public class GameStateMessage : GameEventMessage
{
    public GameStateType PreviousStateType { get; private set; }
    public GameStateType CurrentStateType { get; private set; }
    public PlayerData CurrentPlayer { get; private set; }
    public PlayerData OpponentPlayer { get; private set; }

    public GameStateMessage(GameStateType previousStateType, GameStateType currentStateType, PlayerData currentPlayer, PlayerData opponentPlayer)
    {
        PreviousStateType = previousStateType;
        CurrentStateType = currentStateType;
        CurrentPlayer = currentPlayer;
        OpponentPlayer = opponentPlayer;
    }

    public GameStateMessage(string eventName, GameStateType previousStateType, GameStateType currentStateType, PlayerData currentPlayer, PlayerData opponentPlayer) : base(eventName)
    {
        PreviousStateType = previousStateType;
        CurrentStateType = currentStateType;
        CurrentPlayer = currentPlayer;
        OpponentPlayer = opponentPlayer;
    }
}
