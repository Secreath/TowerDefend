using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower1 : MonoBehaviour
{
    public float range;
    public float time;
    
    private TowerRange towerRange;
    private Animator anim;
    private GameObject attackTarget;


    private bool attacked;

    private Vector3 targetPos;
    void Start()
    {

        anim = GetComponent<Animator>();
        towerRange = transform.Find("range").GetComponent<TowerRange>();
        
        towerRange.SetRange(range);
    }

    
    public void Attack(Vector3 targetPos)
    {
        if(attacked)
            return;
        attacked = true;
        Invoke(nameof(Attacked),time);
        Debug.Log("Att");
        this.targetPos = targetPos;
        anim.SetBool("isAttack",true);       
        
        
    }

    public void Shoot()
    {
//        GameObject bullet = PoolMgr.GetInstance().PopObj("PeaBall");
//        bullet.tag = "PlayerBullet";
//        bullet.GetComponent<BaseBullet>().SetShootDir(10, targetPos, transform.position);
    }

    public void ResetAttack()
    {
        anim.SetBool("isAttack",false);
    }


    private void Attacked()
    {
        attacked = false;
    }
    
    
    
}
