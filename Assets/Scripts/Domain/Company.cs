using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Company
{
    public string name;
    public string description;
    public Sprite ci;
    public List<Scenario> scenarios;

    public Company(string name, string description, Sprite ci, List<Scenario> scenarios)
    {
        this.name = name;
        this.description = description;
        this.ci = ci;
        this.scenarios = scenarios;
    }
}