using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct Scenario
{
    public string name;
    public Sprite image;
    public NewsType newsType => fluctuationRate.GetValue() > 1 ? NewsType.Good : NewsType.Bad;
    public IFluctuationRate fluctuationRate;
    [SerializeField] public SerializedDictionary<NeedCoins, CoinInformation> coinInformation;

    public Scenario(string name, IFluctuationRate fluctuationRate,
        SerializedDictionary<NeedCoins, CoinInformation> coinInformation,
        Sprite image)
    {
        this.name = name;
        this.fluctuationRate = fluctuationRate;
        this.coinInformation = coinInformation;
        this.image = image;
    }

    public void BuyInformation(NeedCoins coins)
    {
        if (coinInformation.ContainsKey(coins))
        {
            var infm = new CoinInformation(
                coinInformation[coins]
            );
            infm.isBuy = true;

            coinInformation[coins] = new CoinInformation(infm);
        }
    }

    public Scenario Clone()
    {
        return new Scenario(name, fluctuationRate, coinInformation, image);
    }

    public Scenario Buy(NeedCoins coins)
    {
        var scenario = Clone();
        scenario.BuyInformation(coins);
        return scenario;
    }
}