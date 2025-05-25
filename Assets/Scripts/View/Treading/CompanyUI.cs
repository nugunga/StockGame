using UnityEngine;
using UnityEngine.UI;

public class CompanyUI : MonoBehaviour
{
    [SerializeField] private Image image;
    private Company company;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateCompanyImage();
    }

    private void UpdateCompanyImage()
    {
        if (!company.ci) return;
        image.sprite = company.ci;
    }

    public Company? GetCompany()
    {
        return company;
    }

    public void UpdateCompany(Company company)
    {
        this.company = company;
    }
}