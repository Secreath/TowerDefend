using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tower
{
   public class UpGradeTower : MonoBehaviour
   {
      public int UpgradeType;

      public Point point;
      private void Start()
      {
         EventCenter.GetInstance().AddEventListener("GameManagerInit",Init);   
         
      }

      private void Init()
      {
         Vector3Int pointPos = VTool.ToPointPos(transform.position);
         if (!GameManager.Instance.HadThisPoint(pointPos))
            return;
         point = GameManager.Instance.GetPointByPos(pointPos);
         transform.position = point.CenterPos;
      }

      
      
      
      
      private void OnDestroy()
      {
         EventCenter.GetInstance().RemoveEventListener("GameManagerInit", Init);
      }
      
   }
   
   

}
