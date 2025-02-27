using EditorTools.Attributes;
using EditorTools.BaseTypes;
using Robotics.Structs;
using Robotics.Interfaces;
using UnityEngine;

namespace Robotics.Driver.IK
{
    /// <summary>
    /// Unfinished FastIK solver
    /// </summary>
    public class FastIKDriver : ExtendedMonoBehaviour, IRoboticDriver
    {
        [Header("Posing Parameters")]
        public Transform target = null;
        public Transform directionHint = null;

        [Header("Solver Parameters")] 
        public int iterations;

        [Unit("m")]
        public float delta;

        protected RoboticAxis[] roboticAxes;
        
        /// <summary>
        /// update the axes used for calculations in this solver
        /// </summary>
        /// <param name="axes"></param>
        public void updateAxisData(RoboticAxis[] axes)
        {
            roboticAxes = axes;
        }
    }
}