using UnityEngine;  // GameObject data type

public class TangramController
{
	public View view;
	public SilhouetteOverlap silhouette = new SilhouetteOverlap();
	public Model model = new Model();
	private InputManager drag = new InputManager();

	public void Start()
	{
		drag.snapSize = 0.05f;
	}

	private void UpdateSilhouette()
	{
		if (!silhouette.IsSetup()) {
			GameObject silhouetteParent = view.graph["Levels"].children[model.levelParent].children[model.silhouetteParent].self;
			GameObject piecesParent = view.graph["Levels"].children[model.levelParent].children[model.piecesParent].self;
			silhouette.Setup(
				ViewUtil.GetChildren(silhouetteParent),
				ViewUtil.GetChildren(piecesParent)
			);
		}
		if (silhouette.Update())
		{
			model.OverlapSilhouette();
		}
	}

	private void UpdateDrag()
	{
		drag.isEnabled = model.isDragEnabled;
		drag.Update();
		model.isItemSelected = drag.isItemSelected;
		if (null != drag.draggedObject && 0.0f != model.rotateDegrees) {
			ViewUtil.Rotate(drag.draggedObject, model.rotateDegrees);
		}
	}

	public void Update()
	{
		UpdateSilhouette();
		UpdateDrag();
	}
}

