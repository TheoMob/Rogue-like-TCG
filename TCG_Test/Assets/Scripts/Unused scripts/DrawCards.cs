using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script is obsolete and i'm not using it anymore, i'm using the script PlayerDeck now
public class DrawCards : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject card6;
    public GameObject card7;
    public GameObject card8;
    public GameObject card9;
    public GameObject card10;
    public GameObject card11;
    public GameObject card12;
    public GameObject card13;
    public GameObject card14;
    public GameObject card15;
    public GameObject playerArea;
    public GameObject enemyArea;

    List<GameObject> cards = new List<GameObject>(); // this list is to randomize wich card is going to be dealt to the player and the enemy's hands
                                                    // but since i only have one test card for now, i'll live it like that

    void Start()
    {
        cards.Add(card1); // add the cards to the list of cards, only one card is being added in the moment
        cards.Add(card2);
        cards.Add(card3);
        cards.Add(card4);
        cards.Add(card5);
        cards.Add(card6);
        cards.Add(card7);
        cards.Add(card8);
        cards.Add(card9);
        cards.Add(card10);
        cards.Add(card11);
        cards.Add(card12);
        cards.Add(card13);
        cards.Add(card14);
        cards.Add(card15);
    }

    public void OnClick()
    {
        int cardsInTheTable = playerArea.transform.childCount; // armazena quantas cartas já tem na mesa

        for (int i = cardsInTheTable; i < 5; i++)
        {
            if (cards.Count <= 1)
            {
                Debug.Log("there's no more cards in the deck");
            }
            else
            {
                int randomIndex = Random.Range(1, cards.Count); // it starts on one because card number 0 is the backCard

                GameObject playerCard = Instantiate(cards[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
                // GameObject playerCard = Instantiate(cards[Random.Range(1, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity); // instantiate one random card from the card list in the playerArea
                playerCard.transform.SetParent(playerArea.transform, false);

               GameObject enemyCard = Instantiate(cards[randomIndex], new Vector3(0, 0, 0), Quaternion.identity); // Draw the cards for the enemy
                enemyCard.transform.SetParent(enemyArea.transform, false);
                enemyCard.tag = "EnemyCard";

                cards.RemoveAt(randomIndex); // remove the card that was instanciated from the deck
            
            }

        }

    }
}
