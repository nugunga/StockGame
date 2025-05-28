using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RangedFluctuationRate : IFluctuationRate
{
    [SerializeField] private float min;
    [SerializeField] private float max;

    public RangedFluctuationRate(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public double GetValue()
    {
        return Random.Range(min, max);
    }
}