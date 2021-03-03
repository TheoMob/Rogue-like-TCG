using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public GameObject[] cardsToGive;
    public GameObject CardsLoot;

    PlayerCardsInventory cardsInventoryScript;

    int[] cards = new int[5];
    void Start()
    {
        cardsInventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCardsInventory>();
    }
    public void GenerateCards()
    {
        for(int i = 0; i < 3; i++)
        {
            int rndCard = Random.Range(0, cardsToGive.Length - 1);
            GameObject cardChosed = Instantiate(cardsToGive[rndCard], new Vector3(0, 0, 0), Quaternion.identity);
            cardChosed.transform.SetParent(CardsLoot.transform);

            cards[i] = rndCard;
        }

    }

    public void AddCardsLootToInventory()
    {
        cardsInventoryScript.CardsInventory.Add(cardsToGive[cards[0]]);
        cardsInventoryScript.CardsInventory.Add(cardsToGive[cards[1]]);
        cardsInventoryScript.CardsInventory.Add(cardsToGive[cards[2]]);
    }
}
