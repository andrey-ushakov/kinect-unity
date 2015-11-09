using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GameMode gameMode = GameMode.LimitedTime;
	private SkeletonWrapper sw;
	private HandInputManager him;

	// LimitedTime mode variables
	// Timer
	[SerializeField]
	private float timeLeft = 10f;
	[SerializeField]
	private int targetScore = 1;
	// Score counter
	private int score = 0;
	// GUI text
	[SerializeField]
	private Text scoreText;
	// GUI text
	[SerializeField]
	private Text timeLeftText;


	// LimitedLife mode variables
	// Life counter
	[SerializeField]
	private int life = 5;
	// Time counter
	private float timeCounter = 0;
	// GUI text
	[SerializeField]
	private Text timePassedText;
	// GUI text
	[SerializeField]
	private Text lifesText;


	// Both modes variables
	[SerializeField]
	private int targetDamage = 1;
	// Final dialog icon
	[SerializeField]
	public Texture textureRightHandUp;
	[SerializeField]
	private float rateOfTargetSpawn = 1f;

	// Bonus objects
	[SerializeField]
	private float rateOfBonusTargetSpawn = 0.5f;
	[SerializeField]
	private int bonusTargetScore = 2;		// bonus score for LimitedTime mode
	[SerializeField]
	private float bonusTargetTime = 2f;		// bonus time for LimitedTime mode
	[SerializeField]
	private int bonusTargetLife = 1;		// bonus life for LimitedLife mode



	// Internal variables
	private bool isFinished = false;
	private double lastTargetSpawnTime;
	private double lastBonusTargetSpawnTime;
	

	// Class properties
	public static int TargetScore;
	public static int TargetDamage;
	public static int TargetBonusScore;
	public static float TargetBonusTime;
	public static int TargetBonusLife;
	
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



	public static GameManager Instance {
		get;
		private set;
	}




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
		TargetBonusScore = bonusTargetScore;
		TargetBonusTime = bonusTargetTime;
		TargetBonusLife = bonusTargetLife;

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

		TargetSpawn ();
		BonusTargetSpawn ();

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


	private void TargetSpawn() {
		if (this.rateOfTargetSpawn <= 0f) {
			return;
		}
		
		float durationBetweenTwoTargetsSpawn = 1f / this.rateOfTargetSpawn;
		
		if (Time.time < this.lastTargetSpawnTime + durationBetweenTwoTargetsSpawn) {
			return;
		}
		
		// chose random target type
		System.Array values = new int[3];
		values.SetValue(TargetType.TopTarget, 0);
		values.SetValue(TargetType.LeftTarget, 1);
		values.SetValue(TargetType.RightTarget, 2);
		TargetType targetType = (TargetType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		// spawn !
		TargetsFactory.GetTarget(targetType);
		
		this.lastTargetSpawnTime = Time.time;
	}

	private void BonusTargetSpawn() {
		if (this.rateOfBonusTargetSpawn <= 0f) {
			return;
		}
		
		float durationBetweenTwoTargetsSpawn = 1f / this.rateOfBonusTargetSpawn;
		
		if (Time.time < this.lastBonusTargetSpawnTime + durationBetweenTwoTargetsSpawn) {
			return;
		}
		
		// chose random target type
		System.Array values = new int[2];
		values.SetValue(TargetType.TopBonusTarget, 0);
		values.SetValue(TargetType.BottomBonusTarget, 1);
		TargetType targetType = (TargetType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		// spawn !
		TargetsFactory.GetTarget(targetType);
		
		this.lastBonusTargetSpawnTime = Time.time;
	}


	void OnEnable() {
		Target.targetPlayerCollision += OnTargetPlayerCollision;
		HandInputManager.handMotionDetected += OnHandMotion;
		BodyInputManager.bodyMotionDetected += OnBodyMotion;
	}
	
	void OnDisable() {
		Target.targetPlayerCollision -= OnTargetPlayerCollision;
		HandInputManager.handMotionDetected -= OnHandMotion;
		BodyInputManager.bodyMotionDetected -= OnBodyMotion;
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


	void OnBodyMotion(object sender, BodyMotionDetectedEventArgs args) {
		Bonus bonus = new Bonus();
		switch(args.motion) {
		case BodyMotion.JUMP:
			bonus = TargetsFactory.ReleaseAllBonusTargetsByType(TargetType.TopBonusTarget);
			break;
		case BodyMotion.SUPER_JUMP:
			bonus = TargetsFactory.ReleaseAllBonusTargetsByType(TargetType.BottomBonusTarget);
			break;
		}

		if (gameMode == GameMode.LimitedTime) {
			this.score += bonus.Score;
			this.timeLeft += bonus.Time;
			UpdateScoreText();
		} else if (gameMode == GameMode.LimitedLife) {
			this.life += bonus.Life;
			UpdateLifesText();
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
