using UnityEngine;

[CreateAssetMenu(fileName = "Night Enemy Spawning Infos", menuName = "Scriptable Objects/Enemy/Night Spawning Info")]
public class EntireNightEnemySpawningInfos : ScriptableObject
{
    [Tooltip("Animation curve that specifies the number of enemies that will spawn (y) on a certain wave (x)."
    + "AT EVERY INTEGER WAVE OF THE CURVE, THE Y-VALUE ROUNDED UP MUST BE DIFFERENT TO THE PREVIOUS INTEGER " +
    "WAVE Y-VALUE ROUNDED UP")]
    public AnimationCurve NumEnemiesAtWave;
    public AnimationCurve SecsToSpawnEnemiesAtWave;
    public EnemySpawningInfos[] SpawnableEnemiesAtWave;
    public bool IsEndless = false;
}