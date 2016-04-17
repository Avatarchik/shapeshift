// Copied from Damian at http://unity.grogansoft.com/drag-and-drop/
// Features Ethan added:
// * Is enable
// * Set layer mask
// * Snap position
// * Is scale optional
// * Is item selected
using UnityEngine;
using System.Collections;

public class InputManager
{
	public GameObject draggedObject;
	public bool isEnabled = false;
	public bool isScale = false;
	public bool isItemSelected = false;
	public float snapSize = 0.0f;
	private bool isDragging = false;
	private Vector2 touchOffset;
	RaycastHit2D[] touches = new RaycastHit2D[1];
	public int layerMask = Physics2D.DefaultRaycastLayers;
   
	public void SetLayerMask(string[] names)
	{
		layerMask = LayerMask.GetMask(names);
	}

	public void SetEnabled(bool isEnabled)
	{
		if (isEnabled != this.isEnabled)
		{
			this.isEnabled = isEnabled;
			if (!isEnabled) {
				draggedObject = null;
				isDragging = false;
				touches[0] = new RaycastHit2D();
			}
		}
	}

	public void Update()
	{
		if (!isEnabled)
		{
			return;
		}
		if (HasInput)
		{
			DragOrPickUp();
		}
		else
		{
			if (isDragging)
				DropItem();
		}
	}
	 
	Vector2 CurrentTouchPosition
	{
		get
		{
			Vector2 inputPos;
			inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			return inputPos;
		}
	}
 
	private void DragOrPickUp()
	{
		Vector2 inputPosition = CurrentTouchPosition;
	 
		if (isDragging)
		{
			Vector2 position = inputPosition + touchOffset;
			position = ViewUtil.SnapXY(position, snapSize);
			draggedObject.transform.position = position;
		}
		else
		{
			touches[0] = new RaycastHit2D();
			Physics2D.RaycastNonAlloc(inputPosition, inputPosition, touches, 0.5f, layerMask);
			if (touches.Length > 0)
			{
				RaycastHit2D hit = touches[0];
				if (hit && hit.transform != null && hit.collider != null)
				{
					isDragging = true;
					isItemSelected = true;
					draggedObject = hit.transform.gameObject;
					touchOffset = (Vector2)hit.transform.position - inputPosition;
					if (isScale)
					{
						draggedObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
					}
				}
			}
		}
	}
 
	private bool HasInput
	{
		get
		{
			// returns true if either the mouse button is down or at least one touch is felt on the screen
			return Input.GetMouseButton(0);
		}
	}
 
	void DropItem()
	{
		isDragging = false;
		if (isScale)
		{
			draggedObject.transform.localScale = new Vector3(1f,1f,1f);
		}
	}
}
