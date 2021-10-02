using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string unitName;

    public int damage;
    public int maxHP;
    public int currentHP;

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
        currentHP -= dmg;

        if(currentHP <= 0)
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
        currentHP += amount;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    IEnumerator Pause()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);

        anim.SetBool("isHit", false);
    }
}


