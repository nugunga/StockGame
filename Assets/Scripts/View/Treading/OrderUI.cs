using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentCompanyName;

    [SerializeField] private Image buyImage;
    [SerializeField] private TextMeshProUGUI buyRatioText;
    [SerializeField] private ulong buyPrice;
    [SerializeField] private TMP_InputField buyAmountText;
    [SerializeField] private Button buyButton;

    [SerializeField] private Image sellImage;
    [SerializeField] private TextMeshProUGUI sellRatioText;
    [SerializeField] private ulong sellPrice;
    [SerializeField] private TMP_InputField sellAmountText;
    [SerializeField] private Button sellButton;

    [SerializeField] private TextMeshProUGUI buyTotalText;

    private void Awake()
    {
        TreadingSystem.instance.changedCompany += InitUI;
        InitUI();
    }

    private void InitUI()
    {
        var company = TreadingSystem.instance.GetCurrentCompany();
        if (company == null) return;

        currentCompanyName.text = company.Value.name;

        buyAmountText.text = "";
        buyImage.fillAmount = 0f;
        buyRatioText.text = "None";
        sellAmountText.text = "";
        sellImage.fillAmount = 0f;
        sellRatioText.text = "None";

        var order = TreadingSystem.instance.GetCurrentCompanyOrder();
        sellButton.interactable = order != null;
        sellAmountText.interactable = order != null;
        buyTotalText.text = $"{order?.Price ?? 0}원 구매 예정";
    }

    public void UpdateBuyPrice(string price)
    {
        if (string.IsNullOrEmpty(price) || price.Length <= 0) return;

        buyPrice = ulong.Parse(price);
        UpdateUI();
    }

    public void UpdateSellPrice(string price)
    {
        if (string.IsNullOrEmpty(price) || price.Length <= 0) return;

        sellPrice = ulong.Parse(price);
        UpdateUI();
    }

    public void Buy()
    {
        TreadingSystem.instance.Buy(buyPrice);
        InitUI();
    }

    public void Sell()
    {
        TreadingSystem.instance.SellPart(sellPrice);
        InitUI();
    }

    private void UpdateUI()
    {
        var company = TreadingSystem.instance.GetCurrentCompany();
        if (company == null) return;

        UpdateBuyUI(company.Value);
        UpdateSellUI(company.Value);
        UpdateCommonUI(company.Value);
    }

    private void UpdateBuyUI(Company company)
    {
        var price = buyPrice == 0 ? 1 : buyPrice;
        buyImage.fillAmount = (float)price / MoneySystem.instance.GetMoney();
        buyRatioText.text = $"{(float)price / MoneySystem.instance.GetMoney() * 100:F2}%";
    }

    private void UpdateSellUI(Company company)
    {
        // 구매한 게 없다면, 업데이트할 게 없는 데 일단 있다고 가정하고 UI 렌더링 돌림.
        var order = TreadingSystem.instance.GetCurrentCompanyOrder();
        if (order == null) return;

        sellImage.fillAmount = (float)sellPrice / order.Price;
        sellRatioText.text = $"{(float)sellPrice / order.Price * 100:F2}%";
    }

    private void UpdateCommonUI(Company company)
    {
        currentCompanyName.text = company.name;
        var order = TreadingSystem.instance.GetCurrentCompanyOrder();
        buyTotalText.text = $"{order?.Price ?? 0}원 구매 예정";
    }
}