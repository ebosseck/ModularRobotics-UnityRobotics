using Robotics.Structs;

namespace Robotics.Interfaces
{
    public interface IRoboticDriver
    {
        /// <summary>
        /// Updates the axis data used by this driver
        /// </summary>
        /// <param name="axes">Robotic axes to use</param>
        public void updateAxisData(RoboticAxis[] axes);
    }
}