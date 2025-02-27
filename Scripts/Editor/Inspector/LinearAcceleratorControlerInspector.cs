using EditorTools.Attributes;
using EditorTools.Inspector;
using Robotics.Controller.RoboticBase;

namespace Robotics.Editor
{ 
        /// <summary>
        /// Dummy class to use custom property inspector
        /// </summary>
        [CustomEditorInfo(typeof(LinearAcceleratorControler))] 
        public class LinearAcceleratorControlerInspector: ExtendedBehaviourInspector
        {

        }

}