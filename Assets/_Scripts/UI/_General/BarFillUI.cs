using UnityEngine;
using UnityEngine.UI;
using System;

public class BarFillUI : MonoBehaviour
{
    public float FillAmount
    {
        get => fillImage.fillAmount;
        set
        {
            if (value < 0f || value > 1f)
                throw new ArgumentException($"FillAmount cannot be less than 0 or greater than 1. It is {value}");

            fillImage.fillAmount = value;
        } 
    }

    [SerializeField] private Image fillImage;
}