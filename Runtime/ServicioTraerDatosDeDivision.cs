using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Ging1991.Salesforce;

namespace Bounds.Salesforce {

	public class ServicioTraerDatosDeDivision : SalesforceAPI {

		private static readonly string SERVICIO = "/services/apexrest/ServicioGetPuntuaciones";

		public ServicioTraerDatosDeDivision(Credencial credencial) : base(credencial) { }


		public async Task<Puntuacion> LlamarAsincronica(string nombreJugador) {
			var parametros = "{\"nombreJugador\" : \"" + nombreJugador + "\"}";
			string jsonRespuesta = await CrearSolicitudAsincronica(SERVICIO, parametros);

			if (string.IsNullOrEmpty(jsonRespuesta))
				return null;

			return JsonUtility.FromJson<Puntuacion>(jsonRespuesta);
		}


		[System.Serializable]
		public class Puntuacion {
			public string nombre;
			public string division;
			public int victorias;
			public int derrotas;
			public List<string> oponentes;
		}


	}

}