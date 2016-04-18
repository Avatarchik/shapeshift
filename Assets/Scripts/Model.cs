using UnityEngine;  // Mathf
using System.Collections.Generic;  // Dictionary

public class Model : IModel
{
	public ViewModel view;
	public bool isMenu = false;
	public bool isDragEnabled = true;
	public bool isItemSelected = false;
	public bool isOverlapSilhouette = false;
	public Dictionary <string, bool> isOverlaps;
	public Dictionary <string, int> rotationCounts;
	public Dictionary <string, float> rotationNormals;
	public float rotateDegrees = 0.0f;
	public string screenParent = "World";
	public string piecesParent = "Pieces";
	public string pieceSelectedParent = "PieceSelected";
	public string silhouetteParent = "Silhouette";
	public string levelsParent = "Levels";
	public string levelState;
	public string levelParent;
	public string[] piecesLayers = new string[]{"Pieces"};
	public string[] canvas;
	public string[] screen;
	private int levelCount = 9;
	private Dictionary<string, object> levels;
	private float normalPerRotation = -0.1f;
	private int score;
	private int scorePerPuzzle = 20;
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

	private void SetupLevels(int buttonOffset)
	{
		levels = (Dictionary<string, object>) view.graph[screenParent];
		levels = (Dictionary<string, object>) levels[levelsParent];
		rotationCounts = new Dictionary<string, int>();
		rotationNormals = new Dictionary<string, float>();
		for (int level = 0; level < levelCount; level++) {
			string levelName = "Level_" + level;
			view.buttons[level + buttonOffset] = levelName + "/" + levelName;
			isOverlaps[levelName] = false;
			rotationCounts[levelName] = 0;
			rotationNormals[levelName] = 1.0f;
			levels[levelName] =
				new Dictionary<string, object>(){
					{"Silhouette", null},
					{"Feedback", new Dictionary<string, object>(){
						{"CorrectBackground", null},
						{"Particles", null}
					}}
				};
		}
		SetState("Level_0");
	}

	public void Start()
	{
		score = 0;
		screen = new string[]{screenParent};
		canvas = new string[]{"Canvas"};
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
					{"PieceSelected", null}
				}}
			}},
			{"Developer", new Dictionary<string, object>(){
				{"SilhouettePoint", null}
			}}
		};
		isOverlaps = new Dictionary<string, bool>();
		rotationCounts = new Dictionary<string, int>();

		int hudButtonCount = 3;
		int buttonCount = hudButtonCount + levelCount;
		view.buttons = new string[buttonCount];
		string[] buttons = view.buttons;
		buttons[0] = "LeftButton";
		buttons[1] = "RightButton";
		buttons[2] = "MenuButton";
		SetupLevels(hudButtonCount);
	}

 	private void ScoreRotate()
	{
		rotationCounts[levelParent]++;
		rotationNormals[levelParent] = Mathf.Max(0.0f, 
			Mathf.Min(1.0f, 
				rotationNormals[levelParent] 
				+ normalPerRotation));
		score += scorePerRotation;
	}

	private void UpdateRotate()
	{
		rotateDegrees = 0.0f;
		if (isDragEnabled && isItemSelected) {
			if ("LeftButton" == view.mouseDown) {
				ScoreRotate();
				rotateDegrees = degreesPerRotation;
			}
			else if ("RightButton" == view.mouseDown) {
				ScoreRotate();
				rotateDegrees = -degreesPerRotation;
			}
		}
	}

	public bool wasComplete()
	{
		return isOverlaps[levelParent];
	}

	public void OverlapSilhouette()
	{
		if (!isOverlapSilhouette && !wasComplete()) {
			isOverlapSilhouette = true;
			isDragEnabled = false;
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
			message = "For a high score, shapeshift into a SIMILAR animal first.";
		}
		else {
			if (wasComplete()) {
				message = "You already shapeshifted into this animal.";
			}
			else {
				message = "Shapeshift into this animal:\n+" + scorePerPuzzle + " points";
			}
		}
		view.SetText(messageText, message);
	}

	private void UpdateMenu()
	{
		if (isMenu) {
			if (levels.ContainsKey(view.mouseDown)) {
				SetState(view.mouseDown);
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
