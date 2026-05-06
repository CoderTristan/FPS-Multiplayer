using UnityEngine;
using PurrNet;
using System;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private SyncVar<int> health = new(100);
    [SerializeField] private int selfLayer, otherLayer;

    public Action<PlayerID> OnDeath_Server;
    protected override void OnSpawned()
    {
        base.OnSpawned();
        var actualLayer = isOwner ? selfLayer : otherLayer;
        SetLayerRecursive(gameObject, actualLayer);

        if (isOwner)
        {
            InstanceHandler.GetInstance<MainGameView>().UpdateHealth(health.value);
            health.onChanged += OnHealthChanged;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        health.onChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int newHealth)
    {
        InstanceHandler.GetInstance<MainGameView>().UpdateHealth(newHealth);
      
    }


    public int Health => health.value;

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    [ServerRpc(requireOwnership:false)]
    public void ChangeHealth(int amount, RPCInfo info = default)
    {
        health.value += amount;
        if (health.value <= 0)
        {
            if (InstanceHandler.TryGetInstance<ScoreManager>(out var scoreManager))
            {
                if (owner.HasValue)
                {
                    scoreManager.AddDeath(owner.Value);
                }
                scoreManager.AddKill(info.sender);

                
            }
            OnDeath_Server.Invoke(owner.Value);
            Destroy(gameObject);
        }
    }
   
}
