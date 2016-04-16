public class TangramController
{
	private InputManager drag = new InputManager();
	public Model model;

	public void Start()
	{
		model = new Model();
	}

	public void Update()
	{
		drag.isEnabled = model.isDragEnabled;
		drag.Update();
		model.isItemSelected = drag.isItemSelected;
		if (null != drag.draggedObject && 0.0f != model.rotateDegrees) {
			ViewUtil.Rotate(drag.draggedObject, model.rotateDegrees);
		}
	}
}

