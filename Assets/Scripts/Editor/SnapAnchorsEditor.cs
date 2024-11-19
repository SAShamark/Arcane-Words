using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class SnapAnchorsEditor : UnityEditor.Editor
	{
		[MenuItem("GameObject/Snap Anchors/in this and it's children", false, 0)]
		static void SweepingSnapAnchorsStatic()
		{
			Debug.Log("Snapping anchors of ''" + Selection.activeTransform.gameObject.name + "'' and its children.");

			StaticSweepingSnapAnchors(Selection.activeGameObject);
		}

		[MenuItem("GameObject/Snap Anchors/in this", false, 0)]
		static void SnapAnchorsStatic()
		{
			Debug.Log("Snapping anchors of ''" + Selection.activeTransform.gameObject.name + ".");
			StaticSnapAnchors(Selection.activeGameObject);
		}
		

		public static void StaticSnapAnchors(GameObject gameObject){

			RectTransform rectTransform = null;
			RectTransform parentTransform = null;
	
			if (gameObject.transform.parent != null) {
				if (gameObject.gameObject.tag != "IgnoreSnapAnchors") {
					if (gameObject.GetComponent<RectTransform> () != null)
						rectTransform = gameObject.GetComponent<RectTransform> ();
					else {
						Debug.LogError (gameObject.name + " Doesn't have RectTransform. SnapAnchors must be used only with UI objects. Please select a objet with RectTransform. Returning function.");
						return;
					}

					if (parentTransform == null) {
						parentTransform = gameObject.transform.parent.GetComponent<RectTransform> ();
					}
					Undo.RecordObject (rectTransform,"Snap Anchors");

					Vector2 offsetMin = rectTransform.offsetMin;
					Vector2 offsetMax = rectTransform.offsetMax;

					Vector2 anchorMin = rectTransform.anchorMin;
					Vector2 anchorMax = rectTransform.anchorMax;

					var parentScale = new Vector2 (parentTransform.rect.width, parentTransform.rect.height);


					rectTransform.anchorMin = new Vector2 (
						anchorMin.x + (offsetMin.x / parentScale.x),
						anchorMin.y + (offsetMin.y / parentScale.y));

					rectTransform.anchorMax = new Vector2 (
						anchorMax.x + (offsetMax.x / parentScale.x),
						anchorMax.y + (offsetMax.y / parentScale.y));

					rectTransform.offsetMin = Vector2.zero;
					rectTransform.offsetMax = Vector2.zero;
				}
			}

		}
		public static void StaticSweepingSnapAnchors(GameObject gameObject)
		{
			StaticSnapAnchors (gameObject);
			for (var i = 0; i < gameObject.transform.childCount; i++)
			{
				StaticSweepingSnapAnchors(gameObject.transform.GetChild(i).gameObject);
			}   
		}
	}
}
