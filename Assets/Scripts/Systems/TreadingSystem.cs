using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TreadingSystem : Singleton<TreadingSystem>
{
    [SerializeField] private List<Order> orders;
    private Dictionary<int, Order[]> account;
    public Action changedCompany;
    private Company? currentCompany;
    private Company? prevCompany;

    protected override void OnSingletonAwake()
    {
        orders = new List<Order>();
    }

    public void UpdateCurrentCompany(Company company)
    {
        prevCompany = currentCompany;
        currentCompany = company;
        if (prevCompany.HasValue && prevCompany.Value.name == company.name) return;
        changedCompany.Invoke();
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
        MoneySystem.instance.PayMoney(price);

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

    public void SellPart(ulong price)
    {
        // 부분 판매.
        if (currentCompany == null) return;
        var order = orders.Find(order => order.CompanyName == currentCompany.Value.name);
        if (order == null) return;

        if (order.Price < price)
        {
            Sell();
            return;
        }

        order.Price -= price;
        MoneySystem.instance.GiveMoney(price);
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

    public Company? GetCurrentCompany()
    {
        return currentCompany;
    }

    [CanBeNull]
    public Order GetCurrentCompanyOrder()
    {
        if (currentCompany.HasValue == false) return null;
        return orders.Find(order => order.CompanyName == currentCompany.Value.name);
    }
}