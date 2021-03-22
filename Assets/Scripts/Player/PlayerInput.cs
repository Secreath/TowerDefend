using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool pressJ;

    void Start()
    {
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyPress",CheckKeyPress);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown",CheckKeyDown);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyUp",CheckKeyUp);
    }
 
    private void CheckKeyPress(KeyCode key)
    {
            
    }
        
    private void CheckKeyDown(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.J:
                Debug.Log("0");
                pressJ = true;
                break;
        }
    }
        
    private void CheckKeyUp(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.J:
                pressJ = false;
                break;
        }   
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Construction"))
        {
            if (pressJ)
            {
                UiManager.Instance.MoveTowerUi(other.transform.position);
                pressJ = false;

            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Construction"))
        {
            pressJ = false;
        }
    }

    void Update()
    {
        
    }
}
