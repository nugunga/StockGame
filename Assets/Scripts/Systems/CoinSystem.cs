using System;
using TMPro;
using UnityEngine;

public class CoinSystem: Singleton<CoinSystem>
{
    [SerializeField] private ulong coin = 5;
    [SerializeField] private TextMeshProUGUI coinText;

    public void PayCoin(ulong needCoins)
    {
        coin -= needCoins;
    }

    public void GiveCoin(ulong needCoins)
    {
        coin += needCoins;
    }

    private void Update()
    {
        if (!this.coinText) return;
        this.coinText.text = coin.ToString();
    }
}
