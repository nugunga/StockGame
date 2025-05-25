using System;

[Serializable]
public class Order
{
    private ulong price;

    public Order(string companyName, ulong price)
    {
        this.CompanyName = companyName;
        this.price = price;
    }

    public string CompanyName { get; }

    public ulong Price
    {
        get => price;
        set => price = value;
    }
}