using System;

[Serializable]
public struct Scenario
{
    public string name;
    public NewsType newsType => fluctuationRate.GetValue() > 1 ? NewsType.Good : NewsType.Bad;
    public IFluctuationRate fluctuationRate;
}