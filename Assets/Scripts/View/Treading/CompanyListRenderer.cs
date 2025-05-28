using System.Linq;
using ScrollCarousel;
using UnityEngine;
using UnityEngine.Events;

public class CompanyListRenderer : MonoBehaviour
{
    [SerializeField] private Carousel carousel;
    [SerializeField] private bool isButton;
    [SerializeField] private UnityEvent<Company> onClick;
    [SerializeField] private UnityEvent<Company> onChanged;

    private Company? currentCompany;
    private Company? prevCompany;

    private void Awake()
    {
        carousel = GetComponent<Carousel>();
        var children = CompanySystem.instance.Companies.Select(company =>
        {
            var child = Instantiate(Resources.Load<GameObject>("Prefabs/Company"), transform);
            child.name = company.name;

            var ui = child.GetComponent<CompanyUI>();
            ui.UpdateCompany(company);

            if (isButton)
            {
                var button = child.AddComponent<CarouselButton>();
                button.onClick.AddListener(() =>
                {
                    if (currentCompany != null) onClick.Invoke(currentCompany.Value);
                });
            }

            return child.GetComponent<RectTransform>();
        });

        carousel.Items = children.ToList();
    }

    private void Update()
    {
        var currentUI = GetCurrentItem();
        currentCompany = currentUI.GetCompany();
        if (prevCompany.HasValue && currentCompany != null &&
            prevCompany.Value.name == currentCompany.Value.name) return;
        prevCompany = currentCompany;
        if (currentCompany != null) onChanged.Invoke(currentCompany.Value);
    }

    public CompanyUI GetCurrentItem()
    {
        var index = carousel.GetCurrentItemIndex();
        return carousel.Items[index].GetComponent<CompanyUI>();
    }
}