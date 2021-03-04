using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogData
{
    [TextArea(15, 20)] public string DialogBox;
    public int answersAmount;
    public string Answer1;
    public string Answer2;
    public string Answer3;
    public string Answer4;
}
