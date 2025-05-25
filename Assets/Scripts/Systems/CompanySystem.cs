using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class CompanySystem : Singleton<CompanySystem>
{
    [SerializeField] private List<Company> companies = new();
    [SerializeField] private SerializedDictionary<string, Scenario> currentScenarios = new();
    public List<Company> Companies => companies;


    protected override void OnSingletonAwake()
    {
        UpdateCompanies();
    }

    private void UpdateCompanies()
    {
        if (companies.Count > 0) return;
        companies.AddRange(new[]
        {
            new Company
            {
                name = "접어캐피탈", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "혼돈의 금융 위기에 신규 금융사, 접어 캐피탈 등장 널널한 대출 전략으로 많은 고객들이 찾아.",
                            fluctuationRate = new IFixedFluctuationRate(1.2)
                        }
                    })
            },
            new Company
            {
                name = "델로메디컬", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "델로 메디컬, 많은 환자들을 구한다는 모토로 비뇨기과에서 종합 의료로 전환.",
                            fluctuationRate = new IFixedFluctuationRate(1.1)
                        }
                    })
            },
            new Company
            {
                name = "그레이아트", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "디자인 회사 그레이 아트의 신규 사업이 선정성 관련 문제로 제재를 받아.",
                            fluctuationRate = new IFixedFluctuationRate(0.7)
                        }
                    })
            },
            new Company
            {
                name = "지읒토피아", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "여행사, 지읒 토피아가 미개척지에 대한 패키지 혀행을 발표, 사람들은 우려를 표해",
                            fluctuationRate = new IFixedFluctuationRate(0.9)
                        }
                    })
            },
            new Company
            {
                name = "여까북스", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "매니악한 팬층들이 선호하는 전문서적만을 다룬 여까북스의 판매량은 저조",
                            fluctuationRate = new IFixedFluctuationRate(0.8)
                        }
                    })
            },
            new Company
            {
                name = "위치스 포션", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "제약회사 위치스 포션, 모 기업인 의료 회사에서 독립하여 독자적인 노선 선택",
                            fluctuationRate = new IFixedFluctuationRate(1.15)
                        }
                    })
            },
            new Company
            {
                name = "이화컬쳐", description = "", scenarios = new List<Scenario>(
                    new[]
                    {
                        new Scenario
                        {
                            name = "서브컬처 산업 침체기에 신규 회사인 이와 컬처가 상장",
                            fluctuationRate = new IFixedFluctuationRate(1.1)
                        }
                    })
            }
        });
    }

    public ulong CalculatePrice(Order order)
    {
        var company = companies.Find(company => company.name == order.CompanyName);
        var scenario = currentScenarios[company.name];
        return (ulong)(order.Price * scenario.fluctuationRate.GetValue());
    }
}