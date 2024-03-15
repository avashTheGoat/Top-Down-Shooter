using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MetalGameTimerUI : MonoBehaviour
{
    [SerializeField] private MetalGame metalGame;

    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();

    private void Start() => metalGame.GameTimer.OnTick += UpdateText;

    private void UpdateText(float _maxTime, float _timeElapsed) => text.text = $"{Mathf.RoundToInt(_maxTime - _timeElapsed)}";
}