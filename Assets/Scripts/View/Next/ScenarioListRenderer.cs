using System.Linq;
using ScrollCarousel;
using UnityEngine;

public class ScenarioListRenderer : MonoBehaviour
{
    [SerializeField] private Carousel carousel;

    private void Awake()
    {
        carousel = GetComponent<Carousel>();
    }

    private void OnEnable()
    {
        // 현재일의 현재 시나리오들을 가져와서 업데이트 시킴.
        // 모든 자식을 삭제
        ClearChildren();
        CreateChildren();
    }

    private void ClearChildren()
    {
        carousel.Items.Clear();
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void CreateChildren()
    {
        var children = CompanySystem.instance.Companies.Select(company =>
        {
            var scenario = CompanySystem.instance.GetCurrentScenario(company);
            var child = Instantiate(Resources.Load<GameObject>("Prefabs/Scenario"), transform);
            var ui = child.GetComponent<ScenarioUI>();

            ui.UpdateScenario(scenario);
            return child.GetComponent<RectTransform>();
        });

        carousel.Items = children.ToList();
    }
}