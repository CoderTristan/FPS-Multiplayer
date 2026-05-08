using UnityEngine;
using PurrNet;
using TMPro;

public class EndGameView : View
{
    [SerializeField] private TMP_Text winnerText;


    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);
    }

    private void OnDestroy()
    {
        InstanceHandler.UnregisterInstance<GameViewManager>();
    }

    public void SetWinner(PlayerID winner)
    {
        winnerText.text = $"Player {winner.id} has won the game!";
    }

    public override void OnHide()
    {
        
    }

    public override void OnShow()
    {
    
}

}
