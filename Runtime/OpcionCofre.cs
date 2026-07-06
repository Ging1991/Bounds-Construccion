using UnityEngine;
using Ging1991.Core.Interfaces;
using Ging1991.Interfaces.Salida;
using Bounds.Cartas;

namespace Bounds.Contruccion {

	public class OpcionCofre : MonoBehaviour, IEjecutable {

		private LineaRecetaConstruccion linea;
		private ISeleccionarCartaID padre;
		public GameObject contadorOBJ;
		public CartaImagenID cartaImagenID;

		public void Iniciar(LineaRecetaConstruccion linea, ISeleccionarCartaID padre, int limite, CartaGenerador cartaGenerador) {
			this.linea = linea;
			this.padre = padre;
			cartaImagenID.generador = cartaGenerador;
			cartaImagenID.MostrarCartaID(linea.cartaID, linea.imagen, linea.rareza);
			GetComponentInChildren<MantenerPresionado>().Iniciar(this);

			Indicador contador = GetComponentInChildren<Indicador>();
			Color colorContador = cartaGenerador.proveedorColores.GetElemento($"NIVEL_{linea.rareza}");
			contador.SetValor(colorContador, linea.cantidadEnMazo, linea.cantidadEnCofre, linea.limite);
			DefinirRestricciones(linea.cartaID, limite);
		}


		private void DefinirRestricciones(int cartaID, int limite) {
			contadorOBJ.GetComponent<ContadorNumero>().SetValor(limite);
			contadorOBJ.SetActive(true);

			if (limite == 0) {
				contadorOBJ.GetComponent<ContadorNumero>().SetColorTexto(Color.white);
				contadorOBJ.GetComponent<ContadorNumero>().SetColorRelleno(Color.red);

			}
			else if (limite == 1 || limite == 2) {
				contadorOBJ.GetComponent<ContadorNumero>().SetColorTexto(Color.white);
				contadorOBJ.GetComponent<ContadorNumero>().SetColorRelleno(Color.magenta);

			}
			else if (limite == 3 || limite == 4) {
				contadorOBJ.GetComponent<ContadorNumero>().SetColorTexto(Color.black);
				contadorOBJ.GetComponent<ContadorNumero>().SetColorRelleno(Color.yellow);
			}
			else {
				contadorOBJ.SetActive(false);
			}
		}


		public void Ejecutar() {
			GameObject visor = GameObject.Find("visor");
			if (visor != null)
				return;

			ConstructorControl constructor = GameObject.Find("Control").GetComponent<ConstructorControl>();
			constructor.CrearVisor(linea);
		}


		void OnMouseUp() {
			if (PuedePresionar()) {
				padre.SeleccionarCartaID(linea.GetCodigoIndividual());
			}

		}


		private bool PuedePresionar() {
			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return false;

			GameObject visor = GameObject.Find("visor");
			if (visor != null)
				return false;

			if (!GetComponentInChildren<MantenerPresionado>().estaPresionado)
				return false;

			return true;
		}


	}

}