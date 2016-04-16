using System.Collections.Generic;  // Dictionary

public class Model : IModel
{
	private ViewModel view;
	public bool isDragEnabled = true;
	public float rotateDegrees = 0.0f;
	private int score;
	private int scorePerPuzzle = 10;
	private int scorePerRotation = -1;
	private float degreesPerRotation = 45.0f;
	private string[] scoreText = new string[]{
		"Canvas", "Panel", "ScoreText"};

	public void SetViewModel(ViewModel viewModel)
	{
		view = viewModel;
	}

	public void Start()
	{
		score = scorePerPuzzle;
		view.buttons = new string[] {
			"LeftButton",
			"RightButton",
			"MenuButton"
		};
		view.graph["Canvas"] = new Dictionary<string, object>(){
			{"Panel", new Dictionary<string, object>(){
				{"ScoreText", null}
			}}
		};
	}

	private void UpdateRotate()
	{
		rotateDegrees = 0.0f;
		if ("LeftButton" == view.mouseDown) {
			score += scorePerRotation;
			rotateDegrees = -degreesPerRotation;
		}
		else if ("RightButton" == view.mouseDown) {
			score += scorePerRotation;
			rotateDegrees = degreesPerRotation;
		}
	}

	public void Update(float deltaTime)
	{
		UpdateRotate();
		view.SetText(scoreText, score.ToString());
	}
}
