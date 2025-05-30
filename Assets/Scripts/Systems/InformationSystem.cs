using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationSystem : Singleton<InformationSystem>
{
    [SerializeField] private List<Button> coinButtons;
    [SerializeField] private RectTransform payCoinsPane;
    [SerializeField] private TextMeshProUGUI conversationText;

    private Company? currentCompany;

    public void Reset()
    {
        InitUI();
        UpdateButtonInteraction();
    }

    protected override void OnSingletonAwake()
    {
        Reset();
    }

    private void InitUI()
    {
        payCoinsPane.gameObject.SetActive(false);
    }

    private void UpdateButtonInteraction()
    {
        for (var i = 0; i < coinButtons.Count; i++)
        {
            coinButtons[i].interactable = CoinSystem.instance.GetCoin() > (ulong)i;
            // 혹은 구매했다면, interactable은 false임.
            if (currentCompany != null)
            {
                var needCoin = i == 0 ? NeedCoins.One : i == 1 ? NeedCoins.Two : NeedCoins.Three;
                var isBuy = CompanySystem.instance.IsBuyScenario(currentCompany.Value, needCoin);
                var isNotEnoughCoin = CoinSystem.instance.GetCoin() >= (ulong)i + 1;
                coinButtons[i].interactable = !isBuy && isNotEnoughCoin;
            }
        }
    }

    private void UpdateConversation(NeedCoins coins)
    {
        // 현재 활성화된 company를 가지고 시나리오를 조회해서 해당 시나리오의 
        if (currentCompany.HasValue == false) return;
        CompanySystem.instance.BuyScenario(currentCompany.Value, coins);
        InitUI();
        UpdateButtonInteraction();
        conversationText.text =
            CompanySystem.instance.GetCurrentScenario(currentCompany.Value).coinInformation[coins].name;
    }

    public void OnChangeCompany(Company company)
    {
        currentCompany = company;
        InitUI();
    }

    public void OnSelectCompany(Company company)
    {
        currentCompany = company;
        payCoinsPane.gameObject.SetActive(true);
        UpdateButtonInteraction();
    }

    public void OnExitPayCoins()
    {
        payCoinsPane.gameObject.SetActive(false);
    }

    public void OnPayOneCoin()
    {
        CoinSystem.instance.PayCoin(1);
        UpdateConversation(NeedCoins.One);
    }

    public void OnPayTwoCoins()
    {
        CoinSystem.instance.PayCoin(2);
        UpdateConversation(NeedCoins.Two);
    }

    public void OnPayThreeCoins()
    {
        CoinSystem.instance.PayCoin(3);
        UpdateConversation(NeedCoins.Three);
    }
}