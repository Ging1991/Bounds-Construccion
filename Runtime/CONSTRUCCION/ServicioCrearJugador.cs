using UnityEngine;
using System.Threading.Tasks;
using Ging1991.Salesforce;

namespace Bounds.Salesforce {

	public class ServicioCrearJugador : SalesforceAPI {

		private static readonly string SERVICIO = "/services/apexrest/ServicioCrearJugador";

		public ServicioCrearJugador(Credencial credencial) : base(credencial) { }

		public async Task<bool> LlamarAsincronica(string nombre) {
			var parametros = "{\"nombre\" : \"" + nombre + "\"}";
			string jsonRespuesta = await CrearSolicitudAsincronica(SERVICIO, parametros);

			if (string.IsNullOrEmpty(jsonRespuesta))
				return false;
			return JsonUtility.FromJson<Respuesta>(jsonRespuesta).jugadorCreado;
		}


		[System.Serializable]
		public class Respuesta {
			public bool jugadorCreado;
		}

	}

}