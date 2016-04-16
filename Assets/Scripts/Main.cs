using UnityEngine;  // Vector2 data type

public class Main : MainView
{
	public Vector2 silhouettePoint;
	private TangramController tangram = new TangramController();

	public override void Start()
	{
		tangram.Start();
		controller.SetModel(tangram.model);
		base.Start();
		tangram.view = controller.view;
	}

	public override void Update()
	{
		base.Update();
		tangram.Update();
		silhouettePoint = tangram.silhouette.point;
		ViewUtil.SetPosition2D(controller.view.graph["Developer"].children["SilhouettePoint"].self, silhouettePoint);
	}
}
