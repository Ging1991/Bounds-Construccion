using System;
using Bounds.Conexiones.Servicios;
using Bounds.Construccion;
using UnityEngine;

namespace Bounds.Contruccion {

	public class Limitador {

		private readonly LectorRestricciones lector;
		private readonly LectorRestricciones.Dato dato;

		public Limitador() {
			lector = new LectorRestricciones();
			dato = lector.Leer();
		}


		public int GetLimite(int cartaID) {
			if (dato.prohibidas.Contains(cartaID))
				return 0;
			if (dato.limitadas.Contains(cartaID))
				return 1;
			if (dato.semilimitadas.Contains(cartaID))
				return 2;
			if (dato.restringidas.Contains(cartaID))
				return 3;
			if (dato.semirestringidas.Contains(cartaID))
				return 4;
			return 5;
		}


		public void ActualizarDatos() {
			if (lector.Leer().fecha != System.DateTime.Today.ToString("yyyy-MM-dd")) {
				Debug.Log("actualizando limites");
				ActualizarDesdeLaNube();
			}
		}


		private async void ActualizarDesdeLaNube() {
			ServicioRestricciones api = new();
			ServicioRestricciones.Salida salida = await api.EjecutarServicio();
			LectorRestricciones.Dato restriccionesLocal = lector.Leer();
			restriccionesLocal.prohibidas = salida.prohibidas;
			restriccionesLocal.limitadas = salida.limitadas;
			restriccionesLocal.semilimitadas = salida.semilimitadas;
			restriccionesLocal.restringidas = salida.restringidas;
			restriccionesLocal.semirestringidas = salida.semirestringidas;
			restriccionesLocal.fecha = DateTime.Today.ToString("yyyy-MM-dd");
			lector.Guardar(restriccionesLocal);
		}

	}

}