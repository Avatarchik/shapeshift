using System.Collections.Generic;  // Dictionary

public class Model : IModel
{
	public ViewModel view;
	public bool isDragEnabled = true;
	public bool isItemSelected = false;
	public bool isOverlapSilhouette = false;
	public float rotateDegrees = 0.0f;
	public string piecesParent = "Pieces";
	public string silhouetteParent = "Silhouette";
	public string levelParent = "Level_Example";
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
		view.graph = new Dictionary<string, object>(){
			{"Canvas", new Dictionary<string, object>(){
				{"Panel", new Dictionary<string, object>(){
					{"ScoreText", null}
				}}
			}},
			{"Levels", new Dictionary<string, object>(){
				{"Level_Example", new Dictionary<string, object>(){
					{"Silhouette", null},
					{"Pieces", null}
				}}
			}},
			{"Developer", new Dictionary<string, object>(){
				{"SilhouettePoint", null}
			}}
		};
	}

	private void UpdateRotate()
	{
		rotateDegrees = 0.0f;
		if (isDragEnabled && isItemSelected) {
			if ("LeftButton" == view.mouseDown) {
				score += scorePerRotation;
				rotateDegrees = degreesPerRotation;
			}
			else if ("RightButton" == view.mouseDown) {
				score += scorePerRotation;
				rotateDegrees = -degreesPerRotation;
			}
		}
	}

	public void OverlapSilhouette()
	{
		if (!isOverlapSilhouette) {
			isOverlapSilhouette = true;
			score += scorePerPuzzle;
		}
	}

	public void Update(float deltaTime)
	{
		UpdateRotate();
		view.SetText(scoreText, score.ToString());
	}
}
