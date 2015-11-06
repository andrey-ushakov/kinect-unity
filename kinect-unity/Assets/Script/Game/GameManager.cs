using UnityEngine;
using System.Collections;

using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private float rateOfTargetSpawn = 0.2f;

	private double lastTargetSpawnTime;

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
	}


	void Start () {
	}
	
	void Update () {
		RandomSpawn ();
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
		TargetType targetType = (TargetType)values.GetValue(Random.Range(0, values.Length));

		Target target = TargetsFactory.GetTarget(targetType);

		this.lastTargetSpawnTime = Time.time;
	}
}
