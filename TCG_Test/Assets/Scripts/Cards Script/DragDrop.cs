using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private GameObject playerSide;
    private GameObject originalparent;
    private GameObject canvas;
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private GameObject player;
    CardsStats cardStats;
    PlayerManager playerManagerScript;
    BattleManager battleManagerScript;
    MoveCardToHand moveCardToHandScript;

    private Vector2 startPosition;

    private bool dontStartDrag = false;
    private bool dontEndDrag = false;
    private bool wrongButton = false;

    private void Awake()
    {
        cardStats = gameObject.GetComponent<CardsStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.Find("Main Canvas");
        playerSide = GameObject.Find("PlayerSide");

        moveCardToHandScript = player.GetComponent<MoveCardToHand>();
        playerManagerScript = player.GetComponent<PlayerManager>();

    }
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            isOverDropZone = true;
            dropZone = collision.gameObject; // make that dropZone becomes the object tha is colliding, in this case, the DropZone
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }

    public void StartDrag()
    {
        originalparent = transform.parent.gameObject;
        startPosition = transform.position;

        if (!Input.GetMouseButton(0))
        {
            wrongButton = true;
            return;
        }

        if (gameObject.transform.parent.tag == "CardsInventory" || gameObject.transform.parent.tag == "Deck")
        {
            StartDragOnDeckOrganizer();
            return;
        }

        DontDrag();
        if(dontStartDrag)
        {
            dontStartDrag = false;
            return;
        }


        isDragging = true;
    }

    private void DontDrag()
    {
        battleManagerScript = GameObject.FindGameObjectWithTag("Battle Manager Tag").GetComponent<BattleManager>(); 
                                                                                                                    
        if (moveCardToHandScript.move || moveCardToHandScript.sendCardsBack)                                        
            dontStartDrag = true;                                                                                   
        if (!battleManagerScript.playerTurn || transform.parent != playerSide.transform)
            dontStartDrag = true;
    }

    public void EndDrag() // doing some experiments here
    {
        if (wrongButton)
        {
            wrongButton = false;
            transform.position = startPosition;
            transform.SetParent(originalparent.transform);
        }

        if (originalparent.transform.tag == "CardsInventory" || originalparent.transform.tag == "Deck")
        {
             EndDragOnDeckOrganizer();
             return;
        }

        DontEndDrag(); 

        if(dontEndDrag)
        {
            dontEndDrag = false;
            return;
        }

        isDragging = false;
        if (isOverDropZone && !battleManagerScript.selectingEnemy)
        {
            if (playerManagerScript.currentMana < cardStats.cardCost) // se a mana do player for menor que o custo da carta, não deixe jogar a carta
            {
                transform.position = startPosition;
                transform.SetParent(playerSide.transform);
            }
            else //se tiver mana, protocolo normal
            {
                transform.SetParent(dropZone.transform, false);

                cardStats.CardActivation(); // activate the cards
            }
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(originalparent.transform);
        }

    }

    private void DontEndDrag()
    {
        battleManagerScript = GameObject.FindGameObjectWithTag("Battle Manager Tag").GetComponent<BattleManager>();

        if (transform.parent != canvas.transform || !battleManagerScript.playerTurn)
            dontEndDrag = true;

        if (moveCardToHandScript.move || moveCardToHandScript.sendCardsBack) // não deixe mecher nas cartas enquanto elas estiverem sendo movidas do deck pra mão e do GY pro Deck
            dontEndDrag = true;
    }

    private void StartDragOnDeckOrganizer()
    {
        originalparent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;

        if (originalparent.tag == "Deck")
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void EndDragOnDeckOrganizer()
    {
        isDragging = false;

        if (isOverDropZone && originalparent.tag == "CardsInventory" && GameObject.FindGameObjectWithTag("Deck").transform.childCount < 30) // isso é pra quando a carta está no catalogo e o Deck tiver menos de 15 cartas
        {
                GameObject.FindGameObjectWithTag("Bonfire").GetComponent<BonfireManager>().AddCardToDeck(gameObject); // adiciona a carta arrastada para o deck do player

        }
        else
        {
            if (originalparent.tag == "Deck")
            {
                if (isOverDropZone)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    transform.position = startPosition;
                    transform.SetParent(originalparent.transform);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Bonfire").GetComponent<BonfireManager>().RemoveCardFromDeck(gameObject);
                }
            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(originalparent.transform);
            }
        }
    }
}
