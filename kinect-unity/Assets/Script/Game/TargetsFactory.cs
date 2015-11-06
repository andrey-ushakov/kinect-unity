﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetsFactory : MonoBehaviour {

	private Dictionary<TargetType, Queue<Target>> availableTargetsByType = new Dictionary<TargetType, Queue<Target>>();

	private int targetCount = 0;

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
		
		//gameObject = (GameObject) GameObject.Instantiate(Instance.targetPrefab);

		// TODO set direction
		switch (targetType) {
		case TargetType.TopTarget:
			gameObject = (GameObject) GameObject.Instantiate(Instance.targetPrefab);
			break;
			
		case TargetType.LeftTarget:
			gameObject = (GameObject) GameObject.Instantiate(Instance.targetPrefab);
			break;
			
		case TargetType.RightTarget:
			gameObject = (GameObject) GameObject.Instantiate(Instance.targetPrefab);
			break;
		}
		
		gameObject.SetActive(false);
		gameObject.transform.parent = TargetsFactory.Instance.gameObject.transform;	// TODO
		Target target = gameObject.GetComponent<Target>();
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
			Debug.Log("Number of targets instantiated = " + TargetsFactory.Instance.targetCount + "\n" + targetType.ToString());
		}

		Vector3 position = new Vector3 (0, 0, 0);

		switch (targetType) {
		case TargetType.TopTarget:
			position = new Vector3 (0, 4, 0);
			break;
			
		case TargetType.LeftTarget:
			position = new Vector3 (-17, 0, 0);
			break;
			
		case TargetType.RightTarget:
			position = new Vector3 (17, 0, 0);
			break;
		}

		target.Position = position;
		target.gameObject.SetActive(true);
		
		return target;
	}

	public static void ReleaseTarget(Target target) {
		Queue<Target> availableTargets = TargetsFactory.Instance.availableTargetsByType[target.Type];
		target.gameObject.SetActive(false);
		availableTargets.Enqueue(target);
	}


}