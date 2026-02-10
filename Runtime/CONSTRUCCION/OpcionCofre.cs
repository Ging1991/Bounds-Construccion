using UnityEngine;
using Bounds.Modulos.Cartas.Tinteros;
using Bounds.Modulos.Cartas;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Ilustradores;
using Ging1991.Core.Interfaces;
using Ging1991.Interfaces.Contadores;

namespace Bounds.Contruccion {

	public class OpcionCofre : MonoBehaviour, IEjecutable {

		private LineaRecetaConstruccion linea;
		private ISeleccionarCartaID padre;
		public GameObject contadorOBJ;

		public void Iniciar(LineaRecetaConstruccion linea, ISeleccionarCartaID padre, int limite, ITintero tintero, IlustradorDeCartas ilustrador) {
			this.linea = linea;
			this.padre = padre;
			GetComponentInChildren<CartaFrente>().Inicializar(DatosDeCartas.Instancia, ilustrador, tintero);
			GetComponentInChildren<CartaFrente>().Mostrar(linea.cartaID, linea.imagen, linea.rareza);
			GetComponentInChildren<MantenerPresionado>().Iniciar(this);

			ContadorSimbolo contador = GetComponentInChildren<ContadorSimbolo>();
			Color colorContador = tintero.GetColor($"NIVEL_{linea.rareza}");
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
				padre.SeleccionarCartaID(linea.GetCodigoParcial());
			}

		}


		private bool PuedePresionar() {
			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return false;

			GameObject visor = GameObject.Find("visor");
			if (visor != null)
				return false;

			return true;
		}


	}

}