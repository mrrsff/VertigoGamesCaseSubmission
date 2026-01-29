using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.Spin.Editor
{
    [CustomPropertyDrawer(typeof(SpinOutcome))]
    public class SpinOutcomeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float iconWidth = position.width * 0.5f;
            float amountWidth = position.width * 0.3f;
            float scalingWidth = position.width * 0.2f - 10;
            
            Rect iconRect = new Rect(position.x, position.y, iconWidth, position.height);
            Rect amountLabelRect = new Rect(position.x + iconWidth + 5, position.y, 50, position.height);
            Rect amountFieldRect = new Rect(position.x + iconWidth + 55, position.y, amountWidth - 55, position.height);
            Rect scalingLabelRect = new Rect(position.x + iconWidth + amountWidth + 10, position.y, 40, position.height);
            Rect scalingRect = new Rect(position.x + iconWidth + amountWidth + 50, position.y, scalingWidth, position.height);
            
            EditorGUI.PropertyField(iconRect, property.FindPropertyRelative("Icon"), GUIContent.none);
            EditorGUI.LabelField(amountLabelRect, "Amount:");
            EditorGUI.PropertyField(amountFieldRect, property.FindPropertyRelative("Amount"), GUIContent.none);
            EditorGUI.LabelField(scalingLabelRect, "Scale:");
            EditorGUI.PropertyField(scalingRect, property.FindPropertyRelative("AllowScaling"), GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}