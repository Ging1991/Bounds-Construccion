using System.Collections.Generic;
using Bounds.Contruccion;
using Ging1991.Idiomas;
using UnityEngine;

namespace Bounds.Construccion {

	public class TraductorConstruccion : MonoBehaviour, ITraductorEspecial {

		public VisorConstruccion visorConstruccion;
		public bool inicializado = false;

		public string Traducir(string clave, string traduccionParcial, List<string> opciones) {
			if (!inicializado)
				return traduccionParcial;
			if (clave == "VENDER_PRECIO")
				return traduccionParcial.Replace("[PRECIO]", $"{visorConstruccion.CalcularPrecioActual()}");
			if (clave == "PAGINA [ACTUAL]/[MAXIMO]") {
				return Paginador.Instancia.TextoVisorPagina();
			}
			return traduccionParcial;
		}

	}

}