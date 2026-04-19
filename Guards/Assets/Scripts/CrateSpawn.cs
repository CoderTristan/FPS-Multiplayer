using UnityEngine;

public class CrateSpawn : MonoBehaviour
{
    [Header("Cluster Prefabs")]
    public GameObject[] clusterPrefabs;

    [Header("Grid Settings")]
    public int rows = 10;
    public int columns = 10;

    public float xSpacing = 20f;
    public float zSpacing = 20f;

    private void Start()
    {
        GenerateCrates();
    }

    void GenerateCrates()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (Random.value < 0.20f)
                    continue;

                SpawnRandomCluster(r, c);
            }
        }
    }

    void SpawnRandomCluster(int r, int c)
    {
        if (clusterPrefabs.Length == 0)
        {
            Debug.LogWarning("No cluster prefabs assigned!");
            return;
        }

        GameObject prefab = clusterPrefabs[Random.Range(0, clusterPrefabs.Length)];

        Vector3 pos = transform.position +
                      new Vector3(c * xSpacing, 0, r * zSpacing);

        Instantiate(prefab, pos, Quaternion.identity, transform);
    }
}
