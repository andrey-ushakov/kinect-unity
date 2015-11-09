using UnityEngine;
using System;
using System.Collections;

public delegate void TargetPlayerCollisionHandler(object sender, EventArgs e);

public class Target : MonoBehaviour {

	private TargetType type;
	private Vector3 direction	= new Vector3 (0, 0, 0);
	private int score			= 1;
	private int damage			= 1;

	private int bonusScore		= 0;
	private float bonusTime		= 0f;
	private int bonusLife		= 0;


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

	public int Damage {
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

	public int BonusScore {
		get {
			return this.bonusScore;
		}
		set {
			this.bonusScore = value;
		}
	}
	
	public float BonusTime {
		get {
			return this.bonusTime;
		}
		set {
			this.bonusTime = value;
		}
	}
	
	public int BonusLife {
		get {
			return this.bonusLife;
		}
		set {
			this.bonusLife = value;
		}
	}



	private void Update() {
		this.UpdatePosition();
		
		// release target if it is in collision with player
		if (this.Position.z < -5 || this.Position.x < -13 || this.Position.x > 13) {
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
