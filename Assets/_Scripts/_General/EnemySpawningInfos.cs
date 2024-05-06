using System.Collections.Generic;

[System.Serializable]
public struct EnemySpawningInfos
{
    public List<EnemySpawningInfo> SpawningInfos;

    public EnemySpawningInfos(List<EnemySpawningInfo> _infos)
    {
        SpawningInfos = _infos;
    }
}