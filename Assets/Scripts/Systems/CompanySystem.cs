using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class CompanySystem : Singleton<CompanySystem>
{
    [SerializeField] private List<Company> companies;
    [SerializeField] private SerializedDictionary<string, Scenario> currentScenarios;
    [SerializeField] private string resourcePath;
    public List<Company> Companies => companies;

    protected override void OnSingletonAwake()
    {
        companies = new CompanyLoader().Load(resourcePath);
        UpdateScenario();
    }

    public double CalculatePrice(Order order)
    {
        var company = companies.Find(company => company.name == order.companyName);
        var scenario = currentScenarios[company.name];
        return order.price * scenario.fluctuationRate.GetValue();
    }

    public void UpdateScenario()
    {
        var day = DaySystem.instance.GetDay();
        currentScenarios = new SerializedDictionary<string, Scenario>();
        companies.ForEach(company => currentScenarios.Add(company.name, company.scenarios[day - 1]));
    }

    public Scenario GetCurrentScenario(Company company)
    {
        return currentScenarios[company.name];
    }

    public void BuyScenario(Company company, NeedCoins coins)
    {
        currentScenarios[company.name] = currentScenarios[company.name].Clone().Buy(coins);
    }

    public bool IsBuyScenario(Company company, NeedCoins coins)
    {
        return currentScenarios[company.name].coinInformation[coins].isBuy;
    }
}