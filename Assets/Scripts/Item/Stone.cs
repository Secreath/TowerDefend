using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : BaseProperty,ITakeDamage
{
    public virtual void TakeDamage(int damage)
    {
        _curHp -= damage;
        HpBarUpDate();
        if(_curHp <=0)
            Dead();    
    }
    
}
