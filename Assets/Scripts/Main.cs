using UnityEngine;  // Vector2 data type

public class Main : MainView
{
	public Vector2 silhouettePoint;
	public bool isDragEnabled;
	private TangramController tangram = new TangramController();

	public override void Start()
	{
		tangram.Start();
		controller.SetModel(tangram.model);
		base.Start();
		tangram.view = controller.view;
	}

	private void UpdateSilhouettePoint()
	{
		silhouettePoint = tangram.silhouette.point;
		var gameObject = controller.view.graph["Developer"].children["SilhouettePoint"].self;
		ViewUtil.SetPosition2D(gameObject, silhouettePoint);
		bool isVisible = tangram.model.isDragEnabled && !tangram.silhouette.isPerfect;
		ViewUtil.SetVisible(gameObject, isVisible);
	}

	public override void Update()
	{
		base.Update();
		tangram.Update();
		UpdateSilhouettePoint();
		isDragEnabled = tangram.model.isDragEnabled;
	}
}
