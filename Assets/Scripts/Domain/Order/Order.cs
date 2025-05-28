using System;

[Serializable]
public class Order
{
    public string companyName;
    public ulong price;

    public Order(string companyName, ulong price)
    {
        this.companyName = companyName;
        this.price = price;
    }
}