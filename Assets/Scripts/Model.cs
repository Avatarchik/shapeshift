using System.Collections.Generic;  // Dictionary

public class Model : IModel
{
	public ViewModel view;
	public bool isMenu = false;
	public bool isDragEnabled = true;
	public bool isItemSelected = false;
	public bool isOverlapSilhouette = false;
	public Dictionary <string, bool> isOverlaps;
	public float rotateDegrees = 0.0f;
	public string screenParent = "World";
	public string piecesParent = "Pieces";
	public string silhouetteParent = "Silhouette";
	public string levelsParent = "Levels";
	public string levelState = "LevelBuffalo";
	public string levelParent = "LevelBuffalo";
	public string[] piecesLayers = new string[]{"Pieces"};
	public string[] screen;
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
		screen = new string[]{"World"};
		view.buttons = new string[] {
			"LeftButton",
			"RightButton",
			"MenuButton",
			"LevelBuffaloButton",
			"LevelFoxButton"
		};
		view.graph = new Dictionary<string, object>(){
			{"Canvas", new Dictionary<string, object>(){
				{"Panel", new Dictionary<string, object>(){
					{"ScoreText", null}
				}}
			}},
			{"World", new Dictionary<string, object>(){
				{"Levels", new Dictionary<string, object>(){
					{"Pieces", null},
					{"LevelBuffalo", new Dictionary<string, object>(){
						{"Silhouette", null}
					}},
					{"LevelFox", new Dictionary<string, object>(){
						{"Silhouette", null}
					}}
				}}
			}},
			{"Developer", new Dictionary<string, object>(){
				{"SilhouettePoint", null}
			}}
		};
		isOverlaps = new Dictionary<string, bool>(){
			{"LevelBuffalo", false},
			{"LevelFox", false}
		};
		SetState("LevelBuffalo");
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
		if (!isOverlapSilhouette && !isOverlaps[levelParent]) {
			isOverlapSilhouette = true;
			score += scorePerPuzzle;
			isOverlaps[levelParent] = true;
		}
	}

	private void SetState(string state)
	{
		levelState = state;
		levelParent = state;
		view.SetState(screen, levelState);
		isMenu = "Menu" == levelState;
		isDragEnabled = !isMenu;
		isOverlapSilhouette = false;
	}

	private void UpdateMenu()
	{
		if (isMenu) {
			if ("LevelBuffaloButton" == view.mouseDown) {
				SetState("LevelBuffalo");
			}
			else if ("LevelFoxButton" == view.mouseDown) {
				SetState("LevelFox");
			}
		}
		else {
			if ("MenuButton" == view.mouseDown) {
				SetState("Menu");
			}
		}
	}

	public void Update(float deltaTime)
	{
		UpdateMenu();
		if (!isMenu) {
			UpdateRotate();
			view.SetText(scoreText, score.ToString());
		}
	}
}
