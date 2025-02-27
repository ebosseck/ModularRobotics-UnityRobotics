using System;
using System.Collections;
using EditorTools.Attributes;
using EditorTools.MeshTools;
using Robotics.Abstract;
using Robotics.Structs;
using UnityEngine;


namespace Robotics.Controller.RoboticBase
{
    /// <summary>
    /// Class implementing ''Firmware'' for linear accelerator bases
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LinearAcceleratorControler: AbstractRoboticBase
    {
        [Header("Linear Accelerator")] 
        [Tooltip("Rail Length")]
        [Range(1, 30)]
        [Unit("m")]
        public int railLength = 1;

        [Tooltip("Zero-Position of the sled on the rails")]
        [Unit("m")]
        public Vector3 minimalPosition = Vector3.zero;

        [Tooltip("Lenght the Sled can be moved along")]
        [Unit("m")]
        public float moveableLength = 0;
        
        [Tooltip("Current Position of the sleigh on the rails")]
        [Range(0, 1)]
        public float currentPosition;

        [Tooltip("Axis along which the sleigh moves")]
        public Vector3 movementAxis;
        
        [Tooltip("Maximum Velocity of the Linear Accelerator")]
        [Unit("m/s")]
        public float maximumVelocity;

        [Header("Rendering")] 
        [Tooltip("GameObject used as Template for the Rail System")] 
        public Mesh railTemplate_midsection;
        public Mesh railTemplate_end_1;
        public Mesh railTemplate_end_2;
        public Vector3 relativeArrayDirection = Vector3.right;
        
        [Tooltip("GameObject used as Sleigh")]
        public GameObject sleigh;

        [Range(0, 1)]
        private float targetPosition = 0;
        
        #region UnityEngine Events
        
        // Update is called once per frame
        void Update()
        {
            updateLinearAccelerator();
        }
        
        /// <summary>
        /// Called when a value form the custom property inspector is changed
        /// </summary>
        /// <param name="origin">name of originating field</param>
        /// <param name="oldValue">old value of field</param>
        /// <param name="newValue">new value for field</param>
        public override void OnInspectorValueChanged(string origin, object oldValue, object newValue)
        {
            gameObject.SetActive(true);
            StartCoroutine(updateObjectFromInspector());
        }
        
        #endregion

        #region CoRoutines

        /// <summary>
        /// CoRoutine updating this object
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateObjectFromInspector()
        {
            yield return new WaitForSecondsRealtime(.05f);
            buildMesh();
            setupRelations();
            
            movementAxis.Normalize();
            float internalMoveableLength = moveableLength / 50; 
            sleigh.transform.localPosition = minimalPosition + (this.currentPosition * internalMoveableLength * this.movementAxis);
        }

        #endregion
        
        #region Movement
        /// <summary>
        /// Updates the movement of this linear accelerator
        /// </summary>
        public void updateLinearAccelerator()
        {
            float step = Math.Max(
                Math.Min(this.targetPosition - this.currentPosition, (this.maximumVelocity/moveableLength)), 
                -(this.maximumVelocity/moveableLength)) * Time.deltaTime;

            this.currentPosition += step;
            float internalMoveableLength = moveableLength / 50; 

            sleigh.transform.localPosition = minimalPosition + (this.currentPosition * internalMoveableLength * this.movementAxis);

        }

        /// <summary>
        /// Sets the target position in range 0..1
        /// </summary>
        /// <param name="position">Posiiton in range 0 .. 1</param>
        public void setPosition(float position)
        {
            this.targetPosition = Math.Max(Math.Min(position, 1), 0);
        }

        #endregion
        
        #region Setup
        /// <summary>
        /// Setup object relations required for rigging
        /// </summary>
        private void setupRelations()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (this.sleigh != null)
            {
                if (this.sleigh.transform.parent != gameObject.transform)
                {
                    setParent(gameObject.transform, this.sleigh.transform); // CoRoutine as workaround against compiler warning
                }
            }

            foreach (Transform robotMountingPoint in this.robotMountingPoints)
            {
                if (robotMountingPoint.parent != gameObject.transform)
                {
                    setParent(this.sleigh.transform, robotMountingPoint); // CoRoutine as workaround against compiler warning
                }
            }
        }

        /// <summary>
        /// Builds the mesh for the linear accellerator rails
        /// </summary>
        private void buildMesh()
        {
            Mesh result = MeshAssembler.assembleArrayMesh(this.railTemplate_end_1, this.railTemplate_midsection,
                this.railTemplate_end_2, this.railLength, this.relativeArrayDirection);

            result.name = "LinearAcceleratorRailMesh";
            result.Optimize();
            setupMeshRendering(result);

        }


        /// <summary>
        /// Coroutine for assigning Parent/Child relationships. Workaround for <b>SendMessage cannot be called during
        /// Awake, CheckConsistency, or OnValidate</b> compiler warning.
        /// </summary>
        /// <returns></returns>
        private void setupMeshRendering(Mesh mesh)
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            if (filter == null)
            {
                filter = gameObject.AddComponent< MeshFilter >();
            }
            filter.mesh = mesh;

            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                renderer = gameObject.AddComponent< MeshRenderer >();
            }
        }

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
        
        #region Overrides from AbstractRoboticsBase
        /// <summary>
        /// returns the type of robotic base
        /// </summary>
        /// <returns>LINEAR_ACCELLERATOR</returns>
        // Overrides AbstractRoboticBase.getBaseType()
        public override RoboticBaseType getBaseType()
        {
            return RoboticBaseType.LINEAR_ACCELLERATOR;
        }
        
        #endregion
    }
}