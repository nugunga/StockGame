using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class TreadingSystem : Singleton<TreadingSystem>
{
    [SerializeField] private List<Order> orders;
    [SerializeField] private SerializedDictionary<int, Order[]> account;
    [SerializeField] private SerializedDictionary<int, ulong> accountPrice;
    public UnityAction changedCompany;
    private Company? currentCompany;
    private Company? prevCompany;

    public void Reset()
    {
        orders = new List<Order>();
        account = new SerializedDictionary<int, Order[]>();
        accountPrice = new SerializedDictionary<int, ulong>();
        accountPrice.Add(DaySystem.instance.GetDay(), MoneySystem.instance.GetMoney());
    }

    protected override void OnSingletonAwake()
    {
        Reset();
    }

    public void UpdateCurrentCompany(Company company)
    {
        prevCompany = currentCompany;
        currentCompany = company;
        if (prevCompany.HasValue && prevCompany.Value.name == company.name) return;
        if (changedCompany != null) changedCompany.Invoke();
    }

    public ulong GetOrderPrice(Company company)
    {
        var order = orders.Find(order1 => order1.companyName == company.name);
        if (order == null) return 0;
        return order.price;
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

        // 만약에 order가 있다면, 해당 order 업데이트
        if (orders.Exists(order => order.companyName == company.name))
        {
            var order = orders.Find(order => order.companyName == company.name);
            order.price += price;
        }
        else
        {
            var order = new Order(name = company.name, price);
            orders.Add(order);
        }
    }

    public void Sell(Company company)
    {
        var price = GetOrderPrice();
        if (price <= 0) return;
        MoneySystem.instance.GiveMoney(price);
        orders.Remove(orders.Find(order => order.companyName == company.name));
    }

    public void Sell()
    {
        if (currentCompany != null) Sell(currentCompany.Value);
    }

    public void SellPart(ulong price)
    {
        // 부분 판매.
        if (currentCompany == null) return;
        var order = orders.Find(order => order.companyName == currentCompany.Value.name);
        if (order == null) return;

        if (order.price < price)
        {
            Sell();
            return;
        }

        order.price -= price;
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
            MoneySystem.instance.GiveMoney((ulong)price);
        });
        orders.Clear();
        accountPrice.Add(currentDay, MoneySystem.instance.GetMoney());
    }

    public Company? GetCurrentCompany()
    {
        return currentCompany;
    }

    [CanBeNull]
    public Order GetCurrentCompanyOrder()
    {
        if (currentCompany.HasValue == false) return null;
        return orders.Find(order => order.companyName == currentCompany.Value.name);
    }

    public Dictionary<int, Order[]> GetAccount()
    {
        return account;
    }

    public Dictionary<int, ulong> GetAccountPrice()
    {
        return accountPrice;
    }
}