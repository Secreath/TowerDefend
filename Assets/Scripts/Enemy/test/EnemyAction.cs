using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace Enemy
{
    public class Walk : Bt_Action
    {
        private Animator animator;
        private Bt_Result result = Bt_Result.Successful;

        public Walk(Animator animator)
        {
            this.animator = animator;
        }

        public override Bt_Result Do_Action()
        {
            DoWalk();
            return result;
        }

        void DoWalk()
        {
            animator.SetBool("isWalk",true);
            Debug.Log("waalk");
        }
    }
    
    public class Attack : Bt_Action
    {
        private Animator animator;
        private Bt_Result result = Bt_Result.Successful;

        public Attack(Animator animator)
        {
            this.animator = animator;
        }

        public override Bt_Result Do_Action()
        {
            DoAttack();
            return result;
        }

        void DoAttack()
        {
            animator.SetBool("isAttack",true);
            Debug.Log("attack");
        }
    }
    
    public class Die : Bt_Action
    {
        private Animator animator;
        private Bt_Result result = Bt_Result.Successful;

        public Die(Animator animator)
        {
            this.animator = animator;
        }

        public override Bt_Result Do_Action()
        {
            DoDie();
            return result;
        }

        void DoDie()
        {
            animator.SetBool("isDie",true);
            Debug.Log("Die");
        }
    }
    

}