using LRT.Utility.Editor;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace LRT.Easing.Editor
{
	public static class EaseGUILayout
	{
		private static readonly int foldedHeight = 30;
		private static readonly int maxWidth = 300;

		private static bool folded = true;

		public static Ease Ease(Ease selectedCurve) => Ease("", selectedCurve, true);
		public static Ease Ease(string label, Ease selectedCurve) => Ease(label, selectedCurve, true);

		public static Ease Ease(string label, Ease selectedCurve, bool drawPreview)
		{
			// Display enum popup
			Ease curve = (Ease)EditorGUILayout.EnumPopup(label, selectedCurve);

			if (drawPreview)
				DrawPreview(curve);

			return curve;
		}

		public static void DrawPreview(Ease curve)
		{
			float width = Mathf.Min(maxWidth, Screen.width * 0.72f); //thank you unity...
			float height = folded ? foldedHeight : width;
			Rect previewRect = EditorGUILayout.GetControlRect(false, height + 1);

			// Calculate width and height and reserve space
			previewRect.width = width;
			previewRect.height = height;

			// Make the area clickable
			if (GUI.Button(previewRect, "", GUIStyle.none))
				folded = !folded;

			// Change cursor on hover
			EditorGUIUtility.AddCursorRect(previewRect, folded ? MouseCursor.ArrowPlus : MouseCursor.ArrowMinus);

			// Draw rect and curve
			EditorGUI.DrawRect(previewRect, Color.black);
			CustomGUIUtility.DrawCross(previewRect, Color.white * 0.5f, 2);
			DrawCurve(previewRect, curve);
			CustomGUIUtility.DrawBorder(previewRect, Color.white * 0.75f, 1);
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

				if (!folded || (graphRect.Contains(previousPoint) && graphRect.Contains(currentPoint)))
					Handles.DrawLine(previousPoint, currentPoint);

				previousPoint = currentPoint;
			}

			Handles.color = originalColor;
		}
	}
}


