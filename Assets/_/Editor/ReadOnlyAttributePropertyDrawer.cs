using UnityEditor;
using UnityEngine;

namespace Prototype.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attribute = this.attribute as ReadOnlyAttribute;
            GUI.enabled = !(Application.isPlaying ? attribute.duringPlayMode : attribute.duringEditMode);
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}