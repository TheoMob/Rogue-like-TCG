using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentionsManager : MonoBehaviour
{
    public Sprite[] intentionSprites;

    public Text enemyIntentionNumber;
    public GameObject enemyIntentionsHelp;
    public Text enemyIntentionHelpText;
    public Image enemyIntentionImage;



    public void IntentionManager(string Intention, float number)
    {
        if (Intention == "Damage")
        {
            enemyIntentionNumber.enabled = true;
            enemyIntentionNumber.text = number.ToString();
            enemyIntentionHelpText.text = "The enemy intends to attack you";
            enemyIntentionImage.sprite = intentionSprites[0];
        }

        if (Intention == "Debuff")
        {
            enemyIntentionNumber.enabled = false;
            enemyIntentionHelpText.text = "The enemy intends to cast a debuff on you";
            enemyIntentionImage.sprite = intentionSprites[2];
        }
        
    }

    public void onHoverEnter()
    {
        enemyIntentionsHelp.SetActive(true);
    }
    public void onHoverExit()
    {
        enemyIntentionsHelp.SetActive(false);
    }
}
