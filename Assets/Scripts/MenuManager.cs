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
		Time.timeScale = 1;
		ShowMenu (PreviousMenu);
	}

	public void SeleccionarElemento(int i){
		if (i == Selected) {		//DobleClick -> Comprar
			comprar = true;
			Selected = 0;
		} else 
			Selected = i;

		//Siempre que seleccionas un item, te guardas su coste en oro/gemas y el item que vas a equiparte en caso de comprarlo.
		//Estos valores se resetean y modifican cada vez q seleccionas un nuevo item
	}

	public void GastarGemas(int valor){
		if (comprar) {
			comprar = false;
			//gemas-=valor (if >=0) --> equipar
			equipar = true;
		} else {
			pagarGemas = valor;
		}
	}
	public void GastarOro(int valor){
		if (comprar) {
			comprar = false;
			//oro -=valor (if >=0) --> equipar
			equipar = true;
		} else {
			pagarOro = valor;
		}
	}

	public void CambiarDisparo(GameObject disparo){
		if (equipar) {
			equipar = false;
			Pool.current.disparo = disparo;
			Pool.current.EquiparDisparos ();
		} else {
			//En caso de haber mas tipos de objetos, el resto se convierten en null!!!!
			disparoEquipable = disparo;
		}
	}
	public void Comprar(){
		if (disparoEquipable != null) {
			comprar = true;
			if(pagarOro > 0)
				GastarOro(pagarOro);
			else 
				GastarGemas(pagarGemas);
			CambiarDisparo(disparoEquipable);
		}
	}

	public void QuitGame(){
		Application.Quit ();
	}

}
