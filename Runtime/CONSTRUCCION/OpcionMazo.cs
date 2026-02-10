using Bounds.Modulos.Cartas.Persistencia;
using Ging1991.Core.Interfaces;
using Ging1991.Relojes;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class OpcionMazo : MonoBehaviour, IEjecutable {

		public LineaRecetaConstruccion linea;
		public GameObject mascaraOBJ;


		public void Iniciar(LineaRecetaConstruccion linea) {
			this.linea = linea;
			SetNivel();
			GetComponentInChildren<MantenerPresionado>().Iniciar(this);
		}


		public void Ejecutar() {
			if (EstaArrastrando()) {
				return;
			}

			GameObject visor = GameObject.Find("visor");
			if (visor != null)
				return;

			ConstructorControl constructor = GameObject.Find("Control").GetComponent<ConstructorControl>();
			constructor.CrearVisor(linea);
		}


		private void SetNivel() {
			Text ui = transform.GetChild(1).GetComponentInChildren<Text>();
			ui.text = "" + DatosDeCartas.Instancia.lector.LeerDatos(linea.cartaID).nivel;
		}


		void OnMouseUp() {

			if (EstaArrastrando()) {
				return;
			}

			if (!ClickeaDentroDeLaMascara(mascaraOBJ))
				return;

			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			GameObject visor = GameObject.Find("visor");
			if (visor != null)
				return;

			ConstructorControl constructor = GameObject.Find("Control").GetComponent<ConstructorControl>();
			constructor.SacarCarta(linea.GetCodigoParcial());
		}


		private bool ClickeaDentroDeLaMascara(GameObject obj) {
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			Canvas canvas = obj.GetComponentInParent<Canvas>();
			Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
			Vector2 localMousePosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, canvasCamera, out localMousePosition);
			bool isWithin = rectTransform.rect.Contains(localMousePosition);
			return isWithin;
		}


		private bool EstaArrastrando() {
			Arrastrable arrastrable1 = GameObject.Find("ContenedorDesplazable").GetComponent<Arrastrable>();
			Arrastrable arrastrable2 = GameObject.Find("BarraDesplazamiento").GetComponent<Arrastrable>();
			return arrastrable1.EstaArrastrando() || arrastrable2.EstaArrastrando();
		}


	}

}