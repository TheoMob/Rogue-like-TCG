using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardToHand : MonoBehaviour /* esse script nada mais é do que uma continuação do PlayerDeck, focado em mover cartas do Deck e cemitério, além de embaralhar */
{
    public GameObject playerHand;
    public GameObject Deck;
    public GameObject[] cardsOfDeck; // so, there's a bug where he shows one card ocupying all the positions, but actually it works fine
    public GameObject Dropzone;
    public GameObject[] cardsOfGY;
    private PlayerManager playerManagerScript;


    public int draw = 0;
    public bool move = false;
    public bool sendCardsBack = false;
    public float speed;
    public float minDistance = 1; // isso serve pra carta não ter que estar exatamente em cima da mesa, mas sim mais ou menos em cima

    int howManyCard;


    private void Start()
    {
        playerManagerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>(); // isso é p acessar o deckCounter
    }
    void Update()
    {

        RecoverCards(); // isso é o script que coloca as cartas do GY para o deck
        ReadDeck(); // esse é o script que constantemente le quais cartas ainda estão no Deck


        if (move && !sendCardsBack)
        {
            MoveCard();
        }
        if (draw > 0 && !sendCardsBack)
        {
            drawCards();
        }
        if (sendCardsBack)
        {
            SendBack();
        }

    }

    private void MoveCard()
    {
        int playerZoneCards = playerHand.transform.childCount;

        if (playerZoneCards >= 5)
        {
            move = false;
            return;
        }

        changeCardSide(); // isso é pra virar a carta p cima

        cardsOfDeck[0].transform.position = Vector2.MoveTowards(cardsOfDeck[0].transform.position, playerHand.transform.position, (speed * Time.deltaTime));

        if(Vector2.Distance(cardsOfDeck[0].transform.position, playerHand.transform.position) < minDistance)
        {
            cardsOfDeck[0].transform.SetParent(playerHand.transform);
            playerManagerScript.DeckCounter(); // isso serve pra manter um contador de quantas cartas há no deck

            if (Deck.transform.childCount == 0)
            {
                sendCardsBack = true;
            }
        }  
    }

    public void ReadDeck()
    {
        howManyCard = Deck.transform.childCount;

        for (int i = 0; i < howManyCard; i++)
        {
            cardsOfDeck[i] = Deck.transform.GetChild(i).gameObject;
        }
    }

    public void RecoverCards()
    {
        for (int i = 0; i < Dropzone.transform.childCount; i++)
        {
            if (Dropzone.transform.GetChild(i).tag != "EnemyCard")
            cardsOfGY[i] = Dropzone.transform.GetChild(i).gameObject;
            else
            {
               GameObject batata = Dropzone.transform.GetChild(i).gameObject;
                Destroy(batata);
            }
        }

    }

    private void SendBack()
    {
        int cardsGY = Dropzone.transform.childCount;

        cardsOfGY[0].transform.position = Vector2.MoveTowards(cardsOfGY[0].transform.position, Deck.transform.position, (speed * Time.deltaTime));

        if (Vector2.Distance(cardsOfGY[0].transform.position, Deck.transform.position) < minDistance)
        {
            cardsOfGY[0].transform.SetParent(Deck.transform);
            playerManagerScript.DeckCounter(); // isso serve pra manter um contador de quantas cartas há no deck


            if (cardsGY == 0)
            {
                sendCardsBack = false;
                hideAllCards();
                shuffleDeck();
            }

        }
    }

    public void drawCards ()
    {
        if (Deck.transform.childCount == 0)
            sendCardsBack = true;

        changeCardSide(); // isso é pra trocar o lado da carta (true = carta pra cima; false = carta pra baixo)
        cardsOfDeck[0].transform.position = Vector2.MoveTowards(cardsOfDeck[0].transform.position, playerHand.transform.position, (speed * Time.deltaTime));


        if (Vector2.Distance(cardsOfDeck[0].transform.position, playerHand.transform.position) < minDistance)
        {
            cardsOfDeck[0].transform.SetParent(playerHand.transform);
            playerManagerScript.DeckCounter();
            draw--;
        }
    }

    public void shuffleDeck()
    {
        for (int i = 0; i < Deck.transform.childCount; i++)
        {
            int rnd = Random.Range(0, Deck.transform.childCount);
            Transform child = Deck.transform.GetChild(i);
            child.SetSiblingIndex(rnd);
        }
    }

    private void changeCardSide()  // isso tudo é pra quando a carta virar pra cima, o nome, texto e custo apareçam
    {
        cardsOfDeck[0].transform.GetChild(0).gameObject.SetActive(true);
        cardsOfDeck[0].transform.GetChild(2).gameObject.SetActive(false);
    }
    private void hideAllCards()
    {
        for(int i = 0; i < Deck.transform.childCount; i++)
        {
            cardsOfDeck[i].transform.GetChild(0).gameObject.SetActive(false);
            cardsOfDeck[i].transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}
