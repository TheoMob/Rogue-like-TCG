using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsInventory : MonoBehaviour
{
  /*  public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4; */

    public List<GameObject> CardsInventory = new List<GameObject>();

    private void Start()
    {
       /* for (int i = 0; i < 3; i++)
        {
            CardsInventory.Add(card1);
            CardsInventory.Add(card2);
            CardsInventory.Add(card3);
            CardsInventory.Add(card4);
        }
        sortList(); */
    }

    public void AddCard(GameObject card)
    {
        CardsInventory.Add(card);
        sortList();
    }

    private void sortList()
    {

        CardsInventory.Sort(delegate (GameObject a, GameObject b)
        {
            return (a.name).CompareTo(b.name);
        });
    }
}
