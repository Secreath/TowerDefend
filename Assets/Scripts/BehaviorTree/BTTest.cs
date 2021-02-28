using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public class BTTest : MonoBehaviour
    {
        private Bt_Node node;
        private void Start()
        {
            Debug.Log("BTTEST");

//            TestSelect();
//            TestSequence();
            TestParallelSelector();
        }

        void TestSelect()
        {
            Bt_Select node;
            node = new Bt_Select();
            
            node.AddChild(new Run());
            node.AddChild(new Idle());

            Debug.Log(node.Do_Action());
        }
        
        void TestSequence()
        {
            Bt_Sequence node;
            node = new Bt_Sequence();
            
            node.AddChild(new Run());
            node.AddChild(new Idle());

            Debug.Log(node.Do_Action());
        }
        
        void TestParallelSelector()
        {
            Bt_ParallelSelector node;
            node = new Bt_ParallelSelector();
            
            node.AddChild(new Run());
            node.AddChild(new Idle());

            Debug.Log(node.Do_Action());
        }
        
        void TestParallelSequence()
        {
            Bt_ParallelSequence node;
            node = new Bt_ParallelSequence();
            
            node.AddChild(new Run());
            node.AddChild(new Idle());

            Debug.Log(node.Do_Action());
        }
        
    }
    

    public class Idle : Bt_Action
    {
        private Animator animator;
        private Bt_Result result = Bt_Result.Fail;

        public Idle()
        {
//            this.animator = animator;
        }

        public override Bt_Result Do_Action()
        {
            DoIdle();
            return result;
        }

        void DoIdle()
        {
            Debug.Log("什么也不做～");
        }
    }

    public class Run : Bt_Action
    {
        private Animator animator;
        private Bt_Result result = Bt_Result.Successful;

        public Run()
        {
//            this.animator = animator;
        }

        public override Bt_Result Do_Action()
        {
            DoRun();
            return result;
        }

        void DoRun()
        {
            Debug.Log("逛街");
        }
    }
    
}
