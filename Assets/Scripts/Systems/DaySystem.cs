using TMPro;
using UnityEngine;

public class DaySystem : Singleton<DaySystem>
{
    [SerializeField] private int day;
    [SerializeField] private int maxDay;
    [SerializeField] private TextMeshProUGUI dayText;

    public void Reset()
    {
        day = 1;
    }

    private void Update()
    {
        if (!dayText) return;
        dayText.text = DayToString();
    }

    protected override void OnSingletonAwake()
    {
        Reset();
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

    public bool IsEnd()
    {
        return day > maxDay;
    }

    public bool IsLastDay()
    {
        return day == maxDay;
    }
}