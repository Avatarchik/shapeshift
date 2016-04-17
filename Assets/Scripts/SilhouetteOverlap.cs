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
	// A sharp corner smaller than this might be overlooked.
	// Smaller step size performs more computations.
	public float step = 0.0625f;
			// 0.125f;
			// 0.25f;
			// 0.5f;
	public float margin;
	public Vector2 point = new Vector2();
	public bool isPerfect;
	private Collider2D[] silhouettes;
	private Collider2D[] pieces;

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

	public void Clear()
	{
		silhouettes = null;
		pieces = null;
	}

	public bool IsSetup()
	{
		return null != silhouettes;
	}

	public void Setup(GameObject[] silhouetteObjects, GameObject[] pieceObjects)
	{
		margin = 
			0.0f;
			// 0.25f * step;
			// 0.5f * step;
			// step;
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
	// Inset by margin.
	private bool IsAllOverlap(Collider2D[] A, Collider2D[] B)
	{
		bool isAllOverlap;
		isAllOverlap = true;
		aLength = A.Length;
		bLength = B.Length;
		for (a = 0; a < aLength; a++) {
			colliderA = A[a];
			min = colliderA.bounds.min;
			max = colliderA.bounds.max;
			for (x = min.x + margin; x <= max.x - margin; x += step) {
				point.x = x;
				for (y = min.y + margin; y <= max.y - margin; y += step) {
					point.y = y;
					if (colliderA.OverlapPoint(point)) {
						isAllOverlap = false;
						for (b = 0; b < bLength; b++) {
							colliderB = B[b];
							if (colliderB != colliderA && colliderB.OverlapPoint(point)) {
								isAllOverlap = true;
								break;
							}
						}
						if (!isAllOverlap) {
							return isAllOverlap;
						}
					}
				}
			}
		}
		return isAllOverlap;
	}

	// For each point in each A collider's bounding box:
	// 	If A colliders overlap point:
	// 		If no B collider overlaps that point:
	// 			Return all A are not overlapped.
	// Variables are members to avoid construction overhead.
	// Inset by margin.
	private bool IsAnyOverlap(Collider2D[] A, Collider2D[] B)
	{
		bool isAnyOverlap = false;
		aLength = A.Length;
		bLength = B.Length;
		for (a = 0; a < aLength; a++) {
			colliderA = A[a];
			min = colliderA.bounds.min;
			max = colliderA.bounds.max;
			for (x = min.x + margin; x <= max.x - margin; x += step) {
				point.x = x;
				for (y = min.y + margin; y <= max.y - margin; y += step) {
					point.y = y;
					if (colliderA.OverlapPoint(point)) {
						for (b = 0; b < bLength; b++) {
							colliderB = B[b];
							if (colliderB != colliderA && colliderB.OverlapPoint(point)) {
								isAnyOverlap = true;
								return isAnyOverlap;
							}
						}
					}
				}
			}
		}
		return isAnyOverlap;
	}

	public bool Update()
	{
		isPerfect = IsAllOverlap(pieces, silhouettes)
			&& !IsAnyOverlap(pieces, pieces);
		return isPerfect;
	}
}
