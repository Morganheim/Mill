using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateRemoving : BaseGameState
{
    public GameStateRemoving(GameManager gameManager, GameData gameData) : base(gameManager, gameData, GameStateType.Removing)
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
        //if node is occupied by opponent, remove piece

        //else if node is occupied by player, show error message "cannot remove your own pieces"

        //else, show error message for empty node
    }
}
