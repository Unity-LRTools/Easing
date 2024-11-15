using UnityEditor;
using UnityEngine;

namespace LRT.Easing.Editor
{
	[CustomPropertyDrawer(typeof(EasePreviewAttribute))]
	public class EasePreviewDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label);
			EaseGUILayout.DrawPreview((Ease)property.enumValueIndex);
		}
	}
}