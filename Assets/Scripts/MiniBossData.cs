using UnityEngine;

[CreateAssetMenu(fileName = "MiniBossData", menuName = "ScriptableObjects/MiniBossData", order = 1)]
public class MiniBossData : ScriptableObject
{
    public int IslandNumber;
    public MinibossSO[] minibosses;
}
