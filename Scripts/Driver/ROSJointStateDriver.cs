using System;
using System.Text;
using EditorTools.Attributes;
using EditorTools.BaseTypes;
using Robotics.Interfaces;
using Robotics.Structs;
using UnityEngine;

#if MTFS_ROS_CONNECTOR
using RosConnector.Abstract;
using ros_messages.control_msgs;
using ros_messages.sensor_msgs;
#endif

namespace Robotics.Driver
{
    /// <summary>
    /// Driver for robotic arms controlled by ROS
    /// </summary>
#if MTFS_ROS_CONNECTOR
    public class ROSJoinStateDriver : ROSBehaviour, IRoboticDriver
#else
    public class ROSJointTrajectoryDriver : ExtendedMonoBehaviour, IRoboticDriver
#endif
    {
        // Start is called before the first frame update

        private IRoboticArm roboticArm = null;

        [Header("ROS Settings")] [Tooltip("Topic this sensor should publish to")]
        public string topic = "";

        public bool isEnabled = true;


        [Header("Axis Settings")] [NonReorderable] [NonResizeable]
        public float[] axisMultiplier = new float[6];

        [NonReorderable] [NonResizeable] [Unit("°")]
        public float[] axisOffset = new float[6];

#if MTFS_ROS_CONNECTOR
        /// <summary>
        /// Called after the object is initialized. Here the connection to ROS is set up
        /// </summary>
        void Start()
        {

            Debug.Log("Initialized Robotic Controller");
            roboticArm = GetComponent<IRoboticArm>();
            Action<JointStateMsg> onMessageAction = onRosMessage;
            connection.Subscribe<JointStateMsg>(topic, onMessageAction);
        }

        // see IRoboticDriver
        public void updateAxisData(RoboticAxis[] axes)
        {
            // Currently not Implemented
        }

        /// <summary>
        /// Called when a new JointTrajectoryControllerState is received
        /// </summary>
        /// <param name="msg">JointTrajectoryControllerState Message received</param>
        private void onRosMessage(JointStateMsg msg)
        {
            Debug.Log("Message Received !");
            if (!isEnabled)
            {
                return;
            }

            double[] radians = msg.position.ToArray();

            float[] angles = new float[radians.Length];

            for (int i = 0; i < radians.Length; i++)
            {
                angles[i] = (Mathf.Rad2Deg * ((float)radians[i]) * axisMultiplier[i]) + axisOffset[i];
            }

            logAngles(angles);

            roboticArm.setAxisAngles(angles);
            roboticArm.updateAxisAngles();

        }

        /// <summary>
        /// Logs all angles received in a human readable format
        /// </summary>
        /// <param name="angles">Angles to log</param>
        private void logAngles(float[] angles)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Received Angles: [");
            sb.Append(angles[0]);

            for (int i = 1; i < angles.Length; i++)
            {
                sb.Append(", ");
                sb.Append(angles[i]);

            }

            sb.Append("]");
        }
#else
        void Start()
        {
            Debug.LogError("ROS Connector not found. Please install the ROS Connector module in your project to use this driver.");
        }
        
        public void updateAxisData(RoboticAxis[] axes)
        {
            throw new NotImplementedException();
        }
#endif

        
    }
}