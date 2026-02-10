using Bounds.Cofres;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class InstanciadorConstruir : MonoBehaviour {

		public GameObject claseOpcionMazo;
		public GameObject claseCuadroAceptar;
		public Sprite imgFondo;
		public GameObject mascaraOBJ;


		public GameObject CrearOpcionMazo(LineaRecetaConstruccion linea, string texto, Vector3 posicion) {
			GameObject panel = GameObject.Find("ContenidoMazo");
			GameObject opcion = Instantiate(claseOpcionMazo, posicion, Quaternion.identity);
			opcion.transform.SetParent(panel.transform);
			opcion.GetComponent<RectTransform>().localPosition = posicion;
			opcion.transform.localScale = new Vector3(1, 1, 1);
			opcion.transform.localPosition = posicion;
			OpcionMazo opcionMazo = opcion.GetComponentInChildren<OpcionMazo>();
			opcionMazo.Iniciar(linea);
			opcionMazo.mascaraOBJ = mascaraOBJ;
			opcion.GetComponentInChildren<Text>().text = texto;
			return opcion;
		}


		public GameObject CrearCuadroAceptar(Vector3 posicion, string texto) {
			GameObject cuadro = null;
			if (!CuadroAceptar.existenCuadros()) {
				cuadro = Instantiate(claseCuadroAceptar, posicion, Quaternion.identity);
				GameObject lienzo = GameObject.Find("Lienzo");
				cuadro.transform.SetParent(lienzo.transform);
				cuadro.transform.localScale = new Vector3(1,1,1);
				cuadro.transform.localPosition = new Vector3(0,0,0);
				cuadro.GetComponent<CuadroAceptar>().iniciar(texto);
			}
			return cuadro;
		}


	}

}