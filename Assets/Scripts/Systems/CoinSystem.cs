using TMPro;
using UnityEngine;

public class CoinSystem : Singleton<CoinSystem>
{
    [SerializeField] private ulong coin = 5;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Update()
    {
        if (!coinText) return;
        coinText.text = coin.ToString();
    }

    public void PayCoin(ulong needCoins)
    {
        coin -= needCoins;
    }

    public void GiveCoin(ulong needCoins)
    {
        coin += needCoins;
    }

    public ulong GetCoin()
    {
        return coin;
    }
}