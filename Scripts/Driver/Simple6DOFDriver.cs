using System.Collections;
using EditorTools.Attributes;
using EditorTools.BaseTypes;
using Robotics.Structs;
using Robotics.Interfaces;
using UnityEngine;

namespace Robotics.Driver
{
    /// <summary>
    /// Driver capable of driving the robot by setting axis angles with sliders in property inspector
    /// </summary>
    public class Simple6DOFDriver : ExtendedMonoBehaviour, IRoboticDriver
    {

        [HideInInspector]
        public float a1_min = 0;
        [HideInInspector]
        public float a1_max = 0;
        
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("a1_min", "a1_max")]
        [Unit("°")]
        public float axis1 = 0;
        
        [HideInInspector]
        public float a2_min = 0;
        [HideInInspector]
        public float a2_max = 0;
        
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("a2_min", "a2_max")]
        [Unit("°")]
        public float axis2 = 0;
        
        [HideInInspector]
        public float a3_min = 0;
        [HideInInspector]
        public float a3_max = 0;
        
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("a3_min", "a3_max")]
        [Unit("°")]
        public float axis3 = 0;
        
        [HideInInspector]
        public float a4_min = 0;
        [HideInInspector]
        public float a4_max = 0;
        
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("a4_min", "a4_max")]
        [Unit("°")]
        public float axis4 = 0;
        
        [HideInInspector]
        public float a5_min = 0;
        [HideInInspector]
        public float a5_max = 0;
        
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("a5_min", "a5_max")]
        [Unit("°")]
        public float axis5 = 0;
        
        [HideInInspector]
        public float a6_min = 0;
        [HideInInspector]
        public float a6_max = 0;
        
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("a6_min", "a6_max")]
        [Unit("°")]
        public float axis6 = 0;

        /// <summary>
        /// Update the axis data used by this driver
        /// </summary>
        /// <param name="axes">Robotic axes</param>
        public void updateAxisData(RoboticAxis[] axes)
        {
            a1_min = axes[0].minimumAngle;
            a1_max = axes[0].maximumAngle;

            a2_min = axes[1].minimumAngle;
            a2_max = axes[1].maximumAngle;
            
            a3_min = axes[2].minimumAngle;
            a3_max = axes[2].maximumAngle;
            
            a4_min = axes[3].minimumAngle;
            a4_max = axes[3].maximumAngle;
            
            a5_min = axes[4].minimumAngle;
            a5_max = axes[4].maximumAngle;
            
            a6_min = axes[5].minimumAngle;
            a6_max = axes[5].maximumAngle;
        }
        
        /// <summary>
        /// Called from custom property inspector if a value got changed in the inspector
        /// </summary>
        /// <param name="origin">Name of event origin field</param>
        /// <param name="oldValue">old value of field</param>
        /// <param name="newValue">new value for field</param>
        public override void OnInspectorValueChanged(string origin, object oldValue, object newValue)
        {
            StartCoroutine(updateObjectFromInspector());
        }

        /// <summary>
        /// Async method tu update robot axes from inspector
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateObjectFromInspector()
        {
            yield return new WaitForSecondsRealtime(0.05f);

            IRoboticArm robotArm = gameObject.GetComponent<IRoboticArm>();
            
            if (robotArm != null)
            {
                robotArm.setAxisAngles(new []{axis1, axis2, axis3, axis4, axis5, axis6});
            }
            else
            {
                Debug.LogWarning("No IRoboticArm found. Please add or specify an IRoboticArm component to this GameObject.");
            }
        }
    }
}