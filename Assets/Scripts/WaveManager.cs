using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int harvestableObjectsCount = 10;
    public int HarvestableObjectsCount
    {
        get => harvestableObjectsCount;
        set => harvestableObjectsCount = Mathf.Max(0, value);
    }

    public GameObject harvestableObjectPrefab;
    public GameObject[] harvestableObjectSpawns;
    public int totalWaves = 10;

    private int currentWave = 0;
    public int CurrentWave
    {
        get => currentWave;
        set => currentWave = Mathf.Clamp(value, 0, totalWaves);
    }

    // Spawn New Wave
    public void SpawnWave()
    {
        // Instantiate a harvestable object at the spawns
        foreach (GameObject spawnPoint in harvestableObjectSpawns)
        {
            // Spawn a harvestable object at the spawn point
            GameObject newHarvestableObject = Instantiate(harvestableObjectPrefab, spawnPoint.transform.position, Quaternion.identity);

        }
    }
    
    

    // Move to Next Island

}