using UnityEngine;
using System;
using System.Collections;

public delegate void TargetPlayerCollisionHandler(object sender, EventArgs e);

public class Target : MonoBehaviour {

	private TargetType type;
	private Vector3 direction;
	private int score;
	private float damage;

	public static event TargetPlayerCollisionHandler targetPlayerCollision;

	
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
		set {
			this.score = value;
		}
	}

	public float Damage {
		get {
			return this.damage;
		}
		set {
			this.damage = value;
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
		this.score = 0;
		this.damage = 0f;
	}

	private void Update() {
		this.UpdatePosition();
		
		// release target if it is in collision with player
		if (this.Position.z < -5) {
			TargetsFactory.ReleaseTarget(this);

			// player collision event
			if (targetPlayerCollision != null) {
				targetPlayerCollision(this, EventArgs.Empty);
			}
		}
	}

	private void UpdatePosition() {
		this.Position += (this.direction) * Time.deltaTime;
	}
}
