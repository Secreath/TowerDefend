using System.Collections;
using System.Collections.Generic;
using tower;
using UnityEngine;

public class Castle : BaseProperty,ITakeDamage
{
    public int Hp;
   
    protected override void Start()
    {
        base.Start();
         maxHp = Hp;
         curHp = Hp;
         transform.position = VTool.ToTilePos(transform.position);
         EventCenter.GetInstance().AddEventListener("GameManagerInit",InitCastle);
    }
    
    public virtual void TakeDamage(int damage)
    {
        curHp -= damage;
        HpBarUpDate();
        if(curHp <=0)
            Dead();    
    }
    
    void InitCastle()
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
