using System;
using UnityEngine;

[Serializable]
public class IFixedFluctuationRate : IFluctuationRate
{
    [SerializeField] private double value;


    public IFixedFluctuationRate(double value)
    {
        this.value = value;
    }

    public double GetValue()
    {
        return value;
    }
}