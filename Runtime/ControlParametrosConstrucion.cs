using Bounds.Sistema.Parametros;
using Ging1991.Persistencia.Direcciones;

namespace Bounds.Limitado {

	public class ControlParametrosConstrucion : ControlParametros {

		public override void SetParametros() {

			parametros.direcciones["MUSICA_AMBIENTAL"] = new DireccionRecursos("Sonidos/Ambiente");

			parametros.escenaAnterior = "TEST";
			parametros.direccionesGeneradas["COLORES"] = new DireccionRecursos("Configuracion", "COLORES").Generar();
			parametros.direccionesGeneradas["SISTEMA"] = new DireccionRecursos("Configuracion", "IDIOMA").Generar();
			parametros.direccionesGeneradas["MUSICA_DERROTA"] = new DireccionRecursos("Musica", "DERROTA").Generar();
			parametros.direccionesGeneradas["MUSICA_VICTORIA"] = new DireccionRecursos("Musica", "VICTORIA").Generar();
			parametros.direccionesGeneradas["CARTAS_HABILIDADES"] = new DireccionRecursos("HABILIDADES", "HABILIDADES").Generar();
			parametros.direccionesGeneradas["PERSONAJES_NOMBRES"] = new DireccionRecursos("PERSONAJES", "NOMBRES").Generar();
			parametros.direccionesGeneradas["ENTRENAMIENTO_MAZOS"] = "MAZOS/ENTRENAMIENTO";
			parametros.direccionesGeneradas["SONIDOS"] = "Sonidos";
			parametros.direccionesGeneradas["HISTORIA"] = "HISTORIA";
			parametros.direccionesGeneradas["TUTORIAL"] = "TUTORIAL";
			parametros.direccionesGeneradas["CARTAS_DATOS"] = "Cartas/Datos";
			parametros.direccionesGeneradas["PERSONAJES_MINIATURA"] = "PERSONAJES/MINIATURAS";
			parametros.direccionesGeneradas["ENTRENAMIENTO"] = new DireccionDinamica("ENTRENAMIENTO", "ENTRENAMIENTO.json").Generar();
			parametros.direccionesGeneradas["SOBRES"] = new DireccionDinamica("TIENDA", "SOBRES.json").Generar();
			parametros.direccionesGeneradas["CONFIGURACION"] = new DireccionDinamica("CONFIGURACION", "CONFIGURACION.json").Generar();
			parametros.direccionesGeneradas["BILLETERA"] = new DireccionDinamica("CONFIGURACION", "BILLETERA.json").Generar();
			parametros.direccionesGeneradas["COLECCIONES"] = "COLECCIONES";
			parametros.direccionesGeneradas["CARTA_NOMBRES"] = new DireccionRecursos("Cartas", "Nombres").Generar();
			parametros.direccionesGeneradas["CARTA_NOMBRES"] = new DireccionRecursos("Cartas", "Nombres").Generar();
			parametros.direccionesGeneradas["CARTA_EFECTOS"] = new DireccionRecursos("Cartas", "Efectos").Generar();
			parametros.direccionesGeneradas["CARTA_AMBIENTACION"] = new DireccionRecursos("Cartas", "Ambientacion").Generar();
			parametros.direccionesGeneradas["COFRE_RECURSOS"] = new DireccionRecursos("MAZOS", "COFRE").Generar();
			parametros.direccionesGeneradas["COFRE"] = new DireccionDinamica("CONFIGURACION", "COFRE.json").Generar();
			parametros.direccionesGeneradas["CARTA_CLASES"] = new DireccionRecursos("Cartas", "Clases").Generar();
			parametros.direccionesGeneradas["CARTA_TIPOS"] = new DireccionRecursos("Cartas", "Tipos").Generar();
			parametros.direccionesGeneradas["CARTA_INVOCACIONES"] = new DireccionRecursos("Cartas", "Invocaciones").Generar();
			parametros.direccionesGeneradas["CARTAS_RECURSO"] = "Cartas/Imagenes";
			parametros.direccionesGeneradas["CARTAS_DINAMICA"] = "IMAGENES/Cartas/Imagenes";
		}

	}

}