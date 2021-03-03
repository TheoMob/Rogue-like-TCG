using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonfireManager : MonoBehaviour
{
    [SerializeField] Text PageText;
    [SerializeField] Text deckAmountText;
    [SerializeField] GameObject[] cardsInventoryPage;
    [SerializeField] Transform playerPos;

    private GameObject player;
    private PlayerCardsInventory cardsInventoryScript;
    private GameObject deckOrganizer; // isso é toda a aba do deckOrganizer
    private Transform deckPos; // isso é posição do deck para organizar as cartas nele
    private PlayerDeck playerDeckScript;
    public List<GameObject> Deckcopy = new List<GameObject>();

    private bool starting = true;
    private bool starting2 = true;
    private int pageIndex = 1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerDeckScript = player.GetComponent<PlayerDeck>();
        cardsInventoryScript = player.GetComponent<PlayerCardsInventory>();
        deckPos = GameObject.FindGameObjectWithTag("Deck").transform;
        deckOrganizer = GameObject.FindGameObjectWithTag("DeckOrganizer"); // o nome deckOrganizer se refere a aba de organização no total

        deckOrganizer.SetActive(false);

        player.transform.position = playerPos.position;
    }

    public void ShowDeckCards()
    {
        if(starting)
        {
            for(int i = 0; i < playerDeckScript.cards.Count; i++)
            {
                Deckcopy.Add(playerDeckScript.cards[i]);
            }

            for(int i = 0; i < Deckcopy.Count; i++)
            {
                GameObject deckCard = Instantiate(Deckcopy[i], new Vector3(0, 0, 0), Quaternion.identity);
                deckCard.transform.SetParent(deckPos, false);
                deckCard.transform.GetChild(0).gameObject.SetActive(false); // frente
                deckCard.transform.GetChild(1).gameObject.SetActive(true); // miniatura
                deckCard.transform.GetChild(2).gameObject.SetActive(false); // tras
            }

            deckAmountText.text = deckPos.childCount + "/30";

            starting = false;
        }
    }
    
    public void ShowInventoryCards()
    {
        if (starting2)
        {

            for (int i = 0; i < cardsInventoryScript.CardsInventory.Count; i++)
            {
                int actualPageIndex = InventoryPageManager();

                GameObject inventoryCard = Instantiate(cardsInventoryScript.CardsInventory[i], new Vector3(0, 0, 0), Quaternion.identity);
                inventoryCard.transform.SetParent(cardsInventoryPage[actualPageIndex].transform, false);
                inventoryCard.transform.GetChild(0).gameObject.SetActive(true); // liga a frente da carta
                inventoryCard.transform.GetChild(1).gameObject.SetActive(false); // desliga a miniatura da carta
                inventoryCard.transform.GetChild(2).gameObject.SetActive(false); // desliga as costas da carta
            }

            starting2 = false;
        }

    }

    public void RemoveCardFromDeck(GameObject cardToRemove)
    {
        int actualPageIndex = InventoryPageManager();

        cardToRemove.transform.SetParent(cardsInventoryPage[actualPageIndex].transform, false);
        cardToRemove.transform.GetChild(0).gameObject.SetActive(true);
        cardToRemove.transform.GetChild(1).gameObject.SetActive(false);
        cardToRemove.transform.GetChild(2).gameObject.SetActive(false);

        deckAmountText.text = deckPos.transform.childCount.ToString() + "/30";

        for (int i = Deckcopy.Count - 1; i > -1; i--) // remove todas as cartas da copia do deck
        {
            Deckcopy.RemoveAt(i);
        }

        for(int i = 0; i < deckPos.transform.childCount; i++) // adiciona todas as cartas que estão na DeckList a copia do deck
        {
            Deckcopy.Add(deckPos.transform.GetChild(i).gameObject);
        } 
    }

    public void AddCardToDeck(GameObject cardToAdd)
    {
        cardToAdd.transform.SetParent(deckPos.transform, false);
        cardToAdd.transform.GetChild(0).gameObject.SetActive(false); // desativa a frente da carta
        cardToAdd.transform.GetChild(1).gameObject.SetActive(true); // liga a miniatura da carta

        deckAmountText.text = deckPos.transform.childCount.ToString() + "/30";

        Deckcopy.Add(cardToAdd);

        OrganizeInventory();
    }

    private void OrganizeInventory()
    {

        for (int i = 0; i < 9; i++)
        {
            if (cardsInventoryPage[i].transform.childCount < 12 && cardsInventoryPage[i + 1].transform.childCount > 0)
                cardsInventoryPage[i + 1].transform.GetChild(0).SetParent(cardsInventoryPage[i].transform);
        }
    }

    private int InventoryPageManager()
    {
        for (int i = 0; i < 9; i++)
        {
            if (cardsInventoryPage[i].gameObject.transform.childCount < 12)
                return i;
        }
        return 9;
    }
    public void NextInventoryPage()
    {
        pageIndex++;
        if (pageIndex > 10)
            pageIndex = 10;
        PageText.text = "Page " + pageIndex;

        for(int i = 0; i < 10; i++)
        {
            cardsInventoryPage[i].SetActive(false);
        }
        cardsInventoryPage[pageIndex - 1].SetActive(true);
    }
    public void PreviousInventoryPage()
    {
        pageIndex--;
        if (pageIndex < 1)
            pageIndex = 1;
        PageText.text = "Page " + pageIndex;

        for (int i = 0; i < 10; i++)
        {
            cardsInventoryPage[i].SetActive(false);
        }
        cardsInventoryPage[pageIndex - 1].SetActive(true);
    }
    public void SaveDeckChanges()
    {
        PlayerCardsInventory cardsInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCardsInventory>();
        int cardsInInventory = cardsInventory.CardsInventory.Count;
        int cardsInDeck = playerDeckScript.cards.Count;

        for(int i = 0; i < cardsInInventory; i++)
        {
            cardsInventory.CardsInventory.RemoveAt(0);
        }

        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < cardsInventoryPage[i].transform.childCount; j++)
            {
                cardsInventory.CardsInventory.Add(cardsInventoryPage[i].transform.GetChild(j).gameObject);
            }
        } 

        for(int i = 0; i < cardsInDeck; i++)
        {
            playerDeckScript.cards.RemoveAt(0);
        }
        for(int i = 0; i < deckPos.transform.childCount; i++)
        {
            playerDeckScript.cards.Add(deckPos.transform.GetChild(i).gameObject);
        }
    }
}
