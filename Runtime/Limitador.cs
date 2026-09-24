using System;
using Bounds.Conexiones;
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
			ConexionRestricciones conexionRestricciones = new ConexionRestricciones();
			ConexionRestricciones.Salida restriccionesNube = await conexionRestricciones.EjecutarAsync();
			LectorRestricciones.Dato restriccionesLocal = lector.Leer();
			restriccionesLocal.prohibidas = restriccionesNube.prohibidas;
			restriccionesLocal.limitadas = restriccionesNube.limitadas;
			restriccionesLocal.semilimitadas = restriccionesNube.semilimitadas;
			restriccionesLocal.restringidas = restriccionesNube.restringidas;
			restriccionesLocal.semirestringidas = restriccionesNube.semirestringidas;
			restriccionesLocal.fecha = DateTime.Today.ToString("yyyy-MM-dd");
			lector.Guardar(restriccionesLocal);
		}

	}

}