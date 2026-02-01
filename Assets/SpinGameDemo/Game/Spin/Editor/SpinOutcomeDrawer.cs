using UnityEditor;
using UnityEngine;

namespace SpinGameDemo.Game.Spin.Editor
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SpinOutcome))]
    public class SpinOutcomeDrawer : PropertyDrawer
    {
        private const float LineHeight = 18f;
        private const float Spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty isBombProp = property.FindPropertyRelative("IsBomb");
            SerializedProperty iconProp = property.FindPropertyRelative("Icon");
            SerializedProperty amountProp = property.FindPropertyRelative("Amount");
            SerializedProperty scaleProp = property.FindPropertyRelative("AllowScaling");

            bool isBomb = isBombProp.boolValue;

            Rect row1Rect = new Rect(position.x, position.y, position.width, LineHeight);
            
            float toggleWidth = 70f;
            Rect iconRect = new Rect(row1Rect.x, row1Rect.y, row1Rect.width - toggleWidth - 5, LineHeight);
            Rect bombLabelRect = new Rect(row1Rect.xMax - toggleWidth, row1Rect.y, 50, LineHeight);
            Rect bombToggleRect = new Rect(bombLabelRect.xMax, row1Rect.y, 20, LineHeight);

            EditorGUI.PropertyField(iconRect, iconProp, GUIContent.none);
            EditorGUI.LabelField(bombLabelRect, "Is Bomb");
            EditorGUI.PropertyField(bombToggleRect, isBombProp, GUIContent.none);

            if (!isBomb)
            {
                Rect row2Rect = new Rect(position.x, position.y + LineHeight + Spacing, position.width, LineHeight);
                
                float labelWidth = 35f;
                float gutter = 10f;
                float fieldWidth = (row2Rect.width / 2f) - labelWidth - gutter;

                Rect amtLabelRect = new Rect(row2Rect.x, row2Rect.y, labelWidth, LineHeight);
                Rect amtFieldRect = new Rect(amtLabelRect.xMax, row2Rect.y, fieldWidth, LineHeight);

                Rect scaleLabelRect = new Rect(amtFieldRect.xMax + gutter, row2Rect.y, 45f, LineHeight);
                Rect scaleFieldRect = new Rect(scaleLabelRect.xMax, row2Rect.y, fieldWidth, LineHeight);

                EditorGUI.LabelField(amtLabelRect, "Amt");
                EditorGUI.PropertyField(amtFieldRect, amountProp, GUIContent.none);

                EditorGUI.LabelField(scaleLabelRect, "Scale");
                EditorGUI.PropertyField(scaleFieldRect, scaleProp, GUIContent.none);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty isBombProp = property.FindPropertyRelative("IsBomb");
            
            if (isBombProp != null && isBombProp.boolValue)
            {
                return LineHeight;
            }

            return (LineHeight * 2) + Spacing;
        }
    }
    #endif
}