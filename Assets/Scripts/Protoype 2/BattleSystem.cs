using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

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

    public BattleState state;

    public Text dialogueText;

    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    public Transform enemySpawn;
    public Transform playerSpawn;

    UnitScript playerUnit;
    UnitScript enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public bool actionChosen = false;

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
        actionChosen = false;
        dialogueText.text = "Choose an action";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionChosen = true;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionChosen = true;
        StartCoroutine(PlayerHeal());
    }

    private IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHP);

        dialogueText.text = "You heal!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerAttack()
    {
        // do attack anim
        playerUnit.anim.SetBool("isAttacking", true);
        enemyUnit.anim.SetBool("isHit", true);
        yield return new WaitForSeconds(0.1f);
        playerUnit.anim.SetBool("isAttacking", false);
        enemyUnit.anim.SetBool("isHit", false);

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack hits!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    private IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        enemyUnit.anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.2f);
        enemyUnit.anim.SetBool("isAttacking", false);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);

        if(isDead)
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

    private void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You Win!";
        }
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "You Lost!";
        }
    }
}
