using EditorTools.Attributes;
using EditorTools.Inspector;
using Robotics.Driver.IK.Numeric;

namespace Robotics.Editor.Driver
{
    /// <summary>
    /// Dummy class to use custom property inspector
    /// </summary>
    [CustomEditorInfo(typeof(NewtonRaphsonDriver))]
    public class NewtonRaphsonDriverInspector: ExtendedBehaviourInspector
    {
        
    }
}