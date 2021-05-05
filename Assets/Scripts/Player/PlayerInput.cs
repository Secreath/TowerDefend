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

        private GameObject pickTower;
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
                    Point point = GameManager.Instance.GetPointByPos(transform.position);
                    if (!point.HadTower  && GameManager.Instance.gameState != GameState.PickTower)
                        UiManager.Instance.JKeyPress(transform.position);
                    else if(point.HadTower && GameManager.Instance.gameState == GameState.PlayGame)
                    {
                        PickTower(point);
                    }
                    else if (!point.HadTower && GameManager.Instance.gameState == GameState.PickTower)
                    {
                        SetTower(point);
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

        public void PickTower(Point point)
        {
            pickTower = point.BaseTower.gameObject;
            pickTower.transform.SetParent(transform);
            pickTower.transform.localPosition = Vector3.zero;
            point.RemoveTower();
            GameManager.Instance.ChangeGameState(GameState.PickTower);
        }

        public void SetTower(Point point)
        {
            pickTower.transform.SetParent(null);
            pickTower.transform.position = point.CenterPos;
            pickTower.GetComponent<SpriteRenderer>().sortingOrder = point.Y;
            point.SetTower(pickTower.GetComponent<BaseTower>());
            GameManager.Instance.ChangeGameState(GameState.PlayGame);
        }
    }

}
