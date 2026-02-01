using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.Game.Spin.Editor
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OutcomeEntry))]
    public class OutcomeEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty outcomeProp = property.FindPropertyRelative("outcome");
            SerializedProperty weightProp = property.FindPropertyRelative("weight");

            float outcomeHeight = EditorGUI.GetPropertyHeight(outcomeProp);
            float weightWidth = 80f;
            Rect outcomeRect = new Rect(position.x, position.y, position.width - weightWidth - 5, outcomeHeight);
            
            // Row 1, Right side: The Weight
            Rect weightRect = new Rect(position.xMax - weightWidth, position.y, weightWidth, EditorGUIUtility.singleLineHeight);

            // Draw fields
            EditorGUI.PropertyField(outcomeRect, outcomeProp, GUIContent.none, true);
            
            float prevLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50f;
            EditorGUI.PropertyField(weightRect, weightProp, new GUIContent("W:"));
            EditorGUIUtility.labelWidth = prevLabelWidth;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("outcome"));
        }
    }
    #endif
}