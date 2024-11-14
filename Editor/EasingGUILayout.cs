using UnityEditor;
using UnityEngine;

namespace LRT.Easing.Editor
{
	public static class EasingGUILayout
	{
		private static readonly int foldedHeight = 30;
		private static readonly int maxWidth = 300;
		
		private static bool folded = true;

		public static Ease Ease(Ease selectedCurve) => Ease("", selectedCurve);

		public static Ease Ease(string label, Ease selectedCurve)
		{
			// Display enum popup
			Ease curve = (Ease)EditorGUILayout.EnumPopup(label, selectedCurve);

			// Calculate width and height and reserve space
			float width = Mathf.Min(maxWidth, Screen.width * 0.785f); //thank you unity...
			float height = folded ? foldedHeight : width;
			Rect easeRect = EditorGUILayout.GetControlRect(false, height);
			easeRect.width = width;
			easeRect.height = height;

			// Make the area clickable
			if (GUI.Button(easeRect, "", GUIStyle.none))
				folded = !folded;

			// Change cursor on hover
			EditorGUIUtility.AddCursorRect(easeRect, folded ? MouseCursor.ArrowPlus : MouseCursor.ArrowMinus);

			// Draw rect and curve
			EditorGUI.DrawRect(easeRect, Color.black);
			DrawCurve(easeRect, curve);

			return curve;
		}

		private static void DrawCurve(Rect graphRect, Ease curve)
		{
			Color originalColor = Handles.color;

			float startX = graphRect.x;
			float startY = graphRect.y + graphRect.height;
			Vector2 previousPoint = new Vector2(startX, startY);

			for (float i = 0f; i <= 1f; i += 0.01f)
			{
				float ease = curve.Evaluate(i);
				Vector2 point = new Vector2(i, ease);
				Vector2 currentPoint = new Vector2(startX + (point.x * graphRect.width), startY - (point.y * graphRect.height));

				Handles.color = Color.Lerp(Color.green, Color.cyan, i);

				Handles.DrawLine(previousPoint, currentPoint);

				previousPoint = currentPoint;
			}

			Handles.color = originalColor;
		}
	}
}


