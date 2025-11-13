using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO", menuName = "ScriptableObjects/WaveSO", order = 1)]
public class WaveSO : ScriptableObject
{
    public int waveNumber;
    public GameObject[] objectPrefabs;
    public int minEnemies;
    public int maxEnemies;
    public MinibossSO minibossData;
}