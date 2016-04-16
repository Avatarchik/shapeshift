// Copied from Damian at http://unity.grogansoft.com/drag-and-drop/
using UnityEngine;
using System.Collections;

public class InputManager
{
	public GameObject draggedObject;
	public bool isEnabled = false;
	public bool isScale = false;
	private bool draggingItem = false;
	private Vector2 touchOffset;
   
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
			if (draggingItem)
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
		var inputPosition = CurrentTouchPosition;
	 
		if (draggingItem)
		{
			draggedObject.transform.position = inputPosition + touchOffset;
		}
		else
		{
			RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
			if (touches.Length > 0)
			{
				var hit = touches[0];
				if (hit.transform != null)
				{
					draggingItem = true;
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
		draggingItem = false;
		if (isScale)
		{
			draggedObject.transform.localScale = new Vector3(1f,1f,1f);
		}
	}
}
