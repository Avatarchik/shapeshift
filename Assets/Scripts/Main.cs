public class Main : MainView
{
	private InputManager drag = new InputManager();
	private Model model;

	public override void Start()
	{
		model = new Model();
		controller.SetModel(model);
		base.Start();
	}

	public override void Update()
	{
		drag.isEnabled = model.isDragEnabled;
		drag.Update();
		base.Update();
	}
}
