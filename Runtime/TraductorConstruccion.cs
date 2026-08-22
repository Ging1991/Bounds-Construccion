using Bounds.Contruccion;
using Ging1991.Idiomas;
using UnityEngine;

namespace Bounds.Construccion {

	public class TraductorConstruccion : MonoBehaviour, ITraductorEspecial {

		public VisorConstruccion visorConstruccion;

		public string Traducir(string clave, string traduccionParcial) {
			if (clave == "VENDER_PRECIO")
				return traduccionParcial.Replace("[PRECIO]", $"{visorConstruccion.CalcularPrecioActual()}");
			throw new System.NotImplementedException();
		}

	}

}