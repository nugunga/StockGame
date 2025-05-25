using TMPro;
using UnityEngine;

public class MoneySystem : Singleton<MoneySystem>
{
    [SerializeField] private ulong money;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Update()
    {
        if (!moneyText) return;
        moneyText.text = MoneyToString();
    }

    public ulong GetMoney()
    {
        return money;
    }

    public void PayMoney(ulong needCoins)
    {
        money -= needCoins;
    }

    public void GiveMoney(ulong needCoins)
    {
        money += needCoins;
    }

    private string MoneyToString()
    {
        // 000,000,000
        return $"{money:n0}";
    }
}