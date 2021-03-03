using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    private GameObject player;
    private Animator playerAnim;
    private PlayerManager playerManagerScript;
    private Rigidbody2D playerRigid;
    private string eventText;
    public GameObject dialogBox;
    private Text dialogBoxText;
    private Text[] dialogOptionsText = new Text[5];
    private string[] dialogOptions = new string[5];
    private float delayTextTime = 0.08f;

    private Vector2 moveTo;
    private bool movePlayer;
    private bool startSomething = false;
    public float speed = 5;
    private float minDist = 1; // isso serve pra movimentar o player
    public int dialogOption = 0;

    public MyEnum Encounter = new MyEnum();
    public enum MyEnum
    {
        none,
        SkeletonInForest,
        test
    };

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnim = player.GetComponent<Animator>();
        playerManagerScript = player.GetComponent<PlayerManager>();
        playerRigid = player.GetComponent<Rigidbody2D>();
        dialogBoxText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        dialogOptionsText[0] = dialogBox.transform.GetChild(1).GetComponent<Text>();
        dialogOptionsText[1] = dialogBox.transform.GetChild(2).GetComponent<Text>();
        dialogBox.SetActive(false);

        randomEncounterSelector();

    }


    void Update()
    {
        if(movePlayer)
        {
            player.transform.position = new Vector2(Mathf.MoveTowards(player.transform.position.x, moveTo.x, (speed * Time.deltaTime)), player.transform.position.y);
            playerAnim.SetBool("Running", true);

            if (Vector2.Distance(player.transform.position, moveTo) < minDist)
            {
                movePlayer = false;
                playerAnim.SetBool("Running", false);
                startSomething = true;
                randomEncounterSelector();
            }
        } 
    }

    private void randomEncounterSelector()
    {
        if (Encounter.ToString() == "SkeletonInForest")
            SkeletonInForest();
    }

    IEnumerator TypeWriterEffect()
    {
        for (int i = 0; i < eventText.Length; i++)
        {
            dialogBoxText.text = eventText.Substring(0, i + 1);
            yield return new WaitForSeconds(delayTextTime);
        }

        for (int i = 0; i < dialogOptions[0].Length; i++)
        {
            dialogOptionsText[0].text = dialogOptions[0].Substring(0, i + 1);
            yield return new WaitForSeconds(delayTextTime);
        }
        for (int i = 0; i < dialogOptions[1].Length; i++)
        {
            dialogOptionsText[1].text = dialogOptions[1].Substring(0, i + 1);
            yield return new WaitForSeconds(delayTextTime);
        }

        StartCoroutine("DialogSelection");
    }

    private void SkeletonInForest()
    {
        if(!startSomething)
        {
            moveTo = new Vector2(0.5f, -4f);
            movePlayer = true;
        }
        else
        {
            dialogBox.SetActive(true);
            eventText = "you founded a fucking skeleted";
            dialogOptions[0] = "> Face him! (lose 10 HP)";
            dialogOptions[1] = "> well hello baby (???)";
            StartCoroutine("TypeWriterEffect");
        }
    }
    IEnumerator DialogSelection()
    {
        while(dialogOption == 0)
        {
            yield return null;
        }
        if (dialogOption == 2)
        {
            EraseDialogBox();

            eventText = "the skeleted said he wants you ded";
            dialogOptions[0] = "> oh no!";
            dialogOptions[1] = "> Oh yeah!";
            StartCoroutine("TypeWriterEffect");
        }
        if (dialogOption == 1)
        {
            EraseDialogBox();

            eventText = "You win, but the skeleted stabbed your head";
            dialogOptions[0] = "> oh okay";
            dialogOptions[1] = "";
            StartCoroutine("TypeWriterEffect");
        }

        dialogOption = 0;
        yield return null;
    }

    private void EraseDialogBox()
    {
        dialogBoxText.text = "";
        dialogOptionsText[0].text = "";
        dialogOptionsText[1].text = "";
    }
}
