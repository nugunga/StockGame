using System.Linq;
using ScrollCarousel;
using UnityEngine;

public class CompanyListRenderer : MonoBehaviour
{
    [SerializeField] private Carousel carousel;

    private void Awake()
    {
        carousel = GetComponent<Carousel>();
        var children = CompanySystem.instance.Companies.Select(company =>
        {
            var child = Instantiate(Resources.Load<GameObject>("Prefabs/Company"), transform);
            child.name = company.name;

            var ui = child.GetComponent<CompanyUI>();
            ui.UpdateCompany(company);

            return child.GetComponent<RectTransform>();
        });

        carousel.Items = children.ToList();
    }

    private void Update()
    {
        var currentUI = GetCurrentItem();
        var currentCompany = currentUI.GetCompany();
        if (!currentCompany.HasValue) return;
        TreadingSystem.instance.UpdateCurrentCompany(currentCompany.Value);
    }

    public CompanyUI GetCurrentItem()
    {
        var index = carousel.GetCurrentItemIndex();
        return carousel.Items[index].GetComponent<CompanyUI>();
    }
}