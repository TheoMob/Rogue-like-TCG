using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public GameObject[] cardsPrefabs;
    public GameObject playerArea;
    private GameObject deckZone;

     public List<GameObject> cards = new List<GameObject>(); // this list is to randomize wich card is going to be dealt to the player

    void Start()
    {
        for(int i = 0; i < cardsPrefabs.Length; i++)
        {
            if(cardsPrefabs[i] != null)
                cards.Add(cardsPrefabs[i]);
        }
    }

    public void InstantiateDeck()
    {
        deckZone = GameObject.FindGameObjectWithTag("Deck"); // isso ta fora do start pra caso eu use essa função isolada

        for (int i = 0; i < cards.Count; i++) // isso é pra instanciar todas as cartas no deck
        {

            GameObject playerCard = Instantiate(cards[i], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(deckZone.transform, false);

            playerCard.transform.GetChild(0).gameObject.SetActive(false);
            playerCard.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}

