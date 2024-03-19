using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CounterUI : MonoBehaviour
{
    public int Count { get; private set; } = 0;

    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();

    public void SetCount(int _newCount)
    {
        Count = _newCount;
        text.text = GetCondensedNumber(Count);
    }

    private string GetCondensedNumber(int _num)
    {
        if (_num < 999)
            return _num.ToString();

        string _condensedNumber = "";
        if (_num >= 1000000)
            _condensedNumber = (_num / 1000000).ToString("0.0") + " M";

        else if (_num >= 1000)
            _condensedNumber = (_num / 1000).ToString("0.0") + " K";

        return _condensedNumber;
    }
}