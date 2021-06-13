using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tower
{
    public class TowerRange : MonoBehaviour
    {
        private ShotTower tower;

        private SpriteRenderer sprite;


        private GameObject target;

        private Vector2 checkPos;
        private bool onAttack;

        private float attackTime;
        private float range;
        
        private void Start()
        {
            checkPos = transform.parent.position + transform.position;
            sprite = transform.GetComponent<SpriteRenderer>();
            tower = transform.parent.GetComponent<ShotTower>();
            Debug.Log(checkPos);
        }

        private void Update()
        {
            CheckRange();
        }

        public void SetRange(float range,float atttckTime)
        {
            transform.localScale = Vector3.one * range;
            this.range = range;
            this.attackTime = atttckTime;
            
        }
        
        private void CheckRange()
        {
            Collider2D other = Physics2D.OverlapCircle(checkPos, range ,LayerMask.GetMask("Enemy"));
            
            if (other != null)
            {
                if (!onAttack)
                {
                    target = other.gameObject;
                    StartCoroutine(AttackEnemy());
                }
                onAttack = true;

            }
            else
            {
                target = null;
                onAttack = false;
            }
            
        }

       

        private IEnumerator AttackEnemy()
        {
            while (target != null)
            {
                Debug.Log("ATTACK");
                tower.Attack(target.transform.position);
                yield return new WaitForSeconds(attackTime);
            }
        }
    }

}
