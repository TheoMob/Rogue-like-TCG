using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsStats : MonoBehaviour
{
    /* Esse script serve pra guardar os status das cartas, assim quando a função CardActivation
     * é chamada ele passe para o Battle Manager as informações da carta*/

    public float cardCost;
    public float cardDamage;
    public float cardHeal;
    public int cardDraw;
    public int cardShield;

    BattleManager battleManagerScript;
    public void CardActivation()
    {
        battleManagerScript = GameObject.FindGameObjectWithTag("Battle Manager Tag").GetComponent<BattleManager>();

            battleManagerScript.OnCardPlayed(gameObject.tag, cardDamage, cardCost, cardHeal, cardDraw, cardShield);
    }
}
