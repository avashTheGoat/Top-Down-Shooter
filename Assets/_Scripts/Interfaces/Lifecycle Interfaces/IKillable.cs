using UnityEngine;
using System;

public interface IKillable
{
    public event Action<GameObject> OnKill;

    public void Kill();
}