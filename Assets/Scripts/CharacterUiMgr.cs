using System;
using System.Collections;
using System.Collections.Generic;
using ui;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi
{
    public class CharacterUiMgr : MonoBehaviour
    {
        //hp
        private Camera UiCamera;
        private RectTransform charUiRect;
        private Image greenHp;

        private Transform uiManagerTrans;
        private void Awake()
        {
            uiManagerTrans = UiManager.Instance.transform;
            charUiRect = transform.GetComponent<RectTransform>();
            greenHp = transform.Find("HPBar").Find("Green").GetComponent<Image>();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        public void SetUiCamera(Camera uiCamera)
        {
            UiCamera = uiCamera;
        }
        
        public void HpBarUpdate(int curHp,int maxHp)
        {
            greenHp.fillAmount = (float)curHp / maxHp;
        }

        public void ReSetUi()
        {
            greenHp.fillAmount = 1;
        }

        public void SetPos(Vector3 pos)
        {
            charUiRect.anchoredPosition = UiTool.GetTowerUiPos(uiManagerTrans, UiCamera, pos);
        }
    }
    

}
