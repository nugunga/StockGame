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
        var dto = JsonUtility.FromJson<Results>(json.text);

        return dto.companies.Select(companyDto =>
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
                            ? new FixedFluctuationRate(scenarioDto.fluctuationRate.value)
                            : new RangedFluctuationRate(scenarioDto.fluctuationRate.min,
                                scenarioDto.fluctuationRate.max);

                    var dictionary = new SerializedDictionary<NeedCoins, CoinInformation>();
                    foreach (var coinInformationDto in scenarioDto.coinInformation)
                    {
                        var needCoins = Enum.Parse<NeedCoins>(coinInformationDto.needCoins);
                        dictionary.Add(needCoins, new CoinInformation(needCoins, coinInformationDto.name));
                    }

                    var imagePath = scenarioDto.image;
                    var image = Resources.Load<Sprite>(imagePath);

                    return new Scenario(
                        scenarioDto.name,
                        fluctuationRate,
                        dictionary,
                        image
                    );
                }).ToList());
        }).ToList();
    }

    [Serializable]
    public class CoinInformationDto
    {
        public string needCoins;
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
        public float max;
        public float min;
        public float value;
    }

    [Serializable]
    public class ScenarioDto
    {
        public List<CoinInformationDto> coinInformation;
        public FluctuationRateDto fluctuationRate;
        public string name;
        public string image;
    }

    [Serializable]
    public class CompanyDto
    {
        public string description;
        public string image;
        public string name;
        public List<ScenarioDto> scenarios;
    }

    public class Results
    {
        public List<CompanyDto> companies;
    }
}