using Bounds.Cofres;
using Bounds.Mazos;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Sistema;
using Bounds.Visor;
using Ging1991.Core;
using Ging1991.Interfaces.Entrada;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class VisorConstruccion : MonoBehaviour, IAlternadorObservador {

		private LineaRecetaConstruccion lineaActual;
		public OpcionBinaria casillaVacio;
		public OpcionBinaria casillaPrincipal;
		private Cofre cofre;
		private Billetera billetera;
		public Text textoBoton;
		public VisorCartaID visorCartaID;
		public bool soloVisual = false;

		private int CalcularPrecio() {
			int rareza = 1;
			if (lineaActual.rareza == "PLA")
				rareza = 10;
			if (lineaActual.rareza == "ORO")
				rareza = 100;
			if (lineaActual.rareza == "MIT")
				rareza = 1000;
			if (lineaActual.rareza == "SEC")
				rareza = 10000;
			if (lineaActual.rareza == "LEG")
				rareza = 100000;
			return rareza * lineaActual.cantidad;
		}

		public void BotonVender() {
			int precio = CalcularPrecio();
			billetera.GanarOro(precio);
			cofre.RemoverCarta(lineaActual);
			cofre.Guardar();
			Recetario.Instancia.CargarCartas();
			Paginador.Instancia.Actualizar();
			BotonCerrar();
		}

		public void BotonCerrar() {
			Bloqueador.BloquearGrupo("GLOBAL", false);
			gameObject.SetActive(false);
		}


		public void Inicializar(Billetera billetera, Cofre cofre, VisorGenerador visorGenerador) {
			this.billetera = billetera;
			this.cofre = cofre;
			visorCartaID.generador = visorGenerador;
			casillaPrincipal.AgregarObservador(this);
			casillaVacio.AgregarObservador(this);
		}

		public void Mostrar(LineaRecetaConstruccion linea) {
			lineaActual = linea;
			textoBoton.text = $"Vender por ${CalcularPrecio()}";
			Bloqueador.BloquearGrupo("GLOBAL", true);
			visorCartaID.Mostrar(linea.cartaID, linea.imagen, linea.rareza);
			SetVacio(linea);
			SetPrincipal(linea);
		}


		private void SetVacio(LineaRecetaConstruccion linea) {
			CartaBD carta = ConstructorControl.Instancia.proveedorCartas.GetElemento(linea.cartaID);
			casillaVacio.gameObject.SetActive(carta.clase == "VACIO");
			if (ConstructorControl.Instancia.vacioPrinpal != null) {
				SetValor(casillaVacio, ConstructorControl.Instancia.vacioPrinpal.cartaID == linea.cartaID);
			}
		}


		private void SetPrincipal(LineaRecetaConstruccion linea) {
			if (ConstructorControl.Instancia.cartaPrinpal != null) {
				SetValor(casillaPrincipal, ConstructorControl.Instancia.cartaPrinpal.cartaID == linea.cartaID);
			}
		}

		private void SetValor(OpcionBinaria casilla, bool valor) {
			if (casilla.valor != valor) {
				soloVisual = true;
				casilla.Presionar();
			}
		}


		public static bool VisorActivo() {
			GameObject visor = GameObject.Find("visor");
			return visor != null;
		}


		public void AlternadorPresionado(Alternador alternador) {
			if (soloVisual) {
				soloVisual = false;
				return;
			}

			ConstructorControl control = FindAnyObjectByType<ConstructorControl>();
			CartaMazo cartaMazo = new CartaMazo(lineaActual.GetCodigo());

			if (alternador.codigo == "VACIO_PRINCIPAL") {
				control.vacioPrinpal = (alternador.valor == true) ? cartaMazo : null;
			}
			if (alternador.codigo == "CARTA_PRINCIPAL") {
				control.cartaPrinpal = (alternador.valor == true) ? cartaMazo : null;
			}
		}


	}

}