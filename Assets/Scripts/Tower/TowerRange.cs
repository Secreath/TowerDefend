using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    private Tower1 tower;

    private SpriteRenderer sprite;
    private bool onAttack;
    private GameObject attackTarget;
    private List<GameObject> enemyList;

    private float attackTime;
    public void Start()
    {
        enemyList = new List<GameObject>();
        sprite = transform.GetComponent<SpriteRenderer>();
        tower = transform.parent.GetComponent<Tower1>();
    }

    public void SetRange(float range,float atttckTime)
    {
        transform.localScale = Vector3.one * range;
        this.attackTime = atttckTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sprite.enabled = true;
        }
        
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("inrange");
            if(!enemyList.Contains(other.gameObject))
                enemyList.Add(other.gameObject);
            if (attackTarget == null)
            {
                attackTarget = other.gameObject;
                StartCoroutine(CheckEnemy(attackTarget));
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sprite.enabled = true;
        }
        
        if (other.CompareTag("Enemy"))
        {
            if (other)
            {
                
            }
            enemyList.Remove(other.gameObject);
            attackTarget = null;
        }
    }

    private IEnumerator CheckEnemy(GameObject target)
    {
        while (target != null)
        {
            tower.Attack(attackTarget.transform.position);
            yield return new WaitForSeconds(attackTime);
        }
    }
}
