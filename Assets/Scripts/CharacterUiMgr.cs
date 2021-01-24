using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi
{
    public class CharacterUiMgr : MonoBehaviour
    {
        //hp
        private Image greenHp;

        private void Start()
        {
            greenHp = transform.Find("HPBar").Find("green").GetComponent<Image>();
        }

        public void HpBarUpdate(int curHp,int maxHp)
        {
            greenHp.fillAmount = (float)curHp / maxHp;
        }
    }
    

}
