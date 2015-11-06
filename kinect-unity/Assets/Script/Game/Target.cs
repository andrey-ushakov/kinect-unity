using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	private TargetType type;
	private Vector3 speed;
	private int score;

	
	public TargetType Type {
		get {
			return this.type;
		}
		
		private set {
			this.type = value;
		}
	}

	public Vector3 Speed {
		get {
			return this.speed;
		}

		private set {
			this.speed = speed;
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
		this.speed = new Vector3 (0, 0, 0);
		this.score = 3;
	}

	private void Update() {
		this.UpdatePosition();
		
		// TODO release bullet if it is in collision with player
		/*if (this.Position.x > 20 || this.Position.x < -20 || this.Position.y > 20 || this.Position.y < -20)
		{
			BulletsFactory.ReleaseBullet(this);
		}*/
	}

	private void UpdatePosition() {
		this.Position += this.speed * Time.deltaTime;
	}
}
