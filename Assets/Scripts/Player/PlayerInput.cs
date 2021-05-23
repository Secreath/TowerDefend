using System;
using System.Collections;
using System.Collections.Generic;
using tower;
using ui;
using UnityEngine;

namespace Player
{
    public class PlayerInput : Singleton<PlayerInput>
    {

        private BaseTower tower;
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
                    Point point = GameManager.GetPointByPos(transform.position);
                    Debug.Log(GameManager.GetJState(point));
                    if (GameManager.GetJState(point) == JState.ControllUi)
                        UiManager.Instance.JKeyPress(transform.position);
                    else if (GameManager.GetJState(point) == JState.PickUp)
                    {
                        Debug.Log("pick");
                         tower = point.BaseTower.PickTower(transform);
                    }
                    else if (GameManager.GetJState(point) == JState.PutDown)
                    {
                        tower.PutPower();
                    }
                    
                    break;
                case KeyCode.Escape:
                    UiManager.Instance.EscPreUi();
                    break;
                case KeyCode.A:
                    UiManager.Instance.PreOrNext(false);
                    break;
                case KeyCode.D:
                    UiManager.Instance.PreOrNext(true);
                    break;
                case KeyCode.W:
                    UiManager.Instance.PreOrNext(false);
                    break;
                case KeyCode.S:
                    UiManager.Instance.PreOrNext(true);
                    break;
            }
        }
            
        private void CheckKeyUp(KeyCode key)
        {
            switch (key)
            {
                
            }   
        }
      
        
    }

}
