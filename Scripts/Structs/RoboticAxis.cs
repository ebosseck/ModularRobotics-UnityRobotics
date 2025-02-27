using System;
using EditorTools.Attributes;
using UnityEngine;

namespace Robotics.Structs
{
    /// <summary>
    /// Struct containing all values relevant to robotic axes
    /// </summary>
    [Serializable]
    public class RoboticAxis
    {
        [Tooltip("Position of the axis in relation to the previous axis")]
        [Unit("m")]
        public Vector3 position;
        [Tooltip("Rotation direction of the axis")]
        public Vector3 direction;
        
        [Header("Rotation Constraints")]
        [Tooltip("Minimal rotation asngle relative to the default rotation state")]
        [Unit("°")]
        public float minimumAngle;
        [Tooltip("Maximal rotation angle relative to the default rotation state")]
        [Unit("°")]
        public float maximumAngle;
        [Tooltip("Maximum angular Velocity [°/s]")]
        [Unit("°/s")]
        public float maximumAngularVelocity;
        
        [Header("Rendering")]
        [Tooltip("GameObject used for rendering the part moved by this axis")]
        public GameObject gameObject;

        [Header("Current Data")] 
        [Tooltip("Current Angle of the robot arm on this axis")]
        [Range(-360, 360)]
        [DynamicRange("minimumAngle", "maximumAngle")]
        [Unit("°")]
        [TypePrecision(32)]
        public float currentAngle;
    }
}