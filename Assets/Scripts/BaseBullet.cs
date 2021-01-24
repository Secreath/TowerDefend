using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float speed;
    [HideInInspector]public int Damage;
    private Rigidbody2D rb;
    private AutoPush push;
    private void Start()
    {
        push = GetComponent<AutoPush>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("EnemyBullet") && other.CompareTag("Block"))
        {
            other.GetComponent<ITakeDamage>().TakeDamage(Damage);
            push.PushRightNow();
        }else if (gameObject.CompareTag("PlayerBullet") && other.CompareTag("Enemy"))
        {
            Debug.Log("Take");
            other.GetComponent<ITakeDamage>().TakeDamage(Damage);
            push.PushRightNow();
        }
        
    }

    public void SetShootDir(int damage,Vector2 target,Vector2 bullet)
    {
        Damage = damage;
        transform.position = bullet;
        GetAngle.RotaWithZ(target,bullet,gameObject);
        Vector2 dir = (target - bullet).normalized;
        GetComponent<Rigidbody2D>().velocity = dir * speed;
    }
    public void Explode()
    {
        
    }
}
