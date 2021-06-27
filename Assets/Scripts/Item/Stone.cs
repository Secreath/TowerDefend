using System;
using System.Collections;
using System.Collections.Generic;
using ui;
using UnityEngine;

public class Stone : BaseProperty,ITakeDamage
{
    public int tauntRange;
    public virtual void TakeDamage(int damage)
    {
        _curHp -= damage;
        HpBarUpDate();
        if(_curHp <=0)
            Dead();    
    }

    void Start()
    {
        transform.position = VTool.ToTilePos(transform.position);
        EventCenter.GetInstance().AddEventListener("GameManagerInit",InitStone);
    }

    void InitStone()
    {
        base.Start();
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
