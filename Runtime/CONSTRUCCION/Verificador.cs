using System;
using System.Collections.Generic;
using Bounds.Global.Mazos;
using Bounds.Infraestructura;
using Bounds.Modulos.Cartas.Persistencia;
using Ging1991.Ventanas;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bounds.Contruccion {

	public class Verificador : MonoBehaviour {

		public DatosDeCartas datosDeCartas;

		public void BotonGuardar() {
			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			int cantidadCartasEnMazo = 0;
			List<LineaRecetaConstruccion> cartasEnMazo = FindAnyObjectByType<Recetario>().GetCartasEnMazo();
			foreach (LineaRecetaConstruccion cartaCofre in cartasEnMazo) {
				cantidadCartasEnMazo += cartaCofre.cantidadEnMazo;
			}

			List<CartaMazo> cartasID = new List<CartaMazo>();
			foreach (LineaRecetaConstruccion linea in cartasEnMazo) {
				cartasID.Add(new CartaMazo(linea.GetCodigoMazo()));
			}

			if (cantidadCartasEnMazo != Legal.CARTAS_MAX_EN_MAZO) {
				MostrarMensajeError("Debes tener exactamente 40 cartas en el mazo, ahora tienes " + cantidadCartasEnMazo);
				return;
			}

			if (GetNivelPromedio() > 4) {
				MostrarMensajeError("El promedio de niveles en el mazo no puede ser mayor a 4, ahora tienes " + GetNivelPromedio());
				return;
			}

			string prohibiciones = CalcularProhibiciones();
			if (prohibiciones != "") {
				MostrarMensajeError(prohibiciones);
				return;
			}

			try {
				MazoJugador mazoJugador = new MazoJugador(MazoJugador.GetPredeterminado());
				ConstructorControl constructor = FindAnyObjectByType<ConstructorControl>();
				mazoJugador.nombre = constructor.nombreMazo;
				mazoJugador.principalVacio = constructor.vacioPrinpal;
				mazoJugador.emblema = constructor.cartaPrinpal;
				mazoJugador.cartas = cartasID;
				mazoJugador.Guardar();
				SceneManager.LoadScene("CONSTRUCCION SELECCION");

			}
			catch (InvalidOperationException ex) {
				Debug.Log(ex.ToString());
			}
		}


		public string CalcularProhibiciones() {
			string ret = "";
			List<LineaRecetaConstruccion> cartas = FindAnyObjectByType<Recetario>().GetCartasEnMazo();
			foreach (LineaRecetaConstruccion carta in cartas) {
				ret += VerificarCantidades(carta);
			}

			if (ret != "")
				ret = ret.Substring(1);
			return ret;
		}


		private string VerificarCantidades(LineaRecetaConstruccion carta) {
			if (carta.cantidadEnMazo > carta.limite)
				return $"\nLímite {carta.limite}: {datosDeCartas.lector.LeerDatos(carta.cartaID).nombre}[{carta.cantidad}]";
			return "";
		}


		public float GetNivelPromedio() {
			List<LineaRecetaConstruccion> cartas = FindAnyObjectByType<Recetario>().GetCartasEnMazo();
			int cantidad = 0;
			int suma = 0;
			foreach (LineaRecetaConstruccion carta in cartas) {
				cantidad += carta.cantidad;
				suma += carta.cantidad * datosDeCartas.lector.LeerDatos(carta.cartaID).nivel;
			}

			if (cantidad > 0) {
				float promedio = (float)suma / cantidad;
				return (float)Math.Round(promedio, 1);
			}
			else {
				return 0.0f;
			}
			;
		}


		public void MostrarMensajeError(string mensaje) {
			VentanaControl.CrearVentanaAceptar(mensaje);
		}


	}

}