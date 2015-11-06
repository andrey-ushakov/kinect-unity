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

		// TODO chose random target type
		//float randomY = Random.Range(-4f, 4f);
		//TargetsFactory.GetTarget(new Vector3(0,0,0), Quaternion.Euler(0f, 0f, 180f), prefabPath);

		System.Array values = TargetType.GetValues(typeof(TargetType));
		TargetType targetType = (TargetType)values.GetValue(Random.Range(0, values.Length));

		Target target = TargetsFactory.GetTarget(targetType);
		//target.Initialize(direction, this.BulletSpeed, this.BulletDamage);

		//Debug.Log ("SPAWN !");
		/*Instantiate(target.transform,
		            target.Position,
		            Quaternion.identity);*/

		this.lastTargetSpawnTime = Time.time;
		
	}
}
