public struct CoinInformation
{
    public NeedCoins needCoins;
    public string name;
    public bool isBuy;

    public CoinInformation(CoinInformation coinInformation)
    {
        needCoins = coinInformation.needCoins;
        name = coinInformation.name;
        isBuy = coinInformation.isBuy;
    }

    public CoinInformation(NeedCoins needCoins, string name)
    {
        this.needCoins = needCoins;
        this.name = name;
        isBuy = false;
    }
}