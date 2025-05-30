using TMPro;
using UnityEngine;

public class CoinSystem : Singleton<CoinSystem>
{
    [SerializeField] private ulong initCoin;
    [SerializeField] private ulong currentCoin;
    [SerializeField] private TextMeshProUGUI coinText;

    public void Reset()
    {
        currentCoin = initCoin;
    }

    private void Update()
    {
        if (!coinText) return;
        coinText.text = currentCoin.ToString();
    }

    protected override void OnSingletonAwake()
    {
        Reset();
    }

    public void PayCoin(ulong needCoins)
    {
        if (currentCoin < needCoins) return;
        currentCoin -= needCoins;
    }

    public void GiveCoin(ulong needCoins)
    {
        currentCoin += needCoins;
    }

    public ulong GetCoin()
    {
        return currentCoin;
    }
}