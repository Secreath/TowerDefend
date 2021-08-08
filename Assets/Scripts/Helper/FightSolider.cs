using System.Collections;
using System.Collections.Generic;
using GameUi;
using ui;
using UnityEngine;

namespace solider
{
    
    public class FightSolider : Soldier,IAttack
    {
        public Vector3 uiOffSet;
        protected CharacterUiMgr uimgr;


        protected override void Start()
        {
            base.Start();
            uimgr = UiManager.EnemyUi.PopCharUiBar();
        }

        protected override void Update()
        {
            base.Update();
            SetUiPos(transform.position);
        }
        public virtual void Attack()
        {
            if(_enemy == null)
                return;
            
            _enemy.GetComponent<BaseEnemy>().TakeDamage(_curProperty.atk);
        }
        
        protected void SetUiPos(Vector3 pos)
        {
            uimgr.SetPos(pos + uiOffSet);
        }
        
    }

}
