using UnityEngine;

// Grid cell length equal to quarter of unit triangle's width.
// All silhouettes overlapped:  True.
// For each point in overlap grid:
// 	If silhouette colliders overlap point:
// 		If a piece collider does not overlap that point:
// 			Return all silhouettes are not overlapped.
// 	Else:
// 		If a piece collider overlaps that point:
// 			Return all silhouettes are not overlapped.
// 
// To visualize, place small red square at each point not overlapping on a sorting layer in front of moving pieces.
// 
// This depends on collider polygons being more precise than the snapping grid and silhouette oversizing.
// 
// Would also want to only enable drag and drop of pieces layer.
// 
// file:///C:/Program%20Files/Unity/Editor/Data/Documentation/en/ScriptReference/Physics2D.OverlapPointNonAlloc.html
public class SilhouetteOverlap
{
	public float stepSize = 0.25f;
	public GameObject[] silhouettes;
	public GameObject[] pieces;

	public bool IsPerfect()
	{
		return false;
	}
}
