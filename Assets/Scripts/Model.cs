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
	public string pieceSelectedParent = "PieceSelected";
	public string silhouetteParent = "Silhouette";
	public string levelsParent = "Levels";
	public string levelState = "Level_0";
	public string levelParent = "Level_0";
	public string[] piecesLayers = new string[]{"Pieces"};
	public string[] canvas;
	public string[] screen;
	private int score;
	private int scorePerPuzzle = 10;
	private int scorePerRotation = -1;
	private float degreesPerRotation = 45.0f;
	private string[] scoreText = new string[]{
		"Canvas", "ScoreText"};
	private string[] messageText = new string[]{
		"Canvas", "Panel", "MessageText"};

	public void SetViewModel(ViewModel viewModel)
	{
		view = viewModel;
	}

	public void Start()
	{
		score = 0;
		screen = new string[]{screenParent};
		canvas = new string[]{"Canvas"};
		view.buttons = new string[] {
			"LeftButton",
			"RightButton",
			"MenuButton",
			"Level_0_Button",
			"Level_1_Button"
		};
		view.graph = new Dictionary<string, object>(){
			{"Canvas", new Dictionary<string, object>(){
				{"ScoreText", null},
				{"Panel", new Dictionary<string, object>(){
					{"MessageText", null}
				}}
			}},
			{"World", new Dictionary<string, object>(){
				{"Levels", new Dictionary<string, object>(){
					{"Pieces", null},
					{"PieceSelected", null},
					{"Level_0", new Dictionary<string, object>(){
						{"Silhouette", null},
						{"Feedback", null}
					}},
					{"Level_1", new Dictionary<string, object>(){
						{"Silhouette", null},
						{"Feedback", null}
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
			string message = "Click MENU to shapeshift into your next animal.";
			view.SetText(messageText, message);
			string[] feedback = new string[]{
				screenParent, levelsParent, levelParent, "Feedback"};
			view.SetState(feedback, "Correct");
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
		string canvasState = isMenu ? levelState : "None";
		view.SetState(canvas, canvasState);
		string message;
		if (isMenu) {
			message = "To score high, shapeshift into similar animals first.";
		}
		else {
			if (isOverlaps[levelParent]) {
				message = "You already shapeshifted into this animal.";
			}
			else {
				message = "Shapeshift into this animal: +10 points";
			}
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
