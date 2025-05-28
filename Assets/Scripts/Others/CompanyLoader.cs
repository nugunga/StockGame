using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CompanyLoader
{
    public List<Company> Load(string resourcePath)
    {
        var json = Resources.Load<TextAsset>(resourcePath);
        var dto = JsonUtility.FromJson<List<CompanyDto>>(json.text);

        return dto.Select(companyDto =>
        {
            var spritePath = companyDto.image;
            var sprite = Resources.Load<Sprite>(spritePath);

            return new Company(
                companyDto.name,
                companyDto.description,
                sprite,
                companyDto.scenarios.Select(scenarioDto =>
                {
                    IFluctuationRate fluctuationRate =
                        scenarioDto.fluctuationRate.type == FluctuationRateDto.FluctuationRateDtoType.FIXED_RATE
                            ? new FixedFluctuationRate((float)scenarioDto.fluctuationRate.value.Value)
                            : new RangedFluctuationRate((float)scenarioDto.fluctuationRate.min.Value,
                                (float)scenarioDto.fluctuationRate.max.Value);

                    var dictionary = new SerializedDictionary<NeedCoins, CoinInformation>();
                    foreach (var coinInformationDto in scenarioDto.coinInformation)
                        dictionary.Add(coinInformationDto.needCoins,
                            new CoinInformation(coinInformationDto.needCoins, coinInformationDto.name));

                    return new Scenario(
                        scenarioDto.name,
                        fluctuationRate,
                        dictionary
                    );
                }).ToList());
        }).ToList();
    }

    [Serializable]
    public class CoinInformationDto
    {
        public NeedCoins needCoins;
        public string name;
    }

    [Serializable]
    public class FluctuationRateDto
    {
        [Serializable]
        public enum FluctuationRateDtoType
        {
            FIXED_RATE
        }

        public FluctuationRateDtoType type;
        public double? max;
        public double? min;
        public double? value;
    }

    [Serializable]
    public class ScenarioDto
    {
        public List<CoinInformationDto> coinInformation;
        public FluctuationRateDto fluctuationRate;
        public string name;
    }

    [Serializable]
    public class CompanyDto
    {
        public string description;
        public string image;
        public string name;
        public List<ScenarioDto> scenarios;
    }
}