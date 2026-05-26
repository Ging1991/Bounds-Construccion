using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Ging1991.Salesforce;

namespace Bounds.Salesforce {

	public class ServicioGetRestricciones : SalesforceAPI {

		private static readonly string SERVICIO = "/services/apexrest/ServicioGetRestricciones";

		public ServicioGetRestricciones(Credencial credencial) : base(credencial) { }

		public async Task<Restricciones> LlamarAsincronica() {
			var parametros = "{}";
			string jsonRespuesta = await CrearSolicitudAsincronica(SERVICIO, parametros);

			if (string.IsNullOrEmpty(jsonRespuesta))
				return null;
			return JsonUtility.FromJson<Restricciones>(jsonRespuesta);
		}


		[System.Serializable]
		public class Restricciones {
			public List<int> prohibidas;
			public List<int> limitadas;
			public List<int> semilimitadas;
			public List<int> restringidas;
			public List<int> semirestringidas;
		}

	}

}