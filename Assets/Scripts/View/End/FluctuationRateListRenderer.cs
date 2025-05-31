using System.Linq;
using UnityEngine;

public class FluctuationRateListRenderer : MonoBehaviour
{
    [SerializeField] private GameObject scrollContent;

    private void OnEnable()
    {
        ClearChildren();
        CreateChildren();
    }

    private void CreateChildren()
    {
        TreadingSystem.instance.GetAccountPrice().ToList().ForEach(orders =>
        {
            var day = orders.Key + "일차";
            var money = orders.Value;

            var child = Instantiate(Resources.Load<GameObject>("Prefabs/FluctuationRate"), scrollContent.transform);
            var ui = child.GetComponent<FluctuationRateUI>();
            var childRect = child.GetComponent<RectTransform>();
            ui.UpdateValue(day, money);

            var contentRect = scrollContent.transform as RectTransform;
            if (contentRect)
                contentRect.sizeDelta =
                    new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + childRect.sizeDelta.y);
        });
    }

    private void ClearChildren()
    {
        var contentRect = scrollContent.transform as RectTransform;
        if (contentRect)
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 0);

        for (var i = 0; i < scrollContent.transform.childCount; i++)
        {
            var child = scrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}