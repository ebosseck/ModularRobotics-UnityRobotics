using System;
using Robotics.Abstract;
using Robotics.Structs;

namespace Robotics.Controller.RoboticBase
{
    [Serializable]
    public class StaticRoboticBaseControler: AbstractRoboticBase
    {
        /// <summary>
        /// returns the type of robotic base
        /// </summary>
        /// <returns>STATIC</returns>
        // Overrides AbstractRoboticBase.getBaseType()
        public override RoboticBaseType getBaseType()
        {
            return RoboticBaseType.STATIC;
        }
    }
}