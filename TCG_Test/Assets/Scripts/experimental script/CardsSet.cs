using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsSet : MonoBehaviour
{
    public RectTransform cardTransform1;
    public RectTransform cardTransform2;
    public RectTransform cardTransform3;
    public RectTransform cardTransform4;
    public Animator cardAnimator1;
    public Animator cardAnimator2;
    public Animator cardAnimator3;
    public Animator cardAnimator4;
    public float moveSpeed;

    private bool cardMove1, cardMove2, cardMove3, cardMove4 = false;
    
    void Start()
    {
        //cardAnimator.GetComponent<Animator>(); // eu botei o animator no lugar errado, então isso n funciona, mas como é só pra testes foda-se
    }

    // Update is called once per frame
    void Update()
    {
        CardDealer();
    }

    public void CardDealer()
    {
        if (Input.GetKeyDown(KeyCode.Space) || cardMove1) // trigger para começar o movimento das cartas
        {
            cardTransform3.position = new Vector3(Mathf.MoveTowards(cardTransform3.position.x, 350, moveSpeed * Time.deltaTime), cardTransform1.position.y, cardTransform3.position.z);
            cardMove1 = true; // as booleanas chamadas cardMove servem só pra garantir que todas as cartas ja se moveram antes que elas virem
        }

        if (cardTransform3.position.x < 355)
        {
            cardTransform2.position = new Vector3(Mathf.MoveTowards(cardTransform2.position.x, 250, moveSpeed * Time.deltaTime), cardTransform1.position.y, cardTransform2.position.z);
            cardMove2 = true;
        }

        if (cardTransform2.position.x < 255)
        {
            cardTransform1.position = new Vector3(Mathf.MoveTowards(cardTransform1.position.x, cardTransform4.position.x, moveSpeed * Time.deltaTime), cardTransform1.position.y, cardTransform1.position.z);
            //cardTransform1.position = new Vector3(Mathf.MoveTowards(cardTransform1.position.x, 150, moveSpeed * Time.deltaTime), cardTransform1.position.y, cardTransform1.position.z);
            cardMove3 = true;

            // caso use um valor qualquer numérico para mover as cartas, o tamanho da tela vai influenciar diretamente na posição das mesmas
            // convém colocar um RectTransform no alvo onde se quer que a carta repouse, pois assim ela ficará em uma posição fixa indepentemente do tamanho da tela
        }

        if (cardTransform1.position.x < 155)
        {
            cardMove4 = true;
        }



        if (cardMove1 && cardMove2 && cardMove3 && cardMove4) // caso todas as cartas ja tenham se movido, então liga o flip de todas elas
        {
            cardAnimator1.SetBool("CardFlipStart", true);
            cardAnimator2.SetBool("CardFlipStart", true);
            cardAnimator3.SetBool("CardFlipStart", true);
        }

       

    }
}
