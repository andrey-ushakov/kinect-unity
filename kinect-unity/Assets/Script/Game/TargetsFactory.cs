using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetsFactory : MonoBehaviour {

	public Dictionary<TargetType, Queue<Target>> availableTargetsByType = new Dictionary<TargetType, Queue<Target>>();

	private int targetCount = 0;
	
	[SerializeField]
	private float setSpeed = 7f;
	private static float speed = 7f;

	[SerializeField]
	private GameObject targetPrefab;
	[SerializeField]
	private int numberOfTopTargetsToPreinstantiate;
	[SerializeField]
	private int numberOfLeftTargetsToPreinstantiate;
	[SerializeField]
	private int numberOfRightTargetsToPreinstantiate;

	private static TargetsFactory Instance {
		get;
		set;
	}


	private void Awake() {
		speed = setSpeed;

		if (Instance != null) {
			Debug.LogError("There is multiple instance of singleton TargetsFactory");
			return;
		}
		
		Instance = this;
		
		foreach (object value in System.Enum.GetValues(typeof(TargetType))) {
			this.availableTargetsByType.Add((TargetType)value, new Queue<Target>());
		}
	}


	private void Start() {
		if (this.targetPrefab == null) {
			Debug.LogError("A target prefab is not set.");
			return;
		}

		PreinstantiateTargets(TargetType.TopTarget, 	this.numberOfTopTargetsToPreinstantiate);
		PreinstantiateTargets(TargetType.LeftTarget, 	this.numberOfLeftTargetsToPreinstantiate);
		PreinstantiateTargets(TargetType.RightTarget, 	this.numberOfRightTargetsToPreinstantiate);
	}


	private static void PreinstantiateTargets(TargetType targetType, int numberOfTargetsToPreinstantiate) {
		Queue<Target> targets = TargetsFactory.Instance.availableTargetsByType[targetType];

		for (int index = 0; index < numberOfTargetsToPreinstantiate; index++) {
			Target target = InstantiateTarget(targetType);
			if (target == null) {
				Debug.LogError(string.Format("Failed to instantiate {0} targets.", numberOfTargetsToPreinstantiate));
				break;
			}
			targets.Enqueue(target);
		}
	}


	private static Target InstantiateTarget(TargetType targetType) {
		GameObject gameObject = null;
		
		gameObject = (GameObject) GameObject.Instantiate(Instance.targetPrefab);
		
		gameObject.SetActive(false);
		gameObject.tag = targetType.ToString();
		gameObject.transform.parent = TargetsFactory.Instance.gameObject.transform;
		Target target = gameObject.GetComponent<Target>();

		target.Type		= targetType;
		target.Score 	= GameManager.TargetScore;
		target.Damage 	= GameManager.TargetDamage;

		switch (targetType) {
		case TargetType.TopTarget:
			target.Direction = new Vector3(0, 3, -7) * speed;
			break;
			
		case TargetType.LeftTarget:
			target.Direction = new Vector3(-6, 0, -7) * speed;
			break;
			
		case TargetType.RightTarget:
			target.Direction = new Vector3(6, 0, -7) * speed;
			break;
		}

		TargetsFactory.Instance.targetCount++;
		return target;
	}


	public static Target GetTarget(TargetType targetType) {
		Queue<Target> availableTargets = TargetsFactory.Instance.availableTargetsByType[targetType];
		
		Target target = null;
		if (availableTargets.Count > 0) {
			target = availableTargets.Dequeue();
		}
		
		if (target == null) {
			// Instantiate a new target.
			target = InstantiateTarget(targetType);
			//Debug.Log("Number of targets instantiated = " + TargetsFactory.Instance.targetCount + "\n" + targetType.ToString());
		}


		target.Position = new Vector3 (0, 0, 10);
		target.gameObject.SetActive(true);
		
		return target;
	}


	public static void ReleaseTarget(Target target) {
		Queue<Target> availableTargets = TargetsFactory.Instance.availableTargetsByType[target.Type];
		target.gameObject.SetActive(false);
		availableTargets.Enqueue(target);

		Debug.Log (availableTargets.Count);
	}


	public static void ReleaseAllTargetsByType(TargetType targetType) {
		int score = 0;
		GameObject[] gameObjectsToRelease = GameObject.FindGameObjectsWithTag (targetType.ToString());

		foreach(GameObject gameObject in gameObjectsToRelease) {
			// add score
			Target target = gameObject.GetComponent<Target>();
			score += target.Score;
			// release
			TargetsFactory.ReleaseTarget(target);
		}

		return score;
	}


}
