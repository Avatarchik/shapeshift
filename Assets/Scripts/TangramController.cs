using UnityEngine;  // GameObject data type

public class TangramController
{
	public View view;
	public SilhouetteOverlap silhouette = new SilhouetteOverlap();
	public Model model = new Model();
	private InputManager drag = new InputManager();

	public void Start()
	{
		drag.snapSize = 0.1f;  // 0.05f;
		drag.SetLayerMask(model.piecesLayers);
	}

	private void UpdateSilhouette()
	{
		if (model.isMenu) {
			silhouette.Clear();
		}
		else if (!silhouette.IsSetup()) {
			GameObject silhouetteParent = view.graph[model.screenParent].children[model.levelsParent].children[model.levelParent].children[model.silhouetteParent].self;
			GameObject piecesParent = view.graph[model.screenParent].children[model.levelsParent].children[model.piecesParent].self;
			silhouette.Setup(
				ViewUtil.GetChildren(silhouetteParent),
				ViewUtil.GetChildren(piecesParent)
			);
		}
		else if (silhouette.Update())
		{
			model.OverlapSilhouette();
		}
	}

	private void UpdateDrag()
	{
		drag.Update();
		model.isItemSelected = drag.isItemSelected;
		if (null != drag.draggedObject && 0.0f != model.rotateDegrees) {
			ViewUtil.Rotate(drag.draggedObject, model.rotateDegrees);
		}
		drag.SetEnabled(model.isDragEnabled);
	}

	public void Update()
	{
		UpdateSilhouette();
		if (!model.isMenu) {
			UpdateDrag();
			ViewUtil.CenterOnScreen(view.graph[model.screenParent].children[model.levelsParent].children[model.piecesParent].self);
		}
	}
}

