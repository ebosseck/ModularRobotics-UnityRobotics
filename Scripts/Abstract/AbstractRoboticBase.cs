using System;
using EditorTools.BaseTypes;
using Robotics.Structs;
using Robotics.Interfaces;
using UnityEngine;

namespace Robotics.Abstract
{
    /// <summary>
    /// Base Class for all robotic bases
    /// </summary>
    [Serializable]
    public class AbstractRoboticBase : ExtendedMonoBehaviour, IRoboticBase
    {
        [Header("Mounting Point")]
        [Tooltip("Mounting Point for the robotic arm")]
        public Transform[] robotMountingPoints;
        
        /// <summary>
        /// gets the first mounting point for a robot
        /// </summary>
        /// <returns></returns>
        // Implements IRoboticBase.getMountingPoint()
        public virtual Transform getMountingPoint()
        {
            return getMountingPoint(0);
        }

        /// <summary>
        /// Gets the robot mounting point with the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Transform getMountingPoint(int index)
        {
            return robotMountingPoints[index];
        }

        /// <summary>
        /// returns the type of this base
        /// </summary>
        /// <returns>RoboticBaseType of this base</returns>
        // Implements IRoboticBase.getBaseType()
        public virtual RoboticBaseType getBaseType()
        {
            return RoboticBaseType.ABSTRACT;
        }
    }
}