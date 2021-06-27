using System.Collections;
using System.Collections.Generic;
using tower;
using UnityEngine;

namespace Player
{
    
    [CreateAssetMenu(fileName = "buildRes", menuName = "BuildRes", order = 3)]
    public class BuildRes : ScriptableObject
    {
        public ResType resType;
        public float digTime;
    }

}
