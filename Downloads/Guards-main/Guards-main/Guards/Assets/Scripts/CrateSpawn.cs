using UnityEngine;

public class CrateSpawn : MonoBehaviour
{
    [Header("Cluster Prefabs")]
    public GameObject[] clusterPrefabs;

    [Header("Special Prefabs (spawned in skip spot)")]
    public GameObject[] specialPrefabs;

    [Header("Item to spawn inside special crate")]
    public GameObject insideItem;   // ONE item type

    [Header("Grid Settings")]
    public int rows = 10;
    public int columns = 10;

    public float xSpacing = 20f;
    public float zSpacing = 20f;

    private bool specialPlaced = false;

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
                Vector3 pos = transform.position +
                              new Vector3(c * xSpacing, 0, r * zSpacing);

                if (Random.value < 0.12f)
                {
                    TryPlaceSpecialPrefab(pos);
                    continue;
                }

                SpawnRandomCluster(pos);
            }
        }
    }

    void SpawnRandomCluster(Vector3 pos)
    {
        if (clusterPrefabs.Length == 0)
        {
            Debug.LogWarning("No cluster prefabs assigned!");
            return;
        }

        GameObject prefab = clusterPrefabs[Random.Range(0, clusterPrefabs.Length)];
        Instantiate(prefab, pos, Quaternion.identity, transform);
    }

    void TryPlaceSpecialPrefab(Vector3 pos)
    {
        if (specialPlaced) return;

        if (specialPrefabs.Length == 0)
        {
            Debug.LogWarning("No special prefabs assigned!");
            return;
        }

        GameObject special = Instantiate(
            specialPrefabs[Random.Range(0, specialPrefabs.Length)],
            pos,
            Quaternion.identity,
            transform
        );

        specialPlaced = true;
        Debug.Log("Special prefab placed at: " + pos);

        SpawnInsideCrate(special);
    }

    void SpawnInsideCrate(GameObject crate)
    {
        if (insideItem == null)
        {
            Debug.LogWarning("No inside item assigned!");
            return;
        }

        Transform crateInterior = null;

        foreach (Transform child in crate.transform)
        {
            if (child.CompareTag("Crate"))
            {
                crateInterior = child;
                break;
            }
        }

        if (crateInterior == null)
        {
            Debug.LogWarning("No child with tag 'Crate' found inside special prefab!");
            return;
        }

        Transform[] spawnPoints = crateInterior.GetComponentsInChildren<Transform>();

        int count = 0;

        foreach (Transform point in spawnPoints)
        {
            if (point == crateInterior) continue;

            for (int i = 0; i < 3; i++)
            {
                Instantiate(
                    insideItem,
                    point.position,
                    point.rotation,
                    crate.transform
                );

                count++;
            }
        }

        Debug.Log("Spawned " + count + " items inside crate.");
    }
}
