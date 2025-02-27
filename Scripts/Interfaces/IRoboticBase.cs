using System;
using Robotics.Structs;
using UnityEngine;

namespace Robotics.Interfaces
{
    
    public interface IRoboticBase
    {
        /// <summary>
        /// Returns the mounting point of this robotic base
        /// </summary>
        /// <returns>The Mounting Point for an robotic arm</returns>
        public Transform getMountingPoint();
        
        /// <summary>
        /// Returns the mounting point of this robotic base
        /// </summary>
        /// <param name="index">Index of the mounting point in case the base has multiple mounting points</param>
        /// <returns>The Mounting Point for an robotic arm</returns>
        public Transform getMountingPoint(int index);
        
        //public Transform getRootTransform();
        
        /// <summary>
        /// Returns the type of this robotic base
        /// </summary>
        /// <returns>the RoboticBaseType of this IRoboticBase</returns>
        public RoboticBaseType getBaseType();
    }
}