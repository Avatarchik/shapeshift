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
	}
}

