using Bounds.Cofres;
using Bounds.Global.Mazos;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Modulos.Visor;
using Bounds.Persistencia;
using Ging1991.Core;
using Ging1991.Interfaces.Selecciones;
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
			Destroy(gameObject);
		}


		public void Mostrar(LineaRecetaConstruccion linea, Billetera billetera, Cofre cofre) {
			this.billetera = billetera;
			this.cofre = cofre;
			lineaActual = linea;
			textoBoton.text = $"Vender por ${CalcularPrecio()}";
			Bloqueador.BloquearGrupo("GLOBAL", true);
			GetComponentInChildren<VisorGeneral>().Mostrar(linea.cartaID, linea.imagen, linea.rareza);
			InicializarVacio(linea);
			InicializarPrincipal(linea);
		}


		private void InicializarVacio(LineaRecetaConstruccion linea) {
			CartaBD carta = DatosDeCartas.Instancia.lector.LeerDatos(linea.cartaID);
			casillaVacio.gameObject.SetActive(carta.clase == "VACIO");
			casillaVacio.AgregarObservador(this);
			if (FindAnyObjectByType<ConstructorControl>().vacioPrinpal != null) {
				if (FindAnyObjectByType<ConstructorControl>().vacioPrinpal.cartaID == linea.cartaID) {
					casillaVacio.Presionar();
				}
			}
		}


		private void InicializarPrincipal(LineaRecetaConstruccion linea) {
			casillaPrincipal.AgregarObservador(this);
			if (ConstructorControl.Instancia.cartaPrinpal != null) {
				if (ConstructorControl.Instancia.cartaPrinpal.cartaID == linea.cartaID) {
					casillaPrincipal.Presionar();
				}
			}
		}


		public static bool VisorActivo() {
			GameObject visor = GameObject.Find("visor");
			return visor != null;
		}


		public void AlternadorPresionado(Alternador alternador) {

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