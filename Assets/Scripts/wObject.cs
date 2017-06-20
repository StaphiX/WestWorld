using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class defines anything in the game world
public class wObject
{
    public string Name { get; set; }
    public PhysicalProperties PhysProp { get; set; }

    public wObject()
    {
        SetDefaults();
    }

    protected virtual void SetDefaults() { SetName("Unknown");  }

    protected void SetName(string sName) { Name = sName; }
}

public class PhysicalProperties
{
    public PhysicalProperties() { }

    uint iWeight = 1000; //Weight in Grams
}

