using UnityEngine;  // Vector2 data type

public class Main : MainView
{
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
	}
}
