using UnityEngine;

public class GameManager : Singleton<MonoBehaviour>
{
    [SerializeField] private CanvasController canvasController;

    public void NextOrEnd()
    {
        if (DaySystem.instance.IsLastDay())
            EndGame();
        else
            ProcessNextDay();
    }

    public void EndGame()
    {
        DaySystem.instance.NextDay();
        TreadingSystem.instance.SellAll();
        canvasController.ChangeEndScene();
    }

    public void NextDay()
    {
        canvasController.ChangeNextScene();
    }

    public void ProcessNextDay()
    {
        // 다음날로 넘김
        DaySystem.instance.NextDay();

        // order 기반으로 판매
        TreadingSystem.instance.SellAll();

        // 코인 지급
        CoinSystem.instance.GiveCoin(1);

        // Scenario 다음걸로 업데이트
        CompanySystem.instance.UpdateScenario();

        // 이동
        canvasController.ChangeMainScene();
    }

    public void ResetGame()
    {
        DaySystem.instance.Reset();
        CoinSystem.instance.Reset();
        CompanySystem.instance.Reset();
        InformationSystem.instance.Reset();
        MoneySystem.instance.Reset();
        TreadingSystem.instance.Reset();
        canvasController.ChangeEntryScene();
    }
}