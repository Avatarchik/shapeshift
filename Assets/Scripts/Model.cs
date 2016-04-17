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
	public string levelState = "Level_0";
	public string levelParent = "Level_0";
	public string[] piecesLayers = new string[]{"Pieces"};
	public string[] screen;
	private int score;
	private int scorePerPuzzle = 10;
	private int scorePerRotation = -1;
	private float degreesPerRotation = 45.0f;
	private string[] scoreText = new string[]{
		"Canvas", "Panel", "ScoreText"};
	private string[] messageText = new string[]{
		"Canvas", "Panel", "MessageText"};

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
			"Level_0_Button",
			"Level_1_Button"
		};
		view.graph = new Dictionary<string, object>(){
			{"Canvas", new Dictionary<string, object>(){
				{"Panel", new Dictionary<string, object>(){
					{"ScoreText", null},
					{"MessageText", null}
				}}
			}},
			{"World", new Dictionary<string, object>(){
				{"Levels", new Dictionary<string, object>(){
					{"Pieces", null},
					{"Level_0", new Dictionary<string, object>(){
						{"Silhouette", null}
					}},
					{"Level_1", new Dictionary<string, object>(){
						{"Silhouette", null}
					}}
				}}
			}},
			{"Developer", new Dictionary<string, object>(){
				{"SilhouettePoint", null}
			}}
		};
		isOverlaps = new Dictionary<string, bool>(){
			{"Level_0", false},
			{"Level_1", false}
		};
		SetState("Level_0");
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
			string message = "Click MENU to shift into your next shape.";
			view.SetText(messageText, message);
		}
	}

	private void SetState(string state)
	{
		levelState = state;
		levelParent = state;
		isMenu = "Menu" == levelState;
		isDragEnabled = !isMenu;
		isOverlapSilhouette = false;
		view.SetState(screen, levelState);
		string message;
		if (isMenu) {
			message = "To score high, pick a new shape with few rotations.";
		}
		else {
			message = "Fit animal: +10 points"
				+ "\nRotate: -1 point";
		}
		view.SetText(messageText, message);
	}

	private void UpdateMenu()
	{
		if (isMenu) {
			if ("Level_0_Button" == view.mouseDown) {
				SetState("Level_0");
			}
			else if ("Level_1_Button" == view.mouseDown) {
				SetState("Level_1");
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
