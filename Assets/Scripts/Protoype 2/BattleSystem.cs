using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    #region Singleton
    public static BattleSystem instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleSystem found");
            return;
        }
        instance = this;
    }
    #endregion

    #region Variables
    public BattleState state;

    public Text dialogueText;

    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    public Transform enemySpawn;
    public Transform playerSpawn;

    public UnitScript playerUnit;
    public UnitScript enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject playerAttacksPanel;
    public GameObject openAttacksButton;

    public int healthPerTurn;
    public int stamPerTurn;
    public int manaPerTurn;

    [HideInInspector]
    public bool actionChosen = false;
    #endregion

    void Start()
    {
        actionChosen = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<UnitScript>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGO.GetComponent<UnitScript>();

        dialogueText.text = enemyUnit.unitName + " approaches!";

        enemyHUD.SetHUD(enemyUnit);
        playerHUD.SetHUD(playerUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        playerUnit.Heal(healthPerTurn);
        playerHUD.SetHP(playerUnit.stats.currHP);

        playerUnit.RestoreStam(stamPerTurn);
        playerHUD.SetStamina(playerUnit.stats.currStamina);

        playerUnit.RestoreMana(manaPerTurn);
        playerHUD.SetMana(playerUnit.stats.currMana);

        actionChosen = false;
        dialogueText.text = "Choose an action";
    }

    public void OpenAttacksButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        playerAttacksPanel.transform.GetChild(0).GetComponent<Image>().sprite = playerUnit.actions[0].icon;
        playerAttacksPanel.transform.GetChild(1).GetComponent<Image>().sprite = playerUnit.actions[1].icon;
        playerAttacksPanel.transform.GetChild(2).GetComponent<Image>().sprite = playerUnit.actions[2].icon;
        playerAttacksPanel.transform.GetChild(3).GetComponent<Image>().sprite = playerUnit.actions[3].icon;
        playerAttacksPanel.SetActive(true);
        openAttacksButton.SetActive(false);
    }

    public void CloseAttacksButton()
    {
        playerAttacksPanel.SetActive(false);
        openAttacksButton.SetActive(true);
    }

    public void OnAttackButton(int button)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionChosen = true;
        //if action uses stamina
        if (playerUnit.actions[button].statCost == Action.StatCost.STAMINA)
        {
            StaminaAction(button);
        }
        //if action uses mana
        else if (playerUnit.actions[button].statCost == Action.StatCost.MANA)
        {
            ManaAction(button);
        }
        //if action uses health
        else if (playerUnit.actions[button].statCost == Action.StatCost.HEALTH)
        {
            HealthAction(button);
        }
        else
        {
            CloseAttacksButton();
            StartCoroutine(PlayerAttack(playerUnit.actions[button]));
        }
    }

    public void StaminaAction(int button)
    {
        //if i have enough stamina
        if (playerUnit.stats.currStamina >= playerUnit.actions[button].cost)
        {
            //do it
            playerUnit.stats.currStamina -= playerUnit.actions[button].cost;
            playerHUD.SetStamina(playerUnit.stats.currStamina);
            CloseAttacksButton();

            StartCoroutine(PlayerAttack(playerUnit.actions[button]));
        }
        else
        {
            StartCoroutine(playerHUD.FlashText(playerHUD.stamTracker, playerHUD.stamTracker.color, Color.red, 3));
        }
    }

    public void ManaAction(int button)
    {
        //if i have enough mana
        if (playerUnit.stats.currMana >= playerUnit.actions[button].cost)
        {
            //do it
            playerUnit.stats.currMana -= playerUnit.actions[button].cost;
            playerHUD.SetMana(playerUnit.stats.currMana);
            CloseAttacksButton();

            StartCoroutine(PlayerAttack(playerUnit.actions[button]));
        }
        else
        {
            StartCoroutine(playerHUD.FlashText(playerHUD.manaTracker, playerHUD.manaTracker.color, Color.red, 3));
        }
    }

    public void HealthAction(int button)
    {
        //if i have enough health
        if (playerUnit.stats.currHP >= playerUnit.actions[button].cost)
        {
            //do it
            playerUnit.stats.currHP -= playerUnit.actions[button].cost;
            playerHUD.SetHP(playerUnit.stats.currHP);
            CloseAttacksButton();

            StartCoroutine(PlayerAttack(playerUnit.actions[button]));
        }
        else
        {
            StartCoroutine(playerHUD.FlashText(playerHUD.healthTracker, playerHUD.healthTracker.color, Color.red, 3));
        }
    }

    IEnumerator EnemyHealthAction(Action action)
    {
        //do it
        enemyUnit.stats.currHP -= action.cost;
        enemyHUD.SetHP(enemyUnit.stats.currHP);

        bool isDead = playerUnit.TakeDamage(action.damage);

        StartCoroutine(EnemyAttackAnim(action));

        playerHUD.SetHP(playerUnit.stats.currHP);
        yield return new WaitForSeconds(1f);

        CheckForEndOfGame(isDead);
    }

    IEnumerator EnemyStaminaAction(Action action)
    {
        //do it

        dialogueText.text = enemyUnit.unitName + " uses " + action.actionName;
        
        enemyUnit.stats.currStamina -= action.cost;
        enemyHUD.SetStamina(enemyUnit.stats.currStamina);

        bool isDead = playerUnit.TakeDamage(action.damage);

        StartCoroutine(EnemyAttackAnim(action));

        playerHUD.SetHP(playerUnit.stats.currHP);
        yield return new WaitForSeconds(1f);

        CheckForEndOfGame(isDead);
    }

    IEnumerator EnemyManaAction(Action action)
    {
        //do it
        enemyUnit.stats.currMana -= action.cost;
        enemyHUD.SetMana(enemyUnit.stats.currMana);

        bool isDead = playerUnit.TakeDamage(action.damage);

        StartCoroutine(EnemyAttackAnim(action));

        playerHUD.SetHP(playerUnit.stats.currHP);
        yield return new WaitForSeconds(1f);

        CheckForEndOfGame(isDead);
    }

    private void PlayerHeal(Action action)
    {
        playerUnit.Heal(action.heal);
        playerHUD.SetHP(playerUnit.stats.currHP);
        dialogueText.text = playerUnit.unitName + " heals " + action.heal + " health";
    }

    private void PlayerAddStamina(Action action)
    {
        playerUnit.RestoreStam(action.healStam);
        playerHUD.SetStamina(playerUnit.stats.currStamina);
        dialogueText.text = playerUnit.unitName + " restores " + action.healStam + " stamina";
    }

    private void PlayerAddMana(Action action)
    {
        playerUnit.RestoreMana(action.healMana);
        playerHUD.SetMana(playerUnit.stats.currMana);
        dialogueText.text = playerUnit.unitName + " restores " + action.healMana + " mana";
    }

    IEnumerator PlayerAttack(Action action)
    {
        // do attack anim
        bool isDead = false;
        playerUnit.anim.SetBool("isAttacking", true);
        enemyUnit.anim.SetBool("isHit", true);
        yield return new WaitForSeconds(0.1f);
        playerUnit.anim.SetBool("isAttacking", false);
        enemyUnit.anim.SetBool("isHit", false);
       
        if (action.heal > 0)
        {
            PlayerHeal(action);
            yield return new WaitForSeconds(2f);
        }

        if (action.healStam > 0)
        {
            PlayerAddStamina(action);
            yield return new WaitForSeconds(2f);
        }

        if (action.healMana > 0)
        {
            PlayerAddMana(action);
            yield return new WaitForSeconds(2f);
        }

        if (action.damage > 0)
        {
            isDead = enemyUnit.TakeDamage(action.damage);
            enemyHUD.SetHP(enemyUnit.stats.currHP);
            dialogueText.text = "The attack hits!";

            yield return new WaitForSeconds(2f);
        }
        CheckForEndOfGame(isDead);
    }

    private void EnemyTurn()
    {
        enemyUnit.Heal(healthPerTurn);
        enemyHUD.SetHP(enemyUnit.stats.currHP);

        enemyUnit.RestoreStam(stamPerTurn);
        enemyHUD.SetStamina(enemyUnit.stats.currStamina);

        enemyUnit.RestoreMana(manaPerTurn);
        enemyHUD.SetMana(enemyUnit.stats.currMana);


        ChooseEnemyAttack();
    }

    private void ChooseEnemyAttack()
    {
        int rand = UnityEngine.Random.Range(0, 4);
        //check if the enemy can use that skill
        if (enemyUnit.actions[rand].statCost == Action.StatCost.HEALTH)
        {
            if (enemyUnit.stats.currHP >= enemyUnit.actions[rand].cost)
            {
                StartCoroutine(EnemyHealthAction(enemyUnit.actions[rand]));
            }
            else
            {
                ChooseEnemyAttack();
            }
        }
        else if (enemyUnit.actions[rand].statCost == Action.StatCost.STAMINA)
        {
            if (enemyUnit.stats.currStamina >= enemyUnit.actions[rand].cost)
            {
                StartCoroutine(EnemyStaminaAction(enemyUnit.actions[rand]));
            }
            else
            {
                ChooseEnemyAttack();
            }
        }
        else if (enemyUnit.actions[rand].statCost == Action.StatCost.MANA)
        {
            if (enemyUnit.stats.currMana >= enemyUnit.actions[rand].cost)
            {
                StartCoroutine(EnemyManaAction(enemyUnit.actions[rand]));
            }
            else
            {
                ChooseEnemyAttack();
            }
        }
    }

    IEnumerator EnemyAttackAnim(Action action)
    {
        enemyUnit.anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.2f);
        enemyUnit.anim.SetBool("isAttacking", false);

        yield return new WaitForSeconds(1f);
    }

    public void CheckForEndOfGame(bool isDead)
    {
        if (state == BattleState.ENEMYTURN)
        {
            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else if (state == BattleState.PLAYERTURN)
        {
            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
            }
        }
    }

    private void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You Win!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You Lost!";
        }
    }
}