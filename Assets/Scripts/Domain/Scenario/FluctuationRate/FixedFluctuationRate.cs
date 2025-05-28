using System;
using UnityEngine;

[Serializable]
public class FixedFluctuationRate : IFluctuationRate
{
    [SerializeField] private float value;


    public FixedFluctuationRate(float value)
    {
        this.value = value;
    }

    public double GetValue()
    {
        return value;
    }
}