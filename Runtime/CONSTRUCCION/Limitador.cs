using System;
using Bounds.Salesforce;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Salesforce;
using UnityEngine;

namespace Bounds.Contruccion {

	public class Limitador {

		//private readonly LectorRestricciones lector;

		public Limitador() {
			//	lector = new LectorRestricciones();
		}


		public int GetLimite(int cartaID) {/*
			LectorRestricciones.Restricciones dato = lector.Leer();

			if (dato.prohibidas.Contains(cartaID))
				return 0;
			if (dato.limitadas.Contains(cartaID))
				return 1;
			if (dato.semilimitadas.Contains(cartaID))
				return 2;
			if (dato.restringidas.Contains(cartaID))
				return 3;
			if (dato.semirestringidas.Contains(cartaID))
				return 4;*/
			return 5;
		}


		public void ActualizarDatos() {/*
			if (lector.Leer().FechaConvertida != System.DateTime.Today) {
				ActualizarDesdeLaNube();
			}*/
		}


		private async void ActualizarDesdeLaNube() {
			LectorCredenciales lectorX = new LectorCredenciales(new DireccionRecursos("Salesforce", "Credenciales").Generar());
			ServicioGetRestricciones servicio = new(lectorX.Leer());

			if (await servicio.AutorizarAsincronico()) {
				ServicioGetRestricciones.Restricciones restriccionesNube = await servicio.LlamarAsincronica();
				//LectorRestricciones.Restricciones restriccionesLocal = lector.Leer();
				/*
								restriccionesLocal.prohibidas = restriccionesNube.prohibidas;
								restriccionesLocal.limitadas = restriccionesNube.limitadas;
								restriccionesLocal.semilimitadas = restriccionesNube.semilimitadas;
								restriccionesLocal.restringidas = restriccionesNube.restringidas;
								restriccionesLocal.semirestringidas = restriccionesNube.semirestringidas;
								restriccionesLocal.fecha = DateTime.Today.ToString("yyyy-MM-dd");
								lector.Guardar(restriccionesLocal);*/

			}
			else {
				Debug.LogError("No se pudieron traer los datos de restricciones.");
			}
		}

	}

}