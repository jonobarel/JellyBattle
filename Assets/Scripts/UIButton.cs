using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIButton : MonoBehaviour
{
    // two colours for customizing UI items
    public Color color1;
    public Color color2;

    private string label;
    public string Label {
        get {return label;}
        set {label = value;}
    }

    public abstract void Action();
}
