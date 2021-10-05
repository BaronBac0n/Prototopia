using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{

    public string unitName;

    [System.Serializable]
    public struct Stats
    {
        public int maxHP, currHP;
        public int maxStamina, currStamina;
        public int maxMana, currMana;
    }
    
    public Stats stats;
    public Action[] actions;

    public int damage;

    public Animator anim;

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public bool TakeDamage(int dmg) //returns true if the target dies from the damage
    {

        anim.SetBool("isHit", true);
        StartCoroutine(Pause());

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        stats.currHP -= dmg;

        if(stats.currHP <= 0)
        {
            anim.SetBool("isDead", true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int amount)
    {
        stats.currHP += amount;
        if(stats.currHP > stats.maxHP)
        {
            stats.currHP = stats.maxHP;
        }
    }

    public void RestoreStam(int amount)
    {
        stats.currStamina += amount;
        if(stats.currStamina > stats.maxStamina)
        {
            stats.currStamina = stats.maxStamina;
        }
    }

    public void RestoreMana(int amount)
    {
        stats.currMana += amount;
        if(stats.currMana > stats.maxMana)
        {
            stats.currMana = stats.maxMana;
        }
    }

    IEnumerator Pause()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);

        anim.SetBool("isHit", false);
    }
}


