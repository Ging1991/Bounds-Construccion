using Bounds.Cofres;

namespace Bounds.Contruccion {

	public class LineaRecetaConstruccion : LineaReceta {

		public int limite;
		public int cantidadEnCofre;
		public int cantidadEnMazo;

		public LineaRecetaConstruccion(string codigo, int limite, int cantidadEnMazo) : base(codigo) {
			this.limite = limite;
			this.cantidadEnMazo = cantidadEnMazo;
			cantidadEnCofre = cantidad;
		}


		public string GetCodigoMazo() {
			return $"{GetCartaIDFormateada()}_{imagen}_{rareza}_{cantidadEnMazo}";
		}


	}

}