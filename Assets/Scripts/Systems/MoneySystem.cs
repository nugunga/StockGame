using TMPro;
using UnityEngine;

public class MoneySystem : Singleton<MoneySystem>
{
    [SerializeField] private ulong initMoney;
    [SerializeField] private ulong currentMoney;
    [SerializeField] private TextMeshProUGUI moneyText;

    public void Reset()
    {
        currentMoney = initMoney;
    }

    private void Update()
    {
        if (!moneyText) return;
        moneyText.text = MoneyToString();
    }

    protected override void OnSingletonAwake()
    {
        Reset();
    }

    public ulong GetMoney()
    {
        return currentMoney;
    }

    public void PayMoney(ulong needCoins)
    {
        currentMoney -= needCoins;
    }

    public void GiveMoney(ulong needCoins)
    {
        currentMoney += needCoins;
    }

    private string MoneyToString()
    {
        // 000,000,000
        return $"{currentMoney:n0}";
    }
}