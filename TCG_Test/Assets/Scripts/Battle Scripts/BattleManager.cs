using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public int enemiesAmount;
    public GameObject dropZone;
    public GameObject failureScene;
    public GameObject sucessScene;
    public GameObject yourTurnText;
    public GameObject enemyTurnText;
    public GameObject enemyIntentionsUI1;
    public GameObject enemyIntentionsUI2;

    private EnemyAttackStats enemyAttack1;
    private EnemyAttackStats enemyAttack2;

    public EnemyManager enemyManager1;
    public EnemyManager enemyManager2;
    public PlayerManager playerManagerScript;
    public PlayerDeck playerDeckScript;
    public MoveCardToHand moveCardToHandScript;
    private DebuffBarManager debuffBarManagerscript;

    [HideInInspector]public GameObject enemySelected;


    [HideInInspector]public bool playerTurn = true;
    [HideInInspector]public bool selectingEnemy = false;

    private float cardDamage;
    private float cardHeal;
    private int cardDraw;
    private float cardShield;

    private bool enemy1Alive = true;
    private bool enemy2Alive = false;
    private int enemiesAlive;

    //essa parte é para debuffs//
    private int confusionDebuffTime = 0;


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerManagerScript = player.GetComponent<PlayerManager>();
        playerDeckScript = player.GetComponent<PlayerDeck>();
        moveCardToHandScript = player.GetComponent<MoveCardToHand>();
        debuffBarManagerscript = gameObject.GetComponent<DebuffBarManager>();

        enemiesAlive = enemiesAmount;
        
        if (enemiesAmount >= 2)
        {
            enemy2Alive = true;
        }

    }

    private void Update()
    {
        if (selectingEnemy)
        {
            StartCoroutine("PlayerSelectingEnemy");
        }
        else
        {
            if (enemySelected != null)
                enemySelected = null;
        }

        if (playerManagerScript.currentHealth <= 0)
        {
            PlayerDeath();
            return; // esse return é pra não voltar pro turno do player caso ele morra
        }
    }
    public void CombatStart()
    {
        playerDeckScript.InstantiateDeck(); // instancia todas as cartas para a posição do deck
        moveCardToHandScript.shuffleDeck();
        moveCardToHandScript.ReadDeck(); // le o deck para saber quais cartas estão la dentro ainda
        PlayerStartTurn(); // chama o turno do player
    }

    private void PlayerStartTurn()
    {
        if(enemy1Alive)
        {
            enemyIntentionsUI1.SetActive(true); // isso serve pro player conseguir ver o que o inimigo pretende fazer
            enemyAttack1 = enemyManager1.EnemySkillDecider();
        }

        if (enemiesAmount >= 2 && enemy2Alive)
        {
            enemyIntentionsUI2.SetActive(true);
            enemyAttack2 = enemyManager2.EnemySkillDecider();
        }


        yourTurnText.SetActive(false);
        playerManagerScript.ShieldManager(-999); // isso é pra resetar o shield do player todo fim de turno
        playerTurn = true;
        moveCardToHandScript.move = true; // isso é pra dar draw


    }
    public void OnCardPlayed(string cardTag, float cardDamage1, float cardManaCost1, float cardHeal1, int cardDraw1, float cardShield1)
    {
        // essa função é apenas para as cartas jogadas pelo player

        playerManagerScript.ManaManager(cardManaCost1);

        if (enemiesAmount > 1 &&  enemiesAlive > 1)
        {
            if (cardDamage1 > 0)
            {
                selectingEnemy = true;
                cardDamage = cardDamage1;
                cardHeal = cardHeal1;
                cardDraw = cardDraw1;
            }
            else // o player não precisa selecionar o inimigo para cartas que não dão dano
            {
                playerManagerScript.HealthManager(cardHeal1);
                playerManagerScript.ShieldManager(cardShield1);
                moveCardToHandScript.draw = cardDraw1; // isso faz o script MoveCardToHand comprar X cartas (X = cardDraw)
            }
        }
        else
        {
            if (cardDamage1 > 0)
            {
                if (enemy1Alive)
                {
                    Animator playerAnimator = playerManagerScript.gameObject.GetComponent<Animator>(); // pega o Animator que está no player
                    playerAnimator.SetTrigger("Attack");

                    enemyManager1.OnTakeDamage(cardDamage1);

                    if (enemyManager1.CurrentHealth <= 0)
                    {
                        enemyIntentionsUI1.SetActive(false);
                        enemy1Alive = false;
                        enemiesAlive--;
                    }
                }
                if (enemy2Alive)
                {
                    Animator playerAnimator = playerManagerScript.gameObject.GetComponent<Animator>(); // pega o Animator que está no player
                    playerAnimator.SetTrigger("Attack");

                    enemyManager2.OnTakeDamage(cardDamage1);

                    if (enemyManager2.CurrentHealth <= 0)
                    {
                        enemyIntentionsUI2.SetActive(false);
                        enemy2Alive = false;
                        enemiesAlive--;
                    } 
                }
                if (enemiesAlive == 0)
                    PlayerVictory();

            }
            playerManagerScript.HealthManager(cardHeal1);
            playerManagerScript.ShieldManager(cardShield1);
            moveCardToHandScript.draw = cardDraw1; // isso faz o script MoveCardToHand comprar X cartas (X = cardDraw)
        }

    }

    private IEnumerator PlayerSelectingEnemy()
    {
        if (confusionDebuffTime > 0)
        {
            confusionDebuff();
            selectingEnemy = false;
            enemySelected = null;
            yield return null;

        }

        if (enemySelected == null)
        {
            yield return null;
        }
        else
        {
            Animator playerAnimator = playerManagerScript.gameObject.GetComponent<Animator>(); // pega o Animator que está no player
            playerAnimator.SetTrigger("Attack");

            EnemyManager enemy = enemySelected.GetComponent<EnemyManager>();

            enemy.OnTakeDamage(cardDamage);
            playerManagerScript.HealthManager(cardHeal);
            playerManagerScript.ShieldManager(cardShield);
            moveCardToHandScript.draw = cardDraw; // isso faz o script MoveCardToHand comprar X cartas (X = cardDraw)


            if (enemy.CurrentHealth <= 0)
            {
                if (enemyManager1.CurrentHealth <= 0)
                {
                    enemyIntentionsUI1.SetActive(false);
                    enemy1Alive = false;
                }
                if (enemyManager2.CurrentHealth <= 0)
                {
                    enemyIntentionsUI2.SetActive(false);
                    enemy2Alive = false;
                }

                enemiesAlive--;

                if (enemiesAlive == 0)
                    PlayerVictory();
            }


            selectingEnemy = false;
            enemySelected = null;

            yield return null;
        }
    }
    public void PlayerEndTurn()
    {
        if (moveCardToHandScript.sendCardsBack || playerTurn == false || selectingEnemy) // se as cartas estiverem sendo colcoadas devolta no Deck, não deixe o player terminar o turno, ou caso n seja o turn do player
            return;

        playerManagerScript.ManaManager(-playerManagerScript.playerMaxMana); // reseta a mana do player pro maximo

        playerTurn = false; // isso é chamado no DragDrop, pra impedir que o player consiga arrastar as proprias cartas quando fora do seu turno

        StartCoroutine("PlayerEndTurns"); // isso serve pra troca de turnos não ser instantanea

    }
    IEnumerator PlayerEndTurns()
    {
        DebuffsBuffsTimer(); // isso serve pra quando o turno passar o debuff ou buff que ta no player contabilizar

        enemyIntentionsUI1.SetActive(false); // desativa a UI que mostra que ataque o inimigo pretende lançar
        if (enemiesAmount == 2)
            enemyIntentionsUI2.SetActive(false);
        enemyTurnText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("ChangeTurns2");
    }
    IEnumerator ChangeTurns2()
    {
        enemyTurnText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        EnemyStartTurn();
    }

    public void EnemyStartTurn()
    {
        if(enemy1Alive)
        {

            //playerManagerScript.HealthManager(-enemyAttack1.Damage); // da dano caso o ataque de dano
            //enemyManager1.OnTakeDamage(-enemyAttack1.Heal);

            //if (enemyAttack1.Damage > 0)
            //{
                Animator enemyAnimator = enemyManager1.gameObject.GetComponent<Animator>();
                enemyAnimator.SetTrigger(enemyAttack1.AttackAnimation.ToString());
            //}
        }


        if (enemiesAmount >= 2)
        {
            StartCoroutine("Enemy2StartTurn");
        }
        else
        { 
            Debug.Log("enemy end turn");
            StartCoroutine("EnemyEndTurn"); // isso serve pra troca de turnos não ser instantanea
        }


    }

    IEnumerator Enemy2StartTurn()
    {
        if(enemy1Alive)
            yield return new WaitForSeconds(1f);

        if(enemy2Alive)
        {
            //playerManagerScript.HealthManager(-enemyAttack2.Damage); // da dano caso o ataque de dano
            //enemyManager2.OnTakeDamage(-enemyAttack2.Heal);

                Animator enemyAnimator = enemyManager2.gameObject.GetComponent<Animator>();
                enemyAnimator.SetTrigger(enemyAttack2.AttackAnimation.ToString());
                // o dano dado ao inimigo é chamado diretamente pela animação, pois assim eu posso controlar o exato frame que o ataque deve ser desferido
                


            if (playerManagerScript.currentHealth <= 0)
            {
                PlayerDeath();
                yield return null; // esse return é pra não voltar pro turno do player caso ele morra
            }

        }


        Debug.Log("enemy end turn");
        StartCoroutine("EnemyEndTurn"); // isso serve pra troca de turnos não ser instantanea
    }
    IEnumerator EnemyEndTurn()
    {
        yield return new WaitForSeconds(0.5f);
        yourTurnText.SetActive(true);
        StartCoroutine("ChangeTurns1");
    }
    IEnumerator ChangeTurns1()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerStartTurn();
 
    }

    public void DebuffActivator(string debuffName)
    {
        if (debuffName == "Confusion")
        {
            confusionDebuffTime = 2;
            debuffBarManagerscript.debuffConfusion(true);
        }
    }
    private void DebuffsBuffsTimer()
    {
        if (confusionDebuffTime > 0)
        {
            confusionDebuffTime--;
            if (confusionDebuffTime <= 0)
                debuffBarManagerscript.debuffConfusion(false);
        }
    }
    private void confusionDebuff()
    {
        Animator playerAnimator = playerManagerScript.gameObject.GetComponent<Animator>(); // pega o Animator que está no player
        playerAnimator.SetTrigger("Attack");
        switch (enemiesAlive)
        {
            case 2:
                int rnd = Random.Range(0,2);
                Debug.Log(rnd);
                if(rnd == 1)
                {
                    enemyManager1.OnTakeDamage(cardDamage);


                    if (enemyManager1.CurrentHealth <= 0)
                    {
                        enemyIntentionsUI1.SetActive(false);
                        enemy1Alive = false;
                        enemiesAlive--;
                    }
                }
                else
                {
                    enemyManager2.OnTakeDamage(cardDamage);

                    if (enemyManager2.CurrentHealth <= 0)
                    {
                        enemyIntentionsUI2.SetActive(false);
                        enemy2Alive = false;
                        enemiesAlive--;
                    }
                }

                playerManagerScript.HealthManager(cardHeal);
                playerManagerScript.ShieldManager(cardShield);
                moveCardToHandScript.draw = cardDraw; // isso faz o script MoveCardToHand comprar X cartas (X = cardDraw)

                break;
    
        }
    } // faz o player não poder escolher o alvo do seu ataque

    private void PlayerDeath()
    {
        playerTurn = false;
        moveCardToHandScript.move = false;

        failureScene.SetActive(true);
    }

    private void PlayerVictory()
    {
        playerTurn = false;
        moveCardToHandScript.move = false;
        sucessScene.SetActive(true);

    }
}
