using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	private TargetType type;
	private Vector3 direction;
	private int score;

	
	public TargetType Type {
		get {
			return this.type;
		}
		set {
			this.type = value;
		}
	}

	public Vector3 Direction {
		get {
			return this.direction;
		}
		set {
			this.direction = value;
		}
	}

	public int Score {
		get {
			return this.score;
		}
		private set {
			this.score = value;
		}
	}

	public Vector3 Position {
		get {
			return this.transform.position;
		}
		
		set {
			this.transform.position = value;
		}
	}

	private void Awake() {
		this.direction = new Vector3 (0, 0, 0);
		this.score = 3;
	}

	private void Update() {
		this.UpdatePosition();
		
		// release target if it is in collision with player
		if (this.Position.z < -10) {
			TargetsFactory.ReleaseTarget(this);
		}
	}

	private void UpdatePosition() {
		this.Position += (this.direction) * Time.deltaTime;
	}
}
