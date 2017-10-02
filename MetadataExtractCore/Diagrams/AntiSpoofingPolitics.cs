using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class AntiSpoofingPolitics
{
    public string Name;
    public string Value;

    public AntiSpoofingPolitics()
    {

    }

    public AntiSpoofingPolitics(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

