using System.Collections.Generic;
using Bounds.Cofres;
using Bounds.Global;
using Bounds.Global.Mazos;
using Bounds.Persistencia;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class Recetario : MonoBehaviour {

		private List<LineaRecetaConstruccion> cartas;
		private Limitador limitador;
		private Cofre cofre;
		private Mazo mazo;


		public void Iniciar(string codigo) {

			limitador = new Limitador();
			cofre = new Cofre();
			mazo = new MazoJugador(codigo);

			CargarCartas();

			FindAnyObjectByType<ConstructorControl>().vacioPrinpal = mazo.principalVacio;
			FindAnyObjectByType<ConstructorControl>().cartaPrinpal = mazo.emblema;

			GameObject entrada = GameObject.Find("EntradaNombreMazo");
			entrada.GetComponentInChildren<InputField>().text = mazo.nombre;
			FindAnyObjectByType<ConstructorControl>().nombreMazo = mazo.nombre;
		}


		public List<LineaRecetaConstruccion> GetCartas() {
			return cartas;
		}


		public List<LineaRecetaConstruccion> GetCartasEnMazo() {
			List<LineaRecetaConstruccion> cartasEnMazo = new List<LineaRecetaConstruccion>();
			foreach (var carta in cartas) {
				if (carta.cantidadEnMazo > 0)
					cartasEnMazo.Add(carta);
			}
			return cartasEnMazo;
		}


		public int GetCantidadEnMazo() {
			int cantidad = 0;
			foreach (var carta in GetCartasEnMazo()) {
				cantidad += carta.cantidadEnMazo;
			}
			return cantidad;
		}


		public int GetCantidadEnCofre() {
			int cantidad = 0;
			foreach (var carta in GetCartas()) {
				cantidad += carta.cantidadEnCofre;
			}
			return cantidad;
		}


		private void CargarCartas() {
			cartas = new List<LineaRecetaConstruccion>();

			foreach (LineaReceta linea in cofre.GetCartas()) {
				cartas.Add(new LineaRecetaConstruccion(
					linea.GetCodigo(),
					limitador.GetLimite(linea.cartaID),
					CantidadEnMazo(linea.GetCodigoParcial())
				));
			}

			cartas.Sort(delegate (LineaRecetaConstruccion carta1, LineaRecetaConstruccion carta2) {
				if (carta1.cartaID == carta2.cartaID)
					return 0;
				else if (carta1.cartaID < carta2.cartaID)
					return -1;
				else
					return 1;
			});

		}


		public int CantidadEnMazo(string codigoParcial) {
			foreach (CartaMazo carta in mazo.cartas) {
				LineaReceta linea = new LineaReceta(carta.GetCodigo());
				if (linea.GetCodigoParcial() == codigoParcial) {
					return linea.cantidad;
				}
			}
			return 0;
		}


		public int CantidadEnMazoActual(int cartaID) {
			int cantidad = 0;
			foreach (LineaRecetaConstruccion linea in GetCartasEnMazo()) {
				if (linea.cartaID == cartaID) {
					cantidad += linea.cantidadEnMazo;
				}
			}
			return cantidad;
		}


	}


}