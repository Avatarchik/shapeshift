public class Main : MainView
{
	private TangramController tangram = new TangramController();

	public override void Start()
	{
		tangram.Start();
		controller.SetModel(tangram.model);
		base.Start();
	}

	public override void Update()
	{
		base.Update();
		tangram.Update();
	}
}
