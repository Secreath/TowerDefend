using System;
using System.Collections;
using System.Collections.Generic;
using GameUi;
using UnityEngine;

public class BaseProperty : MonoBehaviour
{
    private CharacterUiMgr uimgr;
    
    [SerializeField]protected int atk;
    [SerializeField]protected int maxHp;
    [SerializeField]protected float moveSpeed;
    
    
    protected int _curHp;
    protected float _curMoveSpeed;
    
    protected virtual void Start()
    {
        uimgr = transform.Find("Canvas").GetComponent<CharacterUiMgr>();
        _curHp = maxHp;
        _curMoveSpeed = moveSpeed;
    }
    

    public virtual void Dead()
    {
        Destroy(gameObject);
    }

    protected void HpBarUpDate()
    {
        uimgr.HpBarUpdate(_curHp,maxHp);
    }
}
