using System;
using System.Collections;
using Robotics.Abstract;
using Robotics.Structs;
using EditorTools.Attributes;
using EditorTools.BaseTypes;
using Robotics.Interfaces;
using UnityEngine;

namespace Robotics.Controller.RoboticArm
{
    /// <summary>
    /// Class implementing ''Firmware'' for robotic arms
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class RoboticArmControler : ExtendedMonoBehaviour, IRoboticArm
    {
        [Tooltip("Name of this Robot")]
        public string robotName = "RoboticArm";
        
        [Header("Accessories")]
        [Tooltip("Base platform the robotic arm should be mounted upon")]
        public AbstractRoboticBase roboticBase;

        [Tooltip("Index of the mounting point to use for this robot")]
        public int baseMountingPointIndex = 0;
        
        [NonReorderable]
        [NonResizeable]
        [Tooltip("Descriptions of the axes of the robotic arm")]
        public RoboticAxis[] axes = new RoboticAxis[6];
        
        #region UnityEvents
        
        /// <summary>
        /// Called when a value is changed in custom property inspector
        /// </summary>
        /// <param name="origin">name of origin field of change</param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public override void OnInspectorValueChanged(string origin, object oldValue, object newValue)
        {
            gameObject.SetActive(true);
            StartCoroutine(updateObjectFromInspector());
        }
        
        #endregion

        #region CoRoutines

        /// <summary>
        /// Coroutine for updating robot mesh
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateObjectFromInspector()
        {
            yield return new WaitForSecondsRealtime(.05f);
            
            setupRelations();
            updateAxisAngles();

            IRoboticDriver[] drivers = gameObject.GetComponents<IRoboticDriver>();

            foreach (IRoboticDriver driver in drivers)
            {
                driver.updateAxisData(axes);
            }
            
        }

        
        #endregion

        #region Movement

        /// <summary>
        /// Updates all the axis angles of this robotic arm
        /// </summary>
        public void updateAxisAngles()
        {
            for (int i = 0; i < axes.Length; i++)
            {
                if (axes[i] == null || axes[i].gameObject == null)
                {
                    Debug.LogWarning("Link " + i + " of Robotic Arm " + name + " is Null !");
                    continue;
                }

                axes[i].gameObject.transform.localRotation = Quaternion.AngleAxis(
                    //Math.Min(Math.Max(axes[i].currentAngle, axes[i].minimumAngle), axes[i].maximumAngle),
                    -axes[i].currentAngle,
                    axes[i].direction);
            }
        }

        /// <summary>
        /// Sets the angles for all axes of the robot
        /// </summary>
        /// <param name="axes">float[] containing all values for all axes to set</param>
        public void setAxisAngles(float[] axes)
        {
            for (int i = 0; i < Math.Min(this.axes.Length, axes.Length); i++)
            {
                this.axes[i].currentAngle = axes[i];
            }
        }

        /// <summary>
        /// gets all axis definitions of the robotic arm
        /// </summary>
        /// <returns></returns>
        public RoboticAxis[] getAxes()
        {
            return this.axes;
        }

        #endregion
        
        #region IRoboticArm Overrides
        
        /// <summary>
        /// Returns the base this robotic arm is mounted on
        /// </summary>
        /// <returns> the base this robotic arm is mounted on</returns>
        // Implements IRoboticArm.getBase()
        public IRoboticBase getBase()
        {
            return roboticBase;
        }

        #endregion
        
        #region Setup
        /// <summary>
        /// Setup all object relations needed for rigging the robotic arm
        /// </summary>
        private void setupRelations()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            Transform rootTransform = gameObject.transform;
            if (roboticBase != null)
            {
                
                //setParent(gameObject.transform, roboticBase.gameObject.transform);
                rootTransform = roboticBase.getMountingPoint(baseMountingPointIndex);
            }

            for (int i = 0; i < axes.Length; i++)
            {
                if (axes[i] != null && axes[i].gameObject != null)
                {
                    setParent(rootTransform, axes[i].gameObject.transform);
                    axes[i].gameObject.transform.localPosition = axes[i].position;
                
                    rootTransform = axes[i].gameObject.transform;
                }
            }
        }

        #endregion
        
        #region Tools
        
        /// <summary>
        /// Coroutine for assigning Parent/Child relationships. Workaround for <b>SendMessage cannot be called during
        /// Awake, CheckConsistency, or OnValidate</b> compiler warning.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        private void setParent(Transform parent, Transform child)
        {
            child.SetParent(parent);
            //yield return null;
        }
        
        #endregion
        
    }
}