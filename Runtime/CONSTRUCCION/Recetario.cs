using System.Collections.Generic;
using Bounds.Cofres;
using Bounds.Global;
using Bounds.Global.Mazos;
using Bounds.Persistencia;
using Ging1991.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class Recetario : SingletonMonoBehaviour<Recetario> {

		private List<LineaRecetaConstruccion> cartas;
		private Limitador limitador;
		private Cofre cofre;
		private Mazo mazo;


		public void Iniciar(string codigo) {

			limitador = new Limitador();
			cofre = ConstructorControl.Instancia.cofre;
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


		public void CargarCartas() {
			cofre = ConstructorControl.Instancia.cofre;
			cartas = new List<LineaRecetaConstruccion>();

			foreach (CartaCofreBD linea in cofre.GetCartas()) {
				cartas.Add(new LineaRecetaConstruccion(
					linea.GetCodigo(),
					limitador.GetLimite(linea.cartaID),
					CantidadEnMazo(linea.GetCodigoIndividual())
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
				CartaCofreBD linea = new(carta.GetCodigo());
				if (linea.GetCodigoIndividual() == codigoParcial) {
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