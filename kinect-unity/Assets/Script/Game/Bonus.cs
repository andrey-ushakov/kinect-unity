using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	private int score = 0;		// bonus score
	private float time = 0f;	// bonus time
	private int life = 0;		// bonus lifes

	public int Score {
		get {
			return this.score;
		}
		set {
			this.score = value;
		}
	}

	public float Time {
		get {
			return this.time;
		}
		set {
			this.time = value;
		}
	}

	public int Life {
		get {
			return this.life;
		}
		set {
			this.life = value;
		}
	}
}
