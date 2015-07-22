using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public float maxHealth;
	public float currentHealth;
	public float speed;
	public float rotationTime = 540;

	// Use this for initialization
	public virtual void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Move (NextPos());
		Rotate();
	}

	public virtual void Move(Vector3 nextPosition){
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
