using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebuffBarManager : MonoBehaviour
{
    [SerializeField] private GameObject debuffBuffBar;
    [SerializeField] private GameObject confusionPrefab;
    [SerializeField] private GameObject helpTextPreFab;

    private Transform mainCanvas;
    private Text helpTextText;
    private GameObject confusionDebuff;
    private GameObject helpText;

    private void Start()
    {
        helpTextText = helpTextPreFab.transform.GetChild(0).GetComponent<Text>();
    }
    public void debuffConfusion(bool activate)
    {
        if(activate)
        {
            confusionDebuff = Instantiate(confusionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            confusionDebuff.transform.SetParent(debuffBuffBar.transform);
        }
        else
        {
            Destroy(confusionDebuff);
        }
    }
    public void OnHoverEnterDebuffConfusion()
    {
        helpTextText = helpTextPreFab.transform.GetChild(0).GetComponent<Text>();
        helpTextText.text = "you are confused and can't choose attack targets.";
        helpText = Instantiate(helpTextPreFab, new Vector3(0, 0, 0), Quaternion.identity);
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        helpText.transform.SetParent(mainCanvas, false);
        helpText.transform.position = new Vector2(Input.mousePosition.x + 150, Input.mousePosition.y - 150);

    }
    public void OnHoverExitDebuffConfusion()
    {
        Destroy(helpText);
    }
}
