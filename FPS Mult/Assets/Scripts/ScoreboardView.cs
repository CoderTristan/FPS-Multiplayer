using UnityEngine;
using System.Collections.Generic;
using PurrNet;

public class ScoreboardView : View
{
    [SerializeField] private Transform scoreboardEntryContainer;
    [SerializeField] private ScoreboardEntry scoreboardEntryPrefab;
    private GameViewManager _gameViewManager;

    private void Start()
    {
        _gameViewManager = InstanceHandler.GetInstance<GameViewManager>();
    }
    

    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _gameViewManager.ShowView<ScoreboardView>(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _gameViewManager.HideView<ScoreboardView>();
        }
    }

    private void OnDestroy()
    {
        InstanceHandler.UnregisterInstance<ScoreboardView>();
    }

    public void SetData(Dictionary<PlayerID, ScoreManager.ScoreData> data)
    {
        foreach (Transform child in scoreboardEntryContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var playerScore in data)
        {
            var entry = Instantiate(scoreboardEntryPrefab, scoreboardEntryContainer);
            entry.SetData(playerScore.Key.id.ToString(), playerScore.Value.kills, playerScore.Value.deaths);
        }
    }
    public override void OnHide()
    {
        
    }

    public override void OnShow()
    {
        
    }
}
