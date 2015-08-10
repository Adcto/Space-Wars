using UnityEngine;
using System.Collections;

public class Asteroide : EnemyController {
	public int tipo = 1;
	public int hijos = 1;
	public int maxDivisiones = 3;
	public Vector2 direction;
	public Vector2 impacto;
	public bool cambio;
	public Vector2 rebote;
	private Rigidbody2D rig;

	public override void Start ()
	{
		rig = GetComponent<Rigidbody2D> ();
		base.Start ();
	}

	public override void OnEnable(){
		base.OnEnable ();
		direction = direction.normalized;
		currentHealth/=tipo;
		speed += tipo;
		score += 10 * tipo;
	}

	// Update is called once per frame
	public override void FixedUpdate () {
		if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Spawn")) {
			rig.velocity = direction * speed;
			Rotate ();
		}
	}

	
	public override void Rotate(){
		transform.Rotate (0, 0, rotationTime * Time.fixedDeltaTime);
	}

	void OnDisable(){
		//crear cantidad segun tipo, dar direcciones, dar calidad
		if (tipo != maxDivisiones) {
			for(int i = 0; i < hijos; i++){
				GameObject go = Pool.current.Crear_Enemigo (6);
				go.transform.position = transform.position;
				go.transform.localScale = transform.localScale - Vector3.one;
				Asteroide nuevo = go.GetComponent<Asteroide> ();
				nuevo.tipo = tipo + 1;
				nuevo.direction += impacto;
				nuevo.calidad = calidad;
			//	nuevo.hijos = hijos-1;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Finish") {
			cambio = true;
			direction = Vector2.Reflect(direction,other.contacts[0].normal);
		}
	}
}
