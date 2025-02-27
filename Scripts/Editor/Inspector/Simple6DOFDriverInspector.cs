using EditorTools.Attributes;
using EditorTools.Inspector;
using Robotics.Driver;
using UnityEditor;

namespace Robotics.Editor
{
    /// <summary>
    /// Dummy class to use custom property inspector
    /// </summary>
    [CustomEditorInfo(typeof(Simple6DOFDriver))]
    public class Simple6DOFDriverInspector : ExtendedBehaviourInspector
    {

    }
}