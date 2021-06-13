using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConstruct : MonoBehaviour
{
    public int Level_2_Price = 100;
    public int Level_3_Price = 300;
    protected int level;
    protected BoxCollider2D box;
    protected Animator anim;
    protected virtual void Start()
    {
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void LevelUp()
    {
        level++;
    }
 
}
