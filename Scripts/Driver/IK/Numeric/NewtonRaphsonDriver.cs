using EditorTools.Attributes;
using EditorTools.BaseTypes;
using Robotics.Abstract.IK;
using Robotics.Structs;
using Robotics.Interfaces;
using UnityEngine;

namespace Robotics.Driver.IK.Numeric
{
    /// <summary>
    /// Unfinished implementation of Newton Raphson IK solver
    /// </summary>
    [ExecuteInEditMode]
    public class NewtonRaphsonDriver : ExtendedMonoBehaviour, IRoboticDriver
    {
        
        [Header("General Settigns")]
        [Tooltip("Object to follow")]
        public Transform followObject = null;
        
        
        private RoboticAxis[] axes = null;

        [Header("Solver Options")] 
        [Tooltip("Iterations of the algorithmn until a solution is accepted")]
        public int iterations = 10;

        [Tooltip("Accuracy of the Solver for early abort")] 
        [Unit("m")]
        public float accuracy = 0.0001f;

        public float stepSize = 0.05f;
        public float thetaMaxStep = 0.2f;
        
        [Tooltip("Object used for obtaining initial Guesses")]
        public AbstractIKGuesser guesser = null;
        
        [Tooltip("Use previons robotic arm position as initial guess on the target position")]
        public bool usePrevious = false;
        
        
        /// <summary>
        /// Update axis data from extern
        /// </summary>
        /// <param name="axes">new values for robotic axes</param>
        public void updateAxisData(RoboticAxis[] axes)
        {
            this.axes = axes;
        }
        
        /// <summary>
        /// Unity Event, called on each frame
        /// </summary>
        public void Update()
        {
            // Not Implemented
            IRoboticArm arm = GetComponent<IRoboticArm>();
            arm.setAxisAngles(computeAxisData(arm));
            arm.updateAxisAngles();
        }

        /// <summary>
        /// Computes the configuration for the robotic arm
        /// </summary>
        /// <param name="arm">Arm to compute the configuration for</param>
        /// <returns></returns>
        public float[] computeAxisData(IRoboticArm arm)
        {
            float[] guess = new float[] {90, 0, 0, 0, 0, 0};
            if (guesser != null)
            {
                guess = guesser.getGuess(followObject, arm);
            }


            return guess;
        }

        /// <summary>
        /// Method to calculate IK config from initial guess
        /// </summary>
        /// <param name="guess">Initial guess</param>
        /// <returns>IK config</returns>
        public float[] calculateIK(float[] guess)
        {
            IRoboticArm arm = GetComponent<IRoboticArm>();
            applyAngles(arm, guess);
            
            Vector3 d_pos = followObject.position - getEndeffectorWorldPos(axes);

            for (int i = 0; i < iterations; i++)
            {
                if (d_pos.magnitude < accuracy)
                {
                    break; // Break if desired accuracy is reached before max iterations reached
                }

                Vector3 steppos = d_pos * stepSize / d_pos.magnitude;
                
                //Matrix jacobian = generateJacobian(guess, arm);
                
                // TODO: PseudoInverse loop here !
                
            }
            
            

            
            
            return guess;
        }

        #region Utils

        /// <summary>
        /// Apply the given angles to the given robotic arm
        /// </summary>
        /// <param name="roboticArm">Robotic arm to apply angles to</param>
        /// <param name="angles">angles to apply</param>
        public void applyAngles(IRoboticArm roboticArm, float[] angles)
        {
            roboticArm.setAxisAngles(angles);
            roboticArm.updateAxisAngles();
        }

    /*    public Matrix4x4 generateLinkTransformMatrix(RoboticAxis axis)
        {
            Matrix4x4 mat = Matrix4x4.Rotate(Quaternion.AngleAxis(axis.currentAngle, axis.direction));
            mat.m03 = axis.position.x;
            mat.m13 = axis.position.y;
            mat.m23 = axis.position.z;

            return mat;
        } */

        /// <summary>
        /// gets the world transform for the axis with the given index
        /// </summary>
        /// <param name="arm"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Matrix4x4 getAxisWorldTransform(IRoboticArm arm, int index)
        {
            GameObject go = arm.getAxes()[index].gameObject;
            if (go != null)
            {
                return go.transform.localToWorldMatrix;
            }

            return Matrix4x4.identity;
        }

        /// <summary>
        /// returns the world position of the endeffector
        /// </summary>
        /// <param name="axes"></param>
        /// <returns></returns>
        public Vector3 getEndeffectorWorldPos(RoboticAxis[] axes)
        {
            return axes[axes.Length - 1].gameObject.transform.position;
        }

     /*   public Matrix generateJacobian(float[] guess, IRoboticArm arm)
        {
            RoboticAxis[] axes = arm.getAxes();
            Vector3 endeffectorWorldCoords = getEndeffectorWorldPos(axes);
            
            Matrix jacobian = new Matrix(3, guess.Length);

            for (int i = 0; i < axes.Length; i++)
            {

                Vector3 cross = Vector3.Cross(axes[i].direction,
                    (axes[i].gameObject.transform.position - endeffectorWorldCoords));
                
                jacobian.setColumn(i, new double[]{cross.x, cross.y, cross.z});
            }
            
            return jacobian;
        }
    */
        #endregion
    }
}


