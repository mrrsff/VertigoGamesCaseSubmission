using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.Spin.Editor
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OutcomeEntry))]
    public class OutcomeEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // [Outcome] Weight:[weight]
            Rect outcomeRect = new Rect(position.x, position.y, position.width * 0.7f, position.height);
            Rect weightRect = new Rect(position.x + position.width * 0.7f + 5, position.y, position.width * 0.3f - 5, position.height);
            Rect weightLabelRect = new Rect(weightRect.x, weightRect.y, 50, weightRect.height);
            Rect weightFieldRect = new Rect(weightRect.x + 50, weightRect.y, weightRect.width - 50, weightRect.height);

            // Draw fields
            EditorGUI.PropertyField(outcomeRect, property.FindPropertyRelative("outcome"), GUIContent.none);
            EditorGUI.LabelField(weightLabelRect, "Weight:");
            EditorGUI.PropertyField(weightFieldRect, property.FindPropertyRelative("weight"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
    #endif
}