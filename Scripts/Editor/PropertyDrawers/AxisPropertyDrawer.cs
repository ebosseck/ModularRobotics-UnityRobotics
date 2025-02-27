using EditorTools.InspectorTools;
using Robotics.Structs;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Robotics.Editor.PropertyDrawers
{
    /// <summary>
    /// Custom property drawer for axis angles
    /// </summary>
    [CustomPropertyDrawer(typeof(RoboticAxis))]
    public class AxisPropertyDrawer: PropertyDrawer
    {
        /// <summary>
        /// Creates a gui for a given property
        /// </summary>
        /// <param name="property">Property to draw GUI for</param>
        /// <returns>root element for this property's inspector GUI</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement inspector = new VisualElement();

            Foldout foldout =
                GUIElemetCreator.createFoldout("Axis " + (SerializedPropertyTools.getArrayIndex(property) + 1));
            
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("position")));
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("direction")));
            
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("minimumAngle")));
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("maximumAngle")));
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("maximumAngularVelocity")));
            
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("gameObject")));
            GUIElementTools.addAllToElement(foldout, GUIElemetCreator.createFields(typeof(RoboticAxis), property.FindPropertyRelative("currentAngle")));
            
            inspector.Add(foldout);
            
            return inspector;
        }
    }
}