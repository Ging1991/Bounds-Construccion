using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ging1991.Salesforce;

namespace Bounds.Salesforce {

	public class ServicioEncontrarOponente : SalesforceAPI {

		private static readonly string SERVICIO = "/services/apexrest/ServicioEncontrarOponente";

		public ServicioEncontrarOponente(Credencial credencial) : base(credencial) { }


		public async Task<Oponente> LlamarAsincronica(string nombreJugador) {
			var parametros = "{\"nombreJugador\" : \"" + nombreJugador + "\"}";
			string jsonRespuesta = await CrearSolicitudAsincronica(SERVICIO, parametros);

			if (string.IsNullOrEmpty(jsonRespuesta))
				return null;

			return JsonUtility.FromJson<Oponente>(jsonRespuesta);
		}


		[System.Serializable]
		public class Oponente {
			public string nombre;
			public string avatar;
			public string nombreMazo;
			public int vacio;
			public List<int> cartas;
		}


	}

}