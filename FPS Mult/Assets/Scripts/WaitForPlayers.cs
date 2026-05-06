using UnityEngine;
using PurrNet.StateMachine;
using PurrNet;
using System.Collections;

public class WaitForPlayers : StateNode
{
    [SerializeField] private int minPlayerCount = 2;

    public override void Enter(bool asServer)
    {
        base.Enter(asServer);
        if (!asServer) return;
        StartCoroutine(WaitForPlayersJoin());
    }

    private IEnumerator WaitForPlayersJoin()
    {
        while (networkManager.players.Count < minPlayerCount)
        {
            yield return null;
        }
        machine.Next();
    }
}
