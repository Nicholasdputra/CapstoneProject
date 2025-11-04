using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IslandManager : MonoBehaviour
{
    public int isInBossPhase;
    public static IslandManager Instance;
    private int currentIslandIndex = 0;
    public GameObject bossPrefab;

    public int CurrentIslandIndex
    {
        get => currentIslandIndex;
        set => currentIslandIndex = value;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GridManager.Instance.DetermineHeightOffset();
    }

    void Update()
    {

    }

    public void SpawnBoss()
    {
        (int occupiedCells, int totalCells) = GridManager.Instance.MarkGridAvailability();

        if ((totalCells - occupiedCells) < 1)
        {
            Debug.Log("Not enough space to spawn the boss!");
            return;
        } 

        Vector3 spawnPos = GridManager.Instance.GetSpawnPos();

        if (spawnPos == Vector3.zero)
        {
            Debug.LogWarning("No free spawn position available, stopping spawn.");
            return;
        }

        GameObject newObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        newObj.GetComponent<HarvestableObject>().myGridCell = GridManager.Instance.WorldToGrid(spawnPos);
            
        Vector2Int gridCell = GridManager.Instance.WorldToGrid(spawnPos);
        GridManager.Instance.SetCellOccupied(gridCell, true);
    }

    public void GoToBossIsland()
    {
        string bossSceneName = "BossIsland" + (CurrentIslandIndex + 1).ToString(); 
        WaveManager.Instance.isInMinibossPhase = 0;
        SceneManager.LoadScene(bossSceneName);
    }

    public void LoseAgainstBoss()
    {
        string bossSceneName = "ResourceIsland" + (CurrentIslandIndex + 1).ToString(); 
        SceneManager.LoadScene(bossSceneName);
        WaveManager.Instance.CurrentWaveIndex = 9;
        WaveManager.Instance.isInMinibossPhase = 0;
        WaveManager.Instance.DestroyedHarvestableObjectsCount = 0;
        WaveManager.Instance.SpawnWave();
    }
    
    public void WinAgainstBoss()
    {
        CurrentIslandIndex++;
        string nextIslandSceneName = "ResourceIsland" + (CurrentIslandIndex + 1).ToString(); 
        WaveManager.Instance.CurrentWaveIndex = 0;
        PlayerData.Instance.playerHUD.UpdateCurrentWaveIndexText();
        isInBossPhase = 0;
        WaveManager.Instance.DestroyedHarvestableObjectsCount = 0;
        StartCoroutine(ChangeToResourceIsland(nextIslandSceneName));
        isInBossPhase = 0;
    }

    public IEnumerator ChangeToResourceIsland(string nextIslandSceneName)
    {
        SceneManager.LoadScene(nextIslandSceneName);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Changing to Resource Island Scene");
        WaveManager.Instance.SpawnWave();
    }
}