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
         UiManager.Instance.ShowUpgradeUi(this,CurPoint);
         
      }

      public void UpGrade()
      {
         Debug.Log("UpGrade");
         state = TowerState.Upgrading;
         buildTower.OnBuilding();
         timeCountDown.SetRefreshTime(10,BuildOver);
      }
      
      private void BuildOver()
      {
         Debug.Log("BuildOver");
         state = TowerState.Idle;
         buildTower.OnBuildOver();
         buildTower = default;
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
