// Copied from Damian at http://unity.grogansoft.com/drag-and-drop/
// Features Ethan added:
// * Is enable
// * Set layer mask
// * Snap position
// * Is scale optional
// * Is item selected
// * Last dragged item will be selected in front next time
// * Optionally keep disabled for a while to nullify old click
using UnityEngine;
using System.Collections;

public class InputManager
{
	public GameObject draggedObject;
	public bool isEnabled = false;
	public bool isScale = false;
	public bool isMouseEnabled = false;
	public bool isItemSelected = false;
	public bool isVerbose = false;
	public float snapSize = 0.0f;
	public float disableTime = 0.0f;
	private float time = 1.0f;
	private bool isDragging = false;
	private float z = -99.0f;
	private Vector2 touchOffset;
	RaycastHit2D hit;
	public int layerMask = Physics2D.DefaultRaycastLayers;
   
	public void SetLayerMask(string[] names)
	{
		layerMask = LayerMask.GetMask(names);
	}

	// Does not fully enable until next new mouse down and time passes.
	public void SetEnabled(bool isEnabledNow)
	{
		if (isEnabledNow != isEnabled)
		{
			isEnabled = isEnabledNow;
			draggedObject = null;
			isDragging = false;
			isMouseEnabled = false;
			time = 0.0f;
			if (isVerbose) {
				Debug.Log("InputManager.SetEnabled: " + isEnabled);
			}
		}
	}

	public void Update()
	{
		time += Time.deltaTime;
		if (!isEnabled || time < disableTime)
		{
			return;
		}
		else if (HasInput())
		{
			DragOrPickUp();
		}
		else
		{
			if (isDragging)
				DropItem();
		}
	}
	 
	private Vector2 CurrentTouchPosition()
	{
		Vector2 inputPos;
		inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return inputPos;
	}
 
	private void DragOrPickUp()
	{
		Vector2 inputPosition = CurrentTouchPosition();
	 
		if (isDragging)
		{
			Vector2 position = inputPosition + touchOffset;
			position = ViewUtil.SnapXY(position, snapSize);
			draggedObject.transform.position = position;
		}
		else
		{
			hit = Physics2D.Raycast(inputPosition, inputPosition, 0.5f, layerMask);
			if (hit)
			{
				if (hit.transform != null && hit.collider != null)
				{
					isDragging = true;
					isItemSelected = true;
					draggedObject = hit.transform.gameObject;
					z -= 1.0f;
					Vector3 position = draggedObject.transform.position;
					position.z = z;
					draggedObject.transform.position = position;
					touchOffset = (Vector2) hit.transform.position - inputPosition;
					if (isScale)
					{
						draggedObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
					}
					if (isVerbose) {
						Debug.Log("InputManager.PickUp: " + inputPosition + draggedObject.transform.position);
					}
				}
			}
		}
	}
 
	// returns true if either the mouse button is down or at least one touch is felt on the screen
	private bool HasInput()
	{
		if (isMouseEnabled) {
			return Input.GetMouseButton(0);
		}
		else if (Input.GetMouseButtonDown(0)) {
			isMouseEnabled = true;
		}
		return false;
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
