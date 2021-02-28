using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Bt_Node
    {
        public virtual Bt_Result Do_Action()
        {
            return Bt_Result.None;
        }
    }

    //组合节点
    public class Bt_Composite : Bt_Node
    {
        protected List<Bt_Node> children;

        public Bt_Composite()
        {
            children = new List<Bt_Node>();
        }

        public void AddChild(Bt_Node node)
        {
            children.Add(node);
        }
    }
    
    //选择节点
    public class Bt_Select : Bt_Composite
    {
        protected int index;

        public Bt_Select()
        {
            Reset();
        }

        public override Bt_Result Do_Action()
        {
            if (this.children == null || this.children.Count == 0)
            {
                return Bt_Result.Fail;
            }

            if (index >= this.children.Count)
            {
                Reset();
            }

            Bt_Result result = Bt_Result.None;

            for (int length = this.children.Count; index < length; index++)
            {
                result = this.children[index].Do_Action();

                if (result == Bt_Result.Successful)
                {
                    Reset();
                    return result;
                }
                if (result == Bt_Result.Runing)
                {
                    return result;
                }
                
                continue;
                
            }
            Reset();
            return Bt_Result.Fail;
        }

        private void Reset()
        {
            index = 0;
        }
    }

    public class Bt_Sequence : Bt_Composite
    {
        private int index;

        public Bt_Sequence()
        {
            Reset();   
        }

        public override Bt_Result Do_Action()
        {
            if (this.children == null || this.children.Count == 0)
            {
                return Bt_Result.Fail;
            }

            if (this.index >= this.children.Count)
            {
                Reset();
            }

            Bt_Result result = Bt_Result.None;

            for (int length = this.children.Count; index < length; index++)
            {
                result = this.children[index].Do_Action();

                if (result == Bt_Result.Fail)
                {
                    Reset();
                    return result;
                }

                if (result == Bt_Result.Runing)
                {
                    return result;
                }
                
                continue;
            }
            
            
            return Bt_Result.Successful;
        }
        
        private void Reset()
        {
            index = 0;
        }
    }

    //并行节点
    public class Bt_Parallel : Bt_Composite
    {
        public Bt_Parallel()
        {
            
        }
    }
    
    //并行选择节点
    //一假全假 一真全真
    public class Bt_ParallelSelector : Bt_Parallel
    {
        private List<Bt_Node> m_pWaitNodes;
        private bool m_pIsFail;

        public Bt_ParallelSelector()
        {
            m_pWaitNodes = new List<Bt_Node>();
            m_pIsFail = false;
        }

        public override Bt_Result Do_Action()
        {
            if (this.children == null || this.children.Count == 0)
            {
                return Bt_Result.Fail;
            }

            Bt_Result result = Bt_Result.None;
            List<Bt_Node> waitNodes = new List<Bt_Node>();
            List<Bt_Node> mainNodes = new List<Bt_Node>();
            
            mainNodes = this.m_pWaitNodes.Count > 0 ? this.m_pWaitNodes : this.children;

            for (int length = mainNodes.Count, i = 0; i < length; i++)
            {
                result = mainNodes[i].Do_Action();
                switch (result)
                {
                    case Bt_Result.Successful:
                        break;
                    case Bt_Result.Runing:
                        waitNodes.Add(mainNodes[i]);
                        break;
                    default:
                        m_pIsFail = true;
                        break;
                    
                }
            }

            if (waitNodes.Count > 0)
            {
                this.m_pWaitNodes = waitNodes;
                return Bt_Result.Runing;
            }

            result = CheckResult();
            Reset();
            return result;
        }

        private Bt_Result CheckResult()
        {
            return m_pIsFail ? Bt_Result.Fail : Bt_Result.Successful;
        }

        private void Reset()
        {
            this.m_pWaitNodes.Clear();
            this.m_pIsFail = false;
        }
    }
    
    //并行顺序节点
    //一真则真 全假则假
    public class Bt_ParallelSequence : Bt_Parallel
    {
        private List<Bt_Node> m_pWaitNodes;
        private bool m_PIsSuccess;

        public Bt_ParallelSequence()
        {
            m_pWaitNodes = new List<Bt_Node>();
            m_PIsSuccess = false;
        }

        public override Bt_Result Do_Action()
        {
            if (this.children == null || this.children.Count == 0)
            {
                return Bt_Result.Successful;
            }

            Bt_Result result = Bt_Result.None;
            
            List<Bt_Node> waitNodes = new List<Bt_Node>();
            List<Bt_Node> mainNodes = new List<Bt_Node>();

            mainNodes = this.m_pWaitNodes.Count > 0 ? this.m_pWaitNodes : this.children;

            for (int length = mainNodes.Count, i = 0; i < length; i++)
            {
                result = mainNodes[i].Do_Action();

                switch (result)
                {
                    case Bt_Result.Successful:
                        this.m_PIsSuccess = true;
                        break;
                    case Bt_Result.Runing:
                        waitNodes.Add(mainNodes[i]);
                        break;
                    default:
                        break;
                }
            }

            //存在等待节点就返回等待节点
            if (waitNodes.Count > 0)
            {
                this.m_pWaitNodes = waitNodes;
                return Bt_Result.Runing;
            }
            //检查返回结果
            this.m_pWaitNodes = waitNodes;
            Reset();
            return Bt_Result.Runing;
        }

        private Bt_Result CheckResult()
        {
            return this.m_PIsSuccess ? Bt_Result.Successful : Bt_Result.Fail;
        }

        private void Reset()
        {
            this.m_pWaitNodes.Clear();
            this.m_PIsSuccess = false;
        }
    }

    //装饰节点
    public class Bt_Decotator : Bt_Node
    {
        private Bt_Node child;

        public Bt_Decotator()
        {
            child = null;
        }

        protected void SetChil(Bt_Node node)
        {
            this.child  = node;
        }
    }
    //条件节点
    public class Bt_Condition : Bt_Node
    {
        public override Bt_Result Do_Action()
        {
            return Bt_Result.Fail;
        } 
    }
    //行为节点
    public class Bt_Action : Bt_Node
    {
        
    }
}