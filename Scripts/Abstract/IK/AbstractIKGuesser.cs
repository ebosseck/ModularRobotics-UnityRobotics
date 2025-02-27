using System;
using Robotics.Interfaces;
using Robotics.Interfaces.IK;
using UnityEngine;

namespace Robotics.Abstract.IK
{
    /// <summary>
    /// Abstract base class for different kinds of Inverse Kinematics guessers
    /// </summary>
    [Serializable]
    public class AbstractIKGuesser : MonoBehaviour, IIKGuesser
    {
        /// <summary>
        /// Guesses the configuration of a robotic arm based oion the desired endeffector transform
        /// </summary>
        /// <param name="transform">Transform of the desired end effector configuration</param>
        /// <param name="arm">Robotic arm to compute the quess for</param>
        /// <returns>a float[] containing the roboArms configuration</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual float[] getGuess(Transform transform, IRoboticArm arm)
        {
            throw new NotImplementedException();
        }
    }
}