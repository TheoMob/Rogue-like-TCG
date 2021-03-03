using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject canvas;
    private GameObject zoomCard;
    public GameObject zoomPlace;

    public float sizeX, sizeY;
    public void Awake()
    {
        canvas = GameObject.Find("Main Canvas");   // acha o canvas e coloca dentro da variavel canvas

        zoomPlace = GameObject.Find("CardZoomPos");
    }

    public void OnHoverEnter()
    {
        if (transform.parent.tag == "Deck" || transform.parent.name == "Main Canvas" || transform.parent.name == "CardsLootWindow") // não dar zoom caso a carta esteja no deck ou esteja sendo movida
            return;
        if (transform.parent.tag == "CardsInventory") // não dar zoom caso esteja na Bonfire Scene (deckOrganizer)
            return;


        zoomCard = Instantiate(gameObject, new Vector2(0, 0), Quaternion.identity);
        zoomCard.transform.SetParent(zoomPlace.transform, false);
        zoomCard.layer = LayerMask.NameToLayer("Zoom");


        RectTransform rect = zoomCard.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(sizeX, sizeY);
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}
