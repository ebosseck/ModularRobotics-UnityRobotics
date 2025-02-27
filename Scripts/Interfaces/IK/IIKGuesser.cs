using UnityEngine;

namespace Robotics.Interfaces.IK
{
    public interface IIKGuesser
    {
        /// <summary>
        /// Computes a guess for the given robotic axes
        /// </summary>
        /// <param name="transform">Transform used as Target</param>
        /// <param name="arm">Robotic Arm to compute guess for</param>
        /// <returns></returns>
        public float[] getGuess(Transform transform, IRoboticArm arm);

    }
}