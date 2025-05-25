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
}