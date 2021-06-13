using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using plugin.ugui.view;
using ui;
using UnityEngine;

namespace tower
{
   public class UpGradeTower : BaseTower
   {
      public UpgradeType Type;

      
      private TimeCountDown timeCountDown;
      
      private BaseTower buildTower;
      public BaseTower BuildTower => buildTower;
      public void Start()
      {
         EventCenter.GetInstance().AddEventListener("GameManagerInit",InitUpgrade); 
      }

      private void InitUpgrade()
      {
         Point point = CurPoint;
         transform.position = point.CenterPos;
         point.SetTower(this);
         GetTimer();
      }

      public void PrepareUpgreade(BaseTower pickTower)
      {
         buildTower = pickTower;
         if (buildTower.NextTower == null)
         {
            state = TowerState.UpgradComplete;
            return;
         }
         UiManager.Instance.ShowUpgradeUi(this,CurPoint);
         
      }

      public void UpGrade()
      {
         Debug.Log("UpGrade");
         state = TowerState.Upgrading;
         buildTower.OnBuilding();
         float buildTime = buildTower.NextTower.buildTime;
         timeCountDown.SetRefreshTime(buildTime,BuildOver);
      }
      
      private void BuildOver()
      {
         Debug.Log("BuildOver");
         state = TowerState.UpgradComplete;
         buildTower.OnBuildOver();
      }
      
      public override BaseTower PickTower(Transform player)
      {
         state = TowerState.Idle;
         buildTower.transform.SetParent(player);
         buildTower.transform.localPosition = Vector3.zero;
         GameManager.ChangeGameState(GameState.PickTower);
         BaseTower renturnTower = buildTower;
         buildTower = default;
         return renturnTower;
      }
      
      public void GetTimer()
      {
         UiManager.Instance.CreateTimer(ref timeCountDown,CurPoint);
      }
      
      private void OnDestroy()
      {
         EventCenter.GetInstance().RemoveEventListener("GameManagerInit", InitUpgrade);
      }

   }
   
   

}
