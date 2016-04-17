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
		drag.disableTime = 0.125f;
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

	// Show piece selected at last dragged object.
	// Test case:  2016-04-16 Rotate.  Jennifer Russ expects to see which piece will rotate.  Got confused.
	private void UpdateViewPieceSelected()
	{
		var levelsTree = view.graph[model.screenParent].children[model.levelsParent];
		var pieceSelected = levelsTree.children[model.pieceSelectedParent].self;
		if (null != drag.draggedObject) {
			ViewUtil.Reposition(drag.draggedObject, pieceSelected);
		}
		ViewUtil.SetVisible(pieceSelected,
			model.isDragEnabled && drag.isItemSelected);
	}

	private void UpdateSilhouettePoint()
	{
		var gameObject = view.graph["Developer"].children["SilhouettePoint"].self;
		ViewUtil.SetPosition(gameObject, silhouette.point);
		bool isVisible = model.isDragEnabled && !silhouette.isPerfect && !model.isMenu;
		ViewUtil.SetVisible(gameObject, isVisible);
	}

	private void UpdateDrag()
	{
		drag.SetEnabled(model.isDragEnabled);
		drag.Update();
		model.isItemSelected = drag.isItemSelected;
		if (null != drag.draggedObject && 0.0f != model.rotateDegrees) {
			ViewUtil.Rotate(drag.draggedObject, model.rotateDegrees);
		}
	}

	public void Update()
	{
		UpdateSilhouette();
		UpdateSilhouettePoint();
		UpdateDrag();
		if (!model.isMenu && !model.wasComplete()) {
			ViewUtil.CenterOnScreen(view.graph[model.screenParent].children[model.levelsParent].children[model.piecesParent].self);
		}
		UpdateViewPieceSelected();
	}
}

