using TMPro;
using UnityEngine;

public class FluctuationRateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI fluctuationText;

    public void UpdateValue(string day, ulong fluctuation)
    {
        dayText.text = day;
        fluctuationText.text = MoneySystem.instance.MoneyToString(fluctuation);
    }
}