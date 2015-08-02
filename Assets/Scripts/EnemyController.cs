using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public float maxHealth = 10;
	public float currentHealth;
	public float speed;
	public float rotationTime = 540;
	public int score = 10;

	// Use this for initialization
	public virtual void Start () {
	}
	public virtual void OnEnable(){
		currentHealth = maxHealth;
	}
	// Update is called once per frame
	public void FixedUpdate () {
		Move (NextPos());
		Rotate();
	}

	public void Update(){
		if (currentHealth <= 0) {
			GameManager.current.AddScore(score);
			GameManager.current.enemigosEliminados++;
			gameObject.SetActive (false);
		}
	}

	public void Move(Vector3 nextPosition){
		transform.position = Vector2.MoveTowards (transform.position,nextPosition ,speed* Time.deltaTime);
	}

	public void Rotate(){
		Vector2 direction = PlayerController.current.transform.position - transform.position;
		direction = direction.normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, 0, angle), rotationTime * Time.deltaTime);
	}

	public virtual Vector3 NextPos(){
		return PlayerController.current.transform.position;
	}

	

}
