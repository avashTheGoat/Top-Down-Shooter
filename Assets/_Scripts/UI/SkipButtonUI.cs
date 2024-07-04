using UnityEngine;
using UnityEngine.UI;
using System;

public class SkipButtonUI : MonoBehaviour
{
    public event Action OnSkip;

    [field: SerializeField] public Button Button { get; private set; }

    private void Awake() => Button.onClick.AddListener(() => OnSkip?.Invoke());
}