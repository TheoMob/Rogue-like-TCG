using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectOption : MonoBehaviour
{
    public void OnHoverEnter()
    {
        Text optionText = gameObject.GetComponent<Text>();
        optionText.fontSize += 4;
        //optionText.color = Color.gray;
    }
    public void OnHoverExit()
    {
        Text optionText = gameObject.GetComponent<Text>();
        optionText.fontSize -= 4;
        //optionText.color = Color.white;
    }

    public void OnClick()
    {
        EncounterManager encountermanager = GameObject.FindGameObjectWithTag("EncounterManager").GetComponent<EncounterManager>();
        if (gameObject.name == "Option 1")
            encountermanager.dialogOption = 1;
        if (gameObject.name == "Option 2")
            encountermanager.dialogOption = 2;
    }
}
