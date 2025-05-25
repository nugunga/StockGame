using UnityEngine;

public class Order
{
    [SerializeField] private readonly string companyName;
    [SerializeField] private readonly ulong price;

    public Order(string companyName, ulong price)
    {
        this.companyName = companyName;
        this.price = price;
    }

    public string CompanyName => companyName;
    public ulong Price => price;
}