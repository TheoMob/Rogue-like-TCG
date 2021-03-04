using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs")]
public class DialogScriptableObject : ScriptableObject
{
    public List<DialogData> DialogAmount = new List<DialogData>();
}
