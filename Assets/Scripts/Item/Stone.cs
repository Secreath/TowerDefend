using System;
using System.Collections;
using System.Collections.Generic;
using tower;
using ui;
using UnityEngine;

public class Stone : BaseProperty,ITakeDamage
{
    public int Hp;
    public int tauntRange;
    public virtual void TakeDamage(int damage)
    {
        curHp -= damage;
        HpBarUpDate();
        if(curHp <=0)
            Dead();    
    }

    protected override void Start()
    {
        base.Start();
        canMove = false;

        maxHp = Hp;
        curHp = Hp;
        
        transform.position = VTool.ToTilePos(transform.position);
        EventCenter.GetInstance().AddEventListener("GameManagerInit",InitStone);
    }

    void InitStone()
    {
        SetUiPos(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().TauntEnemy(transform,gameObject.GetHashCode());
        }
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().EventTrigger($"{gameObject.GetHashCode()}Destory");
    }
}
