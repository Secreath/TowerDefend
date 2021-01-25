using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class LittleWitchHouse : BaseCons
{
    public GameObject Child;
    public float BornCD;
    private int _childCount;

    private Queue<GameObject> childQueue;
    private Transform house;
    void Start()
    {
        base.Start();
        level = 1;
        _childCount = 3;
        childQueue = new Queue<GameObject>();
        house = transform.Find("House");
        StartCoroutine(CheckChild());
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        BornCD -= 5;
        _childCount += 2;
    }


    private IEnumerator CheckChild()
    {
        if (childQueue.Count < _childCount)
        {
            anim.SetBool("isPlay",true);
        }
        yield return new WaitForSeconds(BornCD);
    }

    private void PlayAnim()
    {
        if (childQueue.Count < _childCount)
        {
            GameObject child = Instantiate(Child);
            childQueue.Enqueue(child);
            child.transform.position = house.position;
            child.GetComponent<LittleWitch>().SetHouse(house);
            anim.SetBool("isPlay",false);
        }
    } 
}
