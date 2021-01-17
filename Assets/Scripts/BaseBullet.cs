using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [HideInInspector]public int Damage;

    private AutoPush push;
    private void Start()
    {
        push = GetComponent<AutoPush>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("EnemyBullet") && other.CompareTag("Block"))
        {
            other.GetComponent<ITakeDamage>().TakeDamage(Damage);
            push.PushRightNow();
        }
        
    }
    
    public void Explode()
    {
        
    }
}
