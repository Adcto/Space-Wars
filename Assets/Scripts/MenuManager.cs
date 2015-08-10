using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	public Menu CurrentMenu;
	public Menu PreviousMenu;
	private int Selected = 0;
	private bool comprar = false;
	private bool equipar = false;
	private GameObject disparoEquipable;
	private int pagarOro = 0;
	private int pagarGemas = 0;

	// Use this for initialization
	public void Start () {

		ShowMenu (CurrentMenu);
	}
	
	public void ShowMenu(Menu menu){
		if (CurrentMenu != null)
			CurrentMenu.IsOpen = false;


			PreviousMenu = CurrentMenu;
			CurrentMenu = menu;
			CurrentMenu.IsOpen = true;

	}

	public void QuitTienda(){
		GameManager.current.newRound ();
		ShowMenu (PreviousMenu);
	}

	public void SeleccionarElemento(int i){
		if (i == Selected) {		//DobleClick -> Comprar
			comprar = true;
			Selected = 0;
		} else {
			Selected = i;
			StartCoroutine(DobleClick());
		}

		//Siempre que seleccionas un item, te guardas su coste en oro/gemas y el item que vas a equiparte en caso de comprarlo.
		//Estos valores se resetean y modifican cada vez q seleccionas un nuevo item
	}

	IEnumerator DobleClick(){
		yield return new WaitForSeconds (0.4f);
		Selected = 0;
	}

	public void GastarGemas(int valor){
		pagarGemas = valor;
		if (comprar) {
			comprar = false;
			//gemas-=valor (if >=0) --> equipar
			equipar = true;
		} 
	}
	public void GastarOro(int valor){
		pagarOro = valor;
		if (comprar) {
			comprar = false;
			//oro -=valor (if >=0) --> equipar
			equipar = true;
		} 
	}

	public void CambiarDisparo(GameObject disparo){
		if (equipar) {
			equipar = false;
			if(disparo != Pool.current.disparo){
				pagarOro = pagarGemas = 0;
				Pool.current.disparo = disparo;
				Pool.current.EquiparDisparos ();
				Debug.Log("Comprado:" + disparo.name);
			}
			else {
				//oro o gemas += pagarOro o pagarGemas; Reembolsar el pago xk el objeto es el mismo!!
			}
		} else {
			//En caso de haber mas tipos de objetos (variables), que no sean disparos, el resto se convierten en null!!!!
			disparoEquipable = disparo;
		}
	}
	public void Comprar(){
		if (disparoEquipable != null) {
			if(disparoEquipable != Pool.current.disparo){
				comprar = true;
				if(pagarOro > 0)
					GastarOro(pagarOro);
				else 
					GastarGemas(pagarGemas);
				CambiarDisparo(disparoEquipable);
				Debug.Log("Comprado desde el boton buy!");
			}
		}
	}

	public void QuitGame(){
		Application.Quit ();
	}

}
