using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using tower;
using UnityEngine;

public class SoldierTower : BaseTower
{
    public GameObject soldier;
    
    [HideInInspector]
    public float soliderBornCD;
    [HideInInspector]
    public int soliderAtk;
    
    private int soliderCount;
    private Queue<GameObject> _soliderQueue;
    void Start()
    {
        base.Start();
        Init();
//        EventCenter.GetInstance().EventTrigger($"{gameObject.name}Destory");
    }
    
    public override void Init()
    {
        base.Init();
        _soliderQueue = new Queue<GameObject>();
        Debug.Log(index);
        soliderCount = CurTower.nums[0];
        soliderAtk = CurTower.nums[1];
        soliderBornCD = CurTower.nums[2];
        Debug.Log($"build {soliderCount}  {soliderAtk}");
        StartCoroutine(CheckChild());
    }

    private void BornSoldier()
    {
        GameObject obj = Instantiate(soldier);
        _soliderQueue.Enqueue(obj);
        obj.transform.position = transform.position;
        obj.GetComponent<FollowSoldier>().SetHouse(this);
    }

    public override void OnBuildOver()
    {
        base.OnBuildOver();
        
        state = TowerState.Idle;
        soliderCount = CurTower.nums[0];
        soliderAtk = CurTower.nums[1];
        soliderBornCD = CurTower.nums[2];
        EventCenter.GetInstance().EventTrigger($"{gameObject.name}UpGrade");
        Debug.Log($"upgrade {soliderCount}  {soliderAtk}");
    }
    
    private IEnumerator CheckChild()
    {
        if (_soliderQueue.Count < soliderCount && state == TowerState.Idle)
        {
            anim.SetBool("isBorn",true);
        }
        yield return new WaitForSeconds(soliderBornCD);
        StartCoroutine(CheckChild());
    }

    private void Born()
    {
        BornSoldier();
        anim.SetBool("isBorn",false);
    } 
}
