using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GameMode gameMode = GameMode.LimitedTime;

	[SerializeField]
	private float timeLeft = 10f;

	private float timeCounter = 0;
	
	[SerializeField]
	private int targetDamage = 1;
	
	[SerializeField]
	private int targetScore = 1;

	[SerializeField]
	private int life = 5;

	private int score = 0;

	// LimitedTime
	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text timeLeftText;

	// LimitedLife
	[SerializeField]
	private Text timePassedText;
	[SerializeField]
	private Text lifesText;

	[SerializeField]
	public Texture textureRightHandUp;

	private SkeletonWrapper sw;
	private HandInputManager him;

	public int Score {
		get {
			return this.score;
		}
		private set {
			this.score = value;
		}
	}

	public float TimePassed {
		get {
			return this.timeCounter;
		}
		private set {
			this.timeCounter = value;
		}
	}

	public GameMode GameMode {
		get {
			return this.gameMode;
		}
		private set {
			this.gameMode = value;
		}
	}


	[SerializeField]
	private float rateOfTargetSpawn = 0.2f;

	private double lastTargetSpawnTime;

	public static GameManager Instance {
		get;
		private set;
	}

	public static int TargetScore;

	public static int TargetDamage;

	private bool isFinished = false;



	private void Awake() {
		if (Instance != null) {
			Debug.LogError("There is multiple instance of singleton GameManager");
			return;
		}
		
		Instance = this;

		sw = SkeletonWrapper.Instance;
		him = HandInputManager.Instance;

		gameMode = GlobalVariables.gameMode;
		TargetScore = targetScore;
		TargetDamage = targetDamage;

		SetupUI ();
	}


	void SetupUI() {
		switch(gameMode) {
		case GameMode.LimitedTime:
			// show/hide text
			scoreText.gameObject.SetActive(true);
			timeLeftText.gameObject.SetActive(true);
			timePassedText.gameObject.SetActive(false);
			lifesText.gameObject.SetActive(false);
			// display score
			UpdateScoreText ();
			break;
			
		case GameMode.LimitedLife:
			// show/hide text
			scoreText.gameObject.SetActive(false);
			timeLeftText.gameObject.SetActive(false);
			timePassedText.gameObject.SetActive(true);
			lifesText.gameObject.SetActive(true);
			// display lifes
			UpdateLifesText ();
			break;
		}
	}


	void Start () {
	}
	
	void Update () {
		if (isFinished) {
			return;
		}

		RandomSpawn ();

		switch(gameMode) {
		case GameMode.LimitedTime:
			timeLeft -= Time.deltaTime;
			UpdateTimeLeftText();
			if (timeLeft <= 0) {
				timeLeft = 0;
				showFinishScreen();
			}
			break;
		case GameMode.LimitedLife:
			timeCounter += Time.deltaTime;
			UpdateTimePassedText();
			break;
		}
	}


	private void RandomSpawn() {
		if (this.rateOfTargetSpawn <= 0f) {
			return;
		}
		
		float durationBetweenTwoTargetsSpawn = 1f / this.rateOfTargetSpawn;

		if (Time.time < this.lastTargetSpawnTime + durationBetweenTwoTargetsSpawn) {
			return;
		}

		// chose random target type
		System.Array values = TargetType.GetValues(typeof(TargetType));
		TargetType targetType = (TargetType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		// spawn !
		TargetsFactory.GetTarget(targetType);

		this.lastTargetSpawnTime = Time.time;
	}


	void OnEnable() {
		Target.targetPlayerCollision += OnTargetPlayerCollision;
		HandInputManager.handMotionDetected += OnHandMotion;
	}
	
	void OnDisable() {
		Target.targetPlayerCollision -= OnTargetPlayerCollision;
		HandInputManager.handMotionDetected -= OnHandMotion;
	}

	void OnTargetPlayerCollision(object sender, EventArgs e) {
		Target target = sender as Target;

		if (gameMode == GameMode.LimitedLife) {
			life -= target.Damage;
			UpdateLifesText ();
			if (life <= 0) {
				life = 0;
				showFinishScreen();
			}
		}
	}


	void OnHandMotion(object sender, HandMotionDetectedEventArgs args) {
		int deltaScore = 0;

		switch(args.motion) {
		case HandMotion.LEFT_HAND_WAVE_OUT:
			deltaScore = TargetsFactory.ReleaseAllTargetsByType(TargetType.LeftTarget);
			break;
		case HandMotion.RIGHT_HAND_WAVE_OUT:
			deltaScore = TargetsFactory.ReleaseAllTargetsByType(TargetType.RightTarget);
			break;
		case HandMotion.TWO_HAND_RISE:
			deltaScore = TargetsFactory.ReleaseAllTargetsByType(TargetType.TopTarget);
			break;
		case HandMotion.RIGHT_HAND_RISE:
			if(isFinished) {
				Application.LoadLevel("MainMenu");
			}
			break;
		}

        if (gameMode == GameMode.LimitedTime) {
            this.score += deltaScore;
            UpdateScoreText();
        }

	}


	void UpdateScoreText() {
		scoreText.text = "Score: " + score.ToString ();
	}
	
	void UpdateTimeLeftText() {
		timeLeftText.text = "Time Left: " + timeLeft.ToString ("F1") + " s.";
	}
	
	void UpdateTimePassedText() {
		timePassedText.text = "Time Passed: " + timeCounter.ToString ("F1") + " s.";
	}
	
	void UpdateLifesText() {
		lifesText.text = "Lifes: " + life.ToString ();
	}


	void showFinishScreen() {
		// stop spawn objects and clear scene
		isFinished = true;
		TargetsFactory.ReleaseAllTargets ();

		// hide texts
		scoreText.gameObject.SetActive(false);
		timeLeftText.gameObject.SetActive(false);
		timePassedText.gameObject.SetActive(false);
		lifesText.gameObject.SetActive(false);

		// show final dialog
		if (gameObject.GetComponent<FinalDialog> () == null) {
			gameObject.AddComponent<FinalDialog> ();
		}
	}
}
