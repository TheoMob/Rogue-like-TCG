using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Slider healthSlider;
    public Text playerHealthText;
    public Text playerManaText;
    public Text playerShieldText;
    public Text deckCardsCountText;
    public float playerMaxHealth;
    public float playerMaxMana;
    public float playerShield;
    private Animator playerAnimator;
    private GameObject deck;

    public float currentHealth;
    public float currentMana;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerAnimator = GetComponent<Animator>();
        deck = GameObject.FindGameObjectWithTag("Deck");

        healthSlider.maxValue = playerMaxHealth;
        healthSlider.value = playerMaxHealth;
        currentHealth = playerMaxHealth;
        currentMana = playerMaxMana;
        playerHealthText.text = currentHealth.ToString() + "/" + playerMaxHealth.ToString();
        playerManaText.text = currentMana.ToString() + "/" + playerMaxMana.ToString();
        playerShieldText.text = playerShield.ToString();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HealthManager(-5f);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShieldManager(5);
        }
    }

    public void HealthManager(float healthModifier)
    {
        float initialHealth = currentHealth;

        if (healthModifier < 0) // esse if serve pra, caso o healthmodificer seja um dano, ele passe primeiro no shield
        {
            if (playerShield + healthModifier >= 0) // se o shield menos o shieldModificer for maior ou igual a zero, é pq o shield é capaz de tankar todo o dano
            {
                playerShield += healthModifier;
                healthModifier = 0;
                playerAnimator.SetTrigger("Block");
            }
            else
            {
                healthModifier += playerShield; // senão subtraia o valor do shield do dano a ser recebido
                playerShield = 0;
            }
            ShieldManager(0);
        }


        currentHealth += healthModifier;
      
        if (currentHealth > playerMaxHealth) // isso serve pra vida do personagem não ser maior que o maximo
            currentHealth = playerMaxHealth;
        if (currentHealth < 0) // isso serve pra vida não ficar negativa caso um atk seja maior que a vida atual
            currentHealth = 0;

        playerHealthText.text = currentHealth.ToString() + "/" + playerMaxHealth.ToString();
        healthSlider.value = currentHealth;

        if (currentHealth < initialHealth && currentHealth > 0)
            playerAnimator.SetTrigger("Damage");
        if (currentHealth <= 0)
            playerAnimator.SetBool("IsDead", true);
    }

    public void ShieldManager ( float shieldModifier)
    {
        playerShield += shieldModifier;
        if (playerShield < 0)
            playerShield = 0;

        playerShieldText.text = playerShield.ToString();
    }

    public void ManaManager(float manaModificer)
    {
        currentMana -= manaModificer;
        if (currentMana > playerMaxMana) // isso serve pra mana do personagem não ser maior que o máximo
            currentMana = playerMaxMana;
        playerManaText.text = currentMana.ToString() + "/" + playerMaxMana.ToString();

    }

    public void DeckCounter() // isso deveria estar no script do deck e não do player
    {
        if (!deckCardsCountText.gameObject.activeSelf) // isso serve pra ligar o texto do deckCount caso ele esteja desligado
            deckCardsCountText.gameObject.SetActive(true);

        int deckCardsCount = deck.transform.childCount;

        deckCardsCountText.text = deckCardsCount.ToString();
    }
}
