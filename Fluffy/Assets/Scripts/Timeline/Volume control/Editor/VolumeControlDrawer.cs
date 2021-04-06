using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    [CustomPropertyDrawer(typeof(VolumeControlBehavior))]
    public class LightDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 1;
            return fieldCount * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty intensityProp = property.FindPropertyRelative("volume");

            Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(singleFieldRect, intensityProp);
        }
    }
}
