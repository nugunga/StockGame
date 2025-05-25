using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreadingSystem : Singleton<TreadingSystem>
{
    [SerializeField] private TextMeshProUGUI currentCompanyName;
    private Dictionary<int, Order[]> account;
    private Company? currentCompany;
    [SerializeField] private List<Order> orders;

    private void Update()
    {
        if (currentCompany == null) return;
        currentCompanyName.text = currentCompany.Value.name;
    }

    public void UpdateCurrentCompany(Company company)
    {
        currentCompany = company;
    }

    public ulong GetOrderPrice(Company company)
    {
        var order = orders.Find(order1 => order1.CompanyName == company.name);
        if (order == null) return 0;
        return order.Price;
    }

    public ulong GetOrderPrice()
    {
        if (currentCompany.HasValue == false) Debug.LogError("Current Company is null");
        return GetOrderPrice(currentCompany.Value);
    }

    public void Buy(ulong price)
    {
        if (currentCompany.HasValue == false) Debug.LogError("Current Company is null");
        var company = currentCompany.Value;

        var money = MoneySystem.instance.GetMoney();
        var needMoney = (long)money - (long)price;
        if (needMoney < 0) return;
        MoneySystem.instance.PayMoney((ulong)needMoney);

        var order = new Order(name = company.name, price);
        orders.Add(order);
    }

    public void Sell(Company company)
    {
        var price = GetOrderPrice();
        if (price <= 0) return;
        MoneySystem.instance.GiveMoney(price);
        orders.Remove(orders.Find(order => order.CompanyName == company.name));
    }

    public void Sell()
    {
        if (currentCompany != null) Sell(currentCompany.Value);
    }

    public void SellAll()
    {
        // 가지고 있는 order를 account에 저장하고, give머니하면됨
        var currentDay = DaySystem.instance.GetDay();
        account.Add(currentDay, orders.ToArray());

        orders.ForEach(order =>
        {
            var companySystem = CompanySystem.instance;
            var price = companySystem.CalculatePrice(order);
            MoneySystem.instance.GiveMoney(price);
        });
        orders.Clear();
    }
}