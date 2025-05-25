using UnityEngine;

public class GameManager : Singleton<MonoBehaviour>
{
    [SerializeField] private CoinSystem coinSystem;
    [SerializeField] private MoneySystem moneySystem;
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private CanvasController canvasController;

    protected override void OnSingletonAwake()
    {
        coinSystem = CoinSystem.instance;
        moneySystem = MoneySystem.instance;
        daySystem = DaySystem.instance;
    }
}