using UnityEngine;
using TMPro;

public class EnemyBossBarManager : MonoBehaviour
{
    [HideInInspector] public HealthBarUI BossBarUI;
    [HideInInspector] public TextMeshProUGUI BossBarName;

    [SerializeField] private string bossName;

    private IDamageable damageable;

    private void Awake() => damageable = GetComponent<IDamageable>();

    private void Start()
    {
        BossBarUI.SetDamageable(damageable);
        BossBarName.text = bossName;
    }
}