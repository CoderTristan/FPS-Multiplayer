using UnityEngine;
using PurrNet.StateMachine;
using System.Linq;
using PurrNet;
using System.Collections.Generic;

public class GameEndState : StateNode
{
    public override void Enter(bool asServer)
    {
        base.Enter(asServer);

        if (!InstanceHandler.TryGetInstance(out ScoreManager scoreManager))
        {
            Debug.LogError("No ScoreManager found in scene!");
            return;
        }
        var winner = scoreManager.GetWinner();
        if (winner == default)
        {
            Debug.LogError("No winner found in scene!");
            return;
        }

        Debug.Log($"{winner} won the game!");
    }
}
