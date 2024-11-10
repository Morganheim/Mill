using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlacing : BaseGameState
{
    public GameStatePlacing(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.Placing)
    {

    }

    public override void OnStateEnter()
    {
        //show active player

        //show possible action (placing)
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        //if node is empty, place piece

        //else show error message
    }
}
