using EditorTools.Attributes;
using EditorTools.Inspector;
using Robotics.Controller.RoboticArm;


namespace Robotics.Editor
{
    /// <summary>
    /// Dummy class to use custom property inspector
    /// </summary>
    [CustomEditorInfo(typeof(RoboticArmControler))]
    public class RoboticArmControlerInspector: ExtendedBehaviourInspector
    {

    }
}
