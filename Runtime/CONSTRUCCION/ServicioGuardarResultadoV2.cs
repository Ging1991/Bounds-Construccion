using System.Collections.Generic;
using Ging1991.Salesforce;
using Newtonsoft.Json;

namespace Bounds.Salesforce {

	public class ServicioGuardarResultado : SalesforceAPI {

		private static readonly string SERVICIO = "/services/apexrest/ServicioGuardarResultadoV2";

		public ServicioGuardarResultado(Credencial credencial) : base(credencial) { }

		public async void LlamarAsincronica(string jugadorGanador, string jugadorPerdedor, List<int> cartasGanadoras, List<int> cartasPerdedoras) {

			var datos = new Resultado {
				jugadorGanador = jugadorGanador,
				jugadorPerdedor = jugadorPerdedor,
				cartasGanadoras = cartasGanadoras,
				cartasPerdedoras = cartasPerdedoras
			};

			var parametros = JsonConvert.SerializeObject(datos);
			string jsonRespuesta = await CrearSolicitudAsincronica(SERVICIO, parametros);
		}

		[System.Serializable]
		public class Resultado {
			public string jugadorGanador;
			public string jugadorPerdedor;
			public List<int> cartasGanadoras;
			public List<int> cartasPerdedoras;
		}

	}

}