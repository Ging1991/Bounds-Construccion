using Ging1991.Core.Interfaces;
using Ging1991.Interfaces.Personalizacion;
using Ging1991.Interfaces.Salida;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class CartaEnMazo : MonoBehaviour, IEjecutable {

		public LineaRecetaConstruccion linea;
		public GameObject mascaraOBJ;
		public TextoUI nombreOBJ;
		public ContadorNumero nivelOBJ;
		public Indicador cantidadOBJ;
		public Image separador;
		public Marco marco;

		public void Iniciar(LineaRecetaConstruccion linea) {
			this.linea = linea;
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


		public void SetNivel(int nivel, Color tinta, Color fondo) {
			nivelOBJ.SetValor(nivel);
			nivelOBJ.SetColorBorde(tinta);
			nivelOBJ.SetColorTexto(tinta);
			nivelOBJ.SetColorRelleno(fondo);
		}

		public void SetCantidad(int cantidad, int maximoPosible, int maximoHabilitado, Color color) {
			cantidadOBJ.SetValor(color, cantidad, maximoPosible, maximoHabilitado);
		}

		public void SetNombre(string nombre, Color tinta) {
			nombreOBJ.SetTexto(nombre);
			nombreOBJ.SetColor(tinta);
			separador.color = tinta;
		}

		public void SetFondo(Color borde, Color relleno) {
			marco.SetColorBorde(borde);
			marco.SetColorRelleno(relleno);
		}

		void OnMouseUp() {

			if (EstaArrastrando()) {
				return;
			}

			if (!GetComponentInChildren<MantenerPresionado>().estaPresionado)
				return;


			if (!ClickeaDentroDeLaMascara(mascaraOBJ))
				return;

			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			GameObject visor = GameObject.Find("visor");
			if (visor != null)
				return;

			ConstructorControl constructor = GameObject.Find("Control").GetComponent<ConstructorControl>();
			constructor.SacarCarta(linea.GetCodigoIndividual());
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