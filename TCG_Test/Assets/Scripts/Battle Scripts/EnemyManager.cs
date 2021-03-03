using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public string enemyName;
    public GameObject healthBar;
    private Slider healthSlider;
    public EnemyAttackStats move1;
    public EnemyAttackStats move2;
    public EnemyAttackStats move3;
    private Animator enemyAnimator;
    private Text HealthText;
    public float MaxHealth;
    public float CurrentHealth;
    public EnemyIntentionsManager enemyintentionmanagerScript;

    private PlayerManager playermanagerscript;

    public Image pointer1;

    private BattleManager battleManagerScript;

    private int attackCooldown = 0;
    private float enemyDamage;
    private string enemyAttackEffect;

    List<EnemyAttackStats> movelist = new List<EnemyAttackStats>();

    void Start()
    {
        healthSlider = healthBar.GetComponent<Slider>();
        HealthText = healthBar.GetComponentInChildren<Text>();
        enemyAnimator = GetComponent<Animator>();
        battleManagerScript = GameObject.FindGameObjectWithTag("Battle Manager Tag").GetComponent<BattleManager>();
        playermanagerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();


        CurrentHealth = MaxHealth;
        HealthText.text = CurrentHealth + "/" + MaxHealth.ToString();
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = CurrentHealth;


        movelist.Add(move1);
        movelist.Add(move2);
        movelist.Add(move3);

        
    }

    public void OnTakeDamage(float damageTaken)
    {
        float initialHealth = CurrentHealth;

        CurrentHealth -= damageTaken;
        if (CurrentHealth <= 0) // isso serve pra vida não ficar negativa
            CurrentHealth = 0;
        if (CurrentHealth > MaxHealth) // isso serve pra ele não se curar mais do que a vida máxima
            CurrentHealth = MaxHealth;

        HealthText.text = CurrentHealth.ToString() + "/" + MaxHealth.ToString();
        healthSlider.value = CurrentHealth;

        if (CurrentHealth < initialHealth && CurrentHealth > 0)
            enemyAnimator.SetTrigger("Damage");
        if (CurrentHealth <= 0)
        {
            enemyAnimator.SetBool("IsDead", true);
            healthBar.SetActive(false);
        }

    }

    public EnemyAttackStats EnemySkillDecider()
    {
        EnemyAttackStats attackToReturn;

        switch (enemyName)
        {
            case "Goblin":
                attackToReturn = GoblinBehavior();
                return attackToReturn;
            case "FlyingEye":
                attackToReturn = FlyingEyeBehavior();
                return attackToReturn;
            default:
                Debug.Log("Error Enemy not Found");
                return null;

        }
    }

    public void attackThePlayer()
    {
        playermanagerscript.HealthManager(-enemyDamage);
        if (enemyAttackEffect == "Confusion")
        {
            battleManagerScript.DebuffActivator("Confusion");
        }
    }
    public void whenClicked()
    {
        if (CurrentHealth > 0)
        {
            battleManagerScript.enemySelected = gameObject;
            pointer1.enabled = false;
        }

    }
    public void OnHover()
    {
        if(battleManagerScript.selectingEnemy && CurrentHealth > 0)
        {
            pointer1.enabled = true;
        }
    }
    public void OnHoverExit()
    {
        pointer1.enabled = false;
    }

    private EnemyAttackStats GoblinBehavior()
    {
        if (CurrentHealth <= (MaxHealth * 0.5) && attackCooldown <= 0)
        {
            attackCooldown = 3;

            enemyDamage = move1.Damage;

            enemyintentionmanagerScript.IntentionManager("Damage", enemyDamage); // isso serve pra mostrar qual atk o inimigo intende usar

            return move1;
        }
        else
        {
            attackCooldown--;

            int attackToReturn = Random.Range(1, movelist.Count);


            if (attackToReturn == 1)
            {
                enemyDamage = move2.Damage;
            }
            else
            {
                enemyDamage = move3.Damage;
            }

            enemyintentionmanagerScript.IntentionManager("Damage", enemyDamage);

            return movelist[attackToReturn];
        }
    }

    private EnemyAttackStats FlyingEyeBehavior()
    {
        if (attackCooldown <= 0)
        {
            attackCooldown = 2;

            enemyDamage = move1.Damage;
            enemyAttackEffect = move1.cardEffects.ToString();

            enemyintentionmanagerScript.IntentionManager("Debuff", enemyDamage);


            return move1;
        }
        else
        {
            attackCooldown--;

            int attackToReturn = Random.Range(1, movelist.Count);


            if (attackToReturn == 1)
            {
                enemyDamage = move2.Damage;
                enemyAttackEffect = move2.cardEffects.ToString();
            }
            else
            {
                enemyDamage = move3.Damage;
                enemyAttackEffect = move3.cardEffects.ToString();
            }

            enemyintentionmanagerScript.IntentionManager("Damage", enemyDamage);


            return movelist[attackToReturn];
        }
    }
}
