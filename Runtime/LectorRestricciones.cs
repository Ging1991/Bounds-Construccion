using System;
using System.Collections.Generic;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;

namespace Bounds.Construccion {

	public class LectorRestricciones : LectorGenerico<LectorRestricciones.Dato> {

		public LectorRestricciones() : base(new DireccionDinamica("CONFIGURACION", "LIMITE.json").Generar(), TipoLector.DINAMICO) {
			if (ExistenDatos() == false) {
				Guardar(new Dato());
			}
		}

		public class Dato {
			public List<int> prohibidas;
			public List<int> limitadas;
			public List<int> semilimitadas;
			public List<int> restringidas;
			public List<int> semirestringidas;
			public string fecha;
		}

	}

}