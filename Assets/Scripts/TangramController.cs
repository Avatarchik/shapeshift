public class TangramController
{
	private InputManager drag = new InputManager();
	private SilhouetteOverlap silhouette = new SilhouetteOverlap();
	public Model model;

	public void Start()
	{
		drag.snapSize = 0.05f;
		model = new Model();
	}

	public void Update()
	{
		if (silhouette.IsPerfect())
		{
			model.OverlapSilhouette();
		}
		drag.isEnabled = model.isDragEnabled;
		drag.Update();
		model.isItemSelected = drag.isItemSelected;
		if (null != drag.draggedObject && 0.0f != model.rotateDegrees) {
			ViewUtil.Rotate(drag.draggedObject, model.rotateDegrees);
		}
	}
}

