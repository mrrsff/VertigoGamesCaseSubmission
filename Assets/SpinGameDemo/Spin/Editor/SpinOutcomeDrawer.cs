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

            Rect iconRect = new Rect(position.x, position.y, position.width * 0.6f, position.height);
            Rect amountRect = new Rect(position.x + position.width * 0.6f + 5, position.y, position.width * 0.4f - 5, position.height);
            Rect amountLabelRect = new Rect(amountRect.x, amountRect.y, 50, amountRect.height);
            Rect amountFieldRect = new Rect(amountRect.x + 50, amountRect.y, amountRect.width - 50, amountRect.height);
            
            EditorGUI.PropertyField(iconRect, property.FindPropertyRelative("Icon"), GUIContent.none);
            EditorGUI.LabelField(amountLabelRect, "Amount:");
            EditorGUI.PropertyField(amountFieldRect, property.FindPropertyRelative("Amount"), GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}