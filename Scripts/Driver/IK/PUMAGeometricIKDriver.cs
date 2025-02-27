using EditorTools.BaseTypes;
using Robotics.Driver.IK.Guesser;
using Robotics.Structs;
using Robotics.Interfaces;
using Robotics.Interfaces.IK;
using UnityEngine;

namespace Robotics.Driver.IK
{
    /// <summary>
    /// Driver driving the robotic arm based on PUMA guesses. This may be wildly incorrect
    /// </summary>
    [ExecuteInEditMode]
    public class PUMAGeometricIKDriver : ExtendedMonoBehaviour, IRoboticDriver
    {
        public Transform followObject;
        

        private IIKGuesser pumaGuesser = new PUMAGeometricGuesser();
        
        /// <summary>
        /// Update the axis data
        /// </summary>
        /// <param name="axes"> Axes of the robot</param>
        public void updateAxisData(RoboticAxis[] axes)
        {
            // Not Implemented
        }

        /// <summary>
        /// Unity Event, called on frame update
        /// </summary>
        public void Update()
        {
            IRoboticArm roboticArm = gameObject.GetComponent<IRoboticArm>();
            roboticArm.setAxisAngles(computeAngles(roboticArm));
            roboticArm.updateAxisAngles();
            
        }

        /// <summary>
        /// Compute all roboric axis angles
        /// </summary>
        /// <param name="arm">arm to calculate angles for</param>
        /// <returns>calculated angles</returns>
        private float[] computeAngles(IRoboticArm arm)
        {
            return pumaGuesser.getGuess(followObject, arm);
        }
    }
}