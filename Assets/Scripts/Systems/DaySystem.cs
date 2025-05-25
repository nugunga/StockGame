using TMPro;
using UnityEngine;

public class DaySystem : Singleton<DaySystem>
{
    [SerializeField] private int day = 1;
    [SerializeField] private int maxDay = 7;
    [SerializeField] private TextMeshProUGUI dayText;

    private void Update()
    {
        if (!dayText) return;
        dayText.text = DayToString();
    }

    private string DayToString()
    {
        // ${day}/{day}
        return $"{day}/{maxDay}";
    }

    public int GetDay()
    {
        return day;
    }

    public void NextDay()
    {
        day++;
    }
}