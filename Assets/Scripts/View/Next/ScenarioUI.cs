using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioUI : MonoBehaviour
{
    [SerializeField] private Scenario currentScenario;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI scenarioText;

    public void UpdateScenario(Scenario scenario)
    {
        currentScenario = scenario;
        UpdateUI();
    }

    public void UpdateUI()
    {
        image.sprite = currentScenario.image;
        scenarioText.text = currentScenario.name;
    }
}