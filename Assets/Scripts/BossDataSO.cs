using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObjects/BossData", order = 1)]
public class BossDataSO : ScriptableObject
{
    public BossSO[] bosses;
}
