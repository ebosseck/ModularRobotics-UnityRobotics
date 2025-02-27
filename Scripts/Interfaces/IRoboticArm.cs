using System.Collections;
using System.Collections.Generic;
using Robotics.Structs;
using UnityEngine;

namespace Robotics.Interfaces
{
    public interface IRoboticArm
    {
        /// <summary>
        /// Returns the base of this Robotic arm
        /// </summary>
        /// <returns>IRoboticBase this robot arm is mounted to</returns>
        public IRoboticBase getBase();
        
        /// <summary>
        /// Updates all the robotic arm axes
        /// </summary>
        public void updateAxisAngles();

        /// <summary>
        /// Sets all current angles for the axes
        /// </summary>
        /// <param name="axes">Array containing all current values for all axes</param>
        public void setAxisAngles(float[] axes);

        /// <summary>
        /// Gets all RoboticAxes 
        /// </summary>
        /// <returns>all robotic axes</returns>
        public RoboticAxis[] getAxes();
    }
}
