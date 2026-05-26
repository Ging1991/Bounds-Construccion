using System;
using Bounds.Construccion;
using Bounds.Salesforce;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Salesforce;
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
			LectorCredenciales lectorCredenciales = new LectorCredenciales(new DireccionRecursos("Salesforce", "Credenciales").Generar());
			ServicioGetRestricciones servicio = new(lectorCredenciales.Leer());

			if (await servicio.AutorizarAsincronico()) {
				ServicioGetRestricciones.Restricciones restriccionesNube = await servicio.LlamarAsincronica();
				LectorRestricciones.Dato restriccionesLocal = lector.Leer();
				restriccionesLocal.prohibidas = restriccionesNube.prohibidas;
				restriccionesLocal.limitadas = restriccionesNube.limitadas;
				restriccionesLocal.semilimitadas = restriccionesNube.semilimitadas;
				restriccionesLocal.restringidas = restriccionesNube.restringidas;
				restriccionesLocal.semirestringidas = restriccionesNube.semirestringidas;
				restriccionesLocal.fecha = DateTime.Today.ToString("yyyy-MM-dd");
				lector.Guardar(restriccionesLocal);

			}
			else {
				Debug.LogError("No se pudieron traer los datos de restricciones.");
			}
		}

	}

}