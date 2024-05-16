using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 1)]
public class EnemyConfig : ScriptableObject
{
    public int spawnCost;
    public int bulletNumber;
    public Color color; // Example to change the color
}