using UnityEngine;

// Grid cell length equal to quarter of unit triangle's width.
// All silhouettes overlapped:  True.
//
// For each point in each silhouette collider's bounding box:
// 	If silhouette colliders overlap point:
// 		If no piece collider overlaps that point:
// 			Return all silhouettes are not overlapped.
// For each point in each piece collider's bounding box:
// 	If a piece collider overlaps that point:
// 		If no silhouette collider overlaps point:
// 			Return all silhouettes are not overlapped.
// 
// To visualize, place small red square at each point not overlapping on a sorting layer in front of moving pieces.
// 
// This depends on collider polygons being more precise than the snapping grid and silhouette oversizing.
// 
// Would also want to only enable drag and drop of pieces layer.
// Silhouettes placed on IgnoreRaycast layer.
// 
// file:///C:/Program%20Files/Unity/Editor/Data/Documentation/en/ScriptReference/Physics2D.OverlapPointNonAlloc.html
public class SilhouetteOverlap
{
	public float step = 0.25f;
	public float half;
	public Vector2 point = new Vector2();
	private Collider2D[] silhouettes;
	private Collider2D[] pieces;

	private bool isBothOverlap;
	private int a;
	private int b;
	private int aLength;
	private int bLength;
	private float x;
	private float y;
	private Collider2D colliderA;
	private Collider2D colliderB;
	private Vector3 min;
	private Vector3 max;

	public bool IsSetup()
	{
		return null != silhouettes;
	}

	public void Setup(GameObject[] silhouetteObjects, GameObject[] pieceObjects)
	{
		half = 0.5f * step;
		int index;
		silhouettes = new Collider2D[silhouetteObjects.Length];
		for (index = 0; index < silhouetteObjects.Length; index++) {
			silhouettes[index] = silhouetteObjects[index].GetComponent<Collider2D>();
		}
		pieces = new Collider2D[pieceObjects.Length];
		for (index = 0; index < pieceObjects.Length; index++) {
			pieces[index] = pieceObjects[index].GetComponent<Collider2D>();
		}
	}

	// For each point in each A collider's bounding box:
	// 	If A colliders overlap point:
	// 		If no B collider overlaps that point:
	// 			Return all A are not overlapped.
	// Variables are members to avoid construction overhead.
	// Offset half a step.
	private bool BothOverlap(Collider2D[] A, Collider2D[] B)
	{
		isBothOverlap = true;
		aLength = A.Length;
		bLength = B.Length;
		for (a = 0; a < aLength; a++) {
			colliderA = A[a];
			min = colliderA.bounds.min;
			max = colliderA.bounds.max;
			for (x = min.x + half; x <= max.x - half; x += step) {
				point.x = x;
				for (y = min.y + half; y <= max.y - half; y += step) {
					point.y = y;
					if (colliderA.OverlapPoint(point)) {
						isBothOverlap = false;
						for (b = 0; b < bLength; b++) {
							colliderB = B[b];
							if (colliderB.OverlapPoint(point)) {
								isBothOverlap = true;
								break;
							}
						}
						if (!isBothOverlap) {
							return isBothOverlap;
						}
					}
				}
			}
		}
		return isBothOverlap;
	}

	public bool IsPerfect()
	{
		return BothOverlap(silhouettes, pieces)
			&& BothOverlap(pieces, silhouettes);
	}
}
