using UnityEngine;
using TMPro;

public class EnemyBossSpawnReceiver : MonoBehaviour
{
    [SerializeField] private EnemyWavesSpawner spawner;

    [Header("UI")]
    [SerializeField] private GameObject bossBarParent;
    [SerializeField] private HealthBarUI bossBarUI;
    [SerializeField] private TextMeshProUGUI bossBarName;

    private void Start()
    {
        spawner.OnBossSpawn += (_boss, _bossName) =>
        {
            bossBarUI.SetDamageable(_boss.GetComponent<IDamageable>());
            bossBarName.text = _bossName;

            bossBarParent.SetActive(true);

            _boss.GetComponent<IKillable>().OnKill += _ => bossBarParent.SetActive(false);
        };
    }
}