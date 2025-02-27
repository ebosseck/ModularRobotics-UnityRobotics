using System;
using EditorTools.Display;
using Robotics.Abstract.IK;
using Robotics.Structs;
using Robotics.Interfaces;
using UnityEngine;

namespace Robotics.Driver.IK.Guesser
{
    /// <summary>
    /// Provides initial IK guesses for PUMA-like 6 DOF robotic arms 
    /// </summary>
    [Serializable]
    public class PUMAGeometricGuesser: AbstractIKGuesser
    {

        [Header("Debugging Options")]
        public bool drawDebugLines = false;
        public int motionPathRetention = 10;
        
        private Vector3 previousHeadPosition;
        private Vector3 previousTransformPosition;
        private Vector3 previousModPosition;
        
        /// <summary>
        /// Guesses the configuration of a robotic arm based oion the desired endeffector transform
        /// </summary>
        /// <param name="transform">Transform of the desired end effector configuration</param>
        /// <param name="arm">Robotic arm to compute the quess for</param>
        /// <returns>a float[] containing the roboArms configuration</returns>
        public override float[] getGuess(Transform transform, IRoboticArm arm)
        {
            //Based on https://bjpcjp.github.io/pdfs/robotics/MR_ch06_inverse_kinematics.pdf
            
            Vector3 localTransform = (transform.position - arm.getBase().getMountingPoint().position);
            
            RoboticAxis[] axes = arm.getAxes();

            Vector3 transformA2_Local = axes[1].gameObject.transform.InverseTransformPoint(transform.position);
            
            float dLocal = transformA2_Local.magnitude;
            
            float a = axes[3].gameObject.transform.localPosition.magnitude + axes[4].gameObject.transform.localPosition.magnitude;
            float b = axes[2].gameObject.transform.localPosition.magnitude;
            
            float cc = Math.Min(dLocal, a + b);
            
            Vector3 correctedTransform = transformA2_Local.normalized * cc;
            Vector3 cGlobal = axes[1].gameObject.transform.TransformPoint(correctedTransform);
            localTransform = (cGlobal - arm.getBase().getMountingPoint().position);
            
            float r = localTransform.magnitude;
            
            float[] angles = new float[6];

            float d1 = (axes[1].position.y);
            
            angles[0] = (float) -(((Math.PI/2) + Math.Atan2(localTransform.z, localTransform.x) - Math.Atan2(d1, Math.Sqrt((r*r) + (d1 * d1)))) * Mathf.Rad2Deg);
            
            float alpha_component = 0;
            

            if (dLocal > a + b)
            {
                angles[2] = 90;
                alpha_component = 0;
            }
            else
            {
                angles[2] = -90 + (float)Math.Acos(((a * a) + (b * b) - (cc * cc)) / (2 * a * b)) * Mathf.Rad2Deg;
                alpha_component = (float)Math.Acos(((cc * cc) + (b * b) - (a * a)) / (2 * cc * b)) * Mathf.Rad2Deg;
            }

            
            float e = (-(new Vector3(0.5f, 0, 0.5f) * cc) + correctedTransform).magnitude;
            
            float alpha_0 = (float)Math.Acos((2*(cc * cc) - (e * e)) / (2 * cc*cc)) * Mathf.Rad2Deg;

            angles[1] = -(alpha_component + alpha_0) +90;
            
            
            if (drawDebugLines)
            {
                drawDebug(transform, arm, axes, cGlobal);
            }

            return angles;
            
            
        }

        /// <summary>
        /// Draw debugging information in unitys scene
        /// </summary>
        /// <param name="transform">reference frame</param>
        /// <param name="arm">Robotic arm</param>
        /// <param name="axes">axes of the robotic arm</param>
        /// <param name="cglobal">goal position</param>
         private void drawDebug(Transform transform, IRoboticArm arm, RoboticAxis[] axes, Vector3 cglobal)
         {
             Debug.DrawLine(axes[2].gameObject.transform.position, axes[1].gameObject.transform.position, Color.green, 0, false);
             Debug.DrawLine(axes[4].gameObject.transform.position, axes[2].gameObject.transform.position, Color.red, 0, false);
             Debug.DrawLine(axes[4].gameObject.transform.position, axes[1].gameObject.transform.position, Color.blue, 0, false);
            
             Debug.DrawLine(arm.getBase().getMountingPoint().position, axes[1].gameObject.transform.position, Color.magenta, 0, false);

             if (previousHeadPosition != null)
             {
                 Debug.DrawLine(previousHeadPosition, axes[4].gameObject.transform.position, Color.magenta, motionPathRetention, false);
             }

             if (previousTransformPosition != null)
             {
                 Debug.DrawLine(previousTransformPosition, transform.position, Color.yellow, motionPathRetention, false);
             }

             if (cglobal != null)
             {
                 Debug.DrawLine(cglobal, previousModPosition, Color.green, motionPathRetention, false);
             }
             
             Geometry.drawAABB(cglobal - Vector3.one*0.1f, Vector3.one*0.2f, Color.green);
             
             previousHeadPosition = axes[4].gameObject.transform.position;
             previousTransformPosition = transform.position;
             previousModPosition = cglobal;
         }

         /*
          public override float[] getGuess(Transform transform, IRoboticArm arm)
        {
            //Based on https://bjpcjp.github.io/pdfs/robotics/MR_ch06_inverse_kinematics.pdf
            
            Vector3 localTransform = (transform.position - arm.getBase().getMountingPoint().position);
            RoboticAxis[] axes = arm.getAxes();
            
            float r = localTransform.magnitude;
            
            float[] angles = new float[6];

            float d1 = (axes[1].position.y) * 100;
            
            angles[0] = (float) -(((Math.PI/2) + Math.Atan2(localTransform.z, localTransform.x) - Math.Atan2(d1, Math.Sqrt((r*r) + (d1 * d1)))) * Mathf.Rad2Deg);

            double a2 = (axes[2].gameObject.transform.position - axes[1].gameObject.transform.position).magnitude;
            double a3 = (axes[4].gameObject.transform.position - axes[2].gameObject.transform.position).magnitude;

            if (drawDebugLines)
            {
                drawDebug(transform, arm, axes);
            }

            //double D = (localTransform.sqrMagnitude - (d1 * d1) - (a2 * a2) - (a3 * a3)) / 2 * a2 * a3;
            localTransform = (localTransform - axes[1].gameObject.transform.localPosition);
            r = localTransform.magnitude;
            d1 = 0;
            
            double D = ((r * r) - (d1 * d1) + (localTransform.y * localTransform.y) - (a2 * a2) - (a3 * a3)) /
                       (2 * a2 * a3);

            // D = Math.Min(D, 1);
            
            double rad3 = - Math.Atan2(Math.Sqrt(1 - (D * D)), D);
            
            
            angles[2] = (float) (((Math.PI/2) + rad3) * Mathf.Rad2Deg);
            angles[1] = (float) ((-(Math.PI/2) + (Math.Atan2(localTransform.y, Math.Sqrt((localTransform.x * localTransform.x) + (localTransform.z * localTransform.z) - (d1 * d1))) -
                                              Mathf.Atan2((float)(a3 * Math.Sin(rad3)), (float)(a2 + (a3 + Math.Cos(rad3)))))) * Mathf.Rad2Deg);
            
            return angles;
            
            
        }
         */
    }
}