using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ui
{
    public class CircleChoosePop : MonoBehaviour
    {
        public GameObject button;

        public float radius;
        public float startAngle = 58.12f;

        private List<GameObject> buttons;

        public void InitCircle(int count)
        {
            buttons = new List<GameObject>();
            
            float deltaTheta = (2f* Mathf.PI) / count;
            float theta = startAngle; //当前角度
            for (var i = 0; i < count; i++)
            {
                Vector2 pos = (transform as RectTransform).anchoredPosition + new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
                GameObject obj = Instantiate(button);
                theta = deltaTheta + theta;
                
                RectTransform rect = obj.transform as RectTransform;
                rect.SetParent(transform,false);
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = pos;
                buttons.Add(obj);
            }
            gameObject.SetActive(false);
            Debug.Log(UiManager.Instance.UiCamera.name);
        }

        


    //    private void UpdatePos()
    //    {
    //        float deltaTheta = (2f* Mathf.PI) / count;
    //        float theta = startAngle; //当前角度
    //        for (var i = 0; i < count; i++)
    //        {
    //            Vector3 pos = transform.position + new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
    //            buttons[i].transform.position = pos;
    //            theta = deltaTheta + theta;
    //        }
    //    }
      
    }
    

}
