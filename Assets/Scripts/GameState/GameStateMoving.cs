using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMoving : BaseGameState
{
    public GameStateMoving(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.Moving)
    {

    }

    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessNodeClick(NodeMessage nodeMessage)
    {
        //if node is empty, move piece to node

        //else show error message
    }
}
