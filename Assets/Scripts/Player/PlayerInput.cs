using System;
using System.Collections;
using System.Collections.Generic;
using Tower;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerInput : Singleton<PlayerInput>
    {
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
                    UiManager.Instance.ShowTowerUi(transform.position);
                    break;
                case KeyCode.Escape:
                    UiManager.Instance.EscPreUi();
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
