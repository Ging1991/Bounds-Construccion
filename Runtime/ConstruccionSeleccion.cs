using Bounds.Cartas;
using Bounds.Infraestructura;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Persistencia.proveedores;
using Bounds.Sistema;
using Bounds.Sistema.Ilustradores;
using Bounds.Sistema.Parametros;
using Ging1991.Core.Interfaces;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;
using Ging1991.Persistencia.Lectores.Directos;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bounds.Contruccion {

	public class ConstruccionSeleccion : MonoBehaviour {
		public ControlBounds controlBounds;
		private ParametrosGlobales parametros;
		public IProveedor<int, CartaBD> proveedorCartas;
		public CartaGenerador cartaGenerador;

		void Start() {
			parametros = controlBounds.InicializarEscena("GENERAL");

			IProveedor<string, Sprite> ilustradorDeCartas = new IlustradorDeCartas(
				new DireccionRecursos(parametros.direccionesGeneradas["CARTAS_RECURSO"]),
				new DireccionDinamica(parametros.direccionesGeneradas["CARTAS_DINAMICA"])
			);

			proveedorCartas = new LectorCartas(new DireccionRecursos(parametros.direccionesGeneradas["CARTAS_DATOS"]));

			cartaGenerador.Inicializar(
				ilustradorDeCartas,
				proveedorCartas,
				new ProveedorColores(
					parametros.direccionesGeneradas["COLORES"],
					TipoLector.RECURSOS
				)
			);

			GameObject[] mazos = GameObject.FindGameObjectsWithTag("mazo");
			foreach (GameObject mazo in mazos) {
				mazo.GetComponent<OpcionMazoConstruccion>().Inicializar(cartaGenerador);
			}

		}

		public void DeseleccionarTodo() {
			GameObject[] mazos = GameObject.FindGameObjectsWithTag("mazo");
			foreach (GameObject mazo in mazos) {
				mazo.GetComponent<OpcionMazoConstruccion>().Deseleccionar();
			}

		}


		public void Predeterminar() {
			Direccion direccion = new DireccionDinamica("MAZOS", "PREDETERMINADO.json");
			LectorCadena lectorCadena = new LectorCadena(direccion.Generar(), TipoLector.DINAMICO);
			GameObject[] mazos = GameObject.FindGameObjectsWithTag("mazo");
			OpcionMazoConstruccion opcion = TraerSeleccionado();
			if (opcion != null) {
				lectorCadena.Guardar(opcion.nombre);
				foreach (GameObject mazo in mazos) {
					mazo.GetComponent<OpcionMazoConstruccion>().MostrarPredeterminado();
				}
			}
		}


		public void Volver() {
			SceneManager.LoadScene(parametros.escenaAnterior);
		}


		public void Modificar() {
			Predeterminar();
			ControlEscena escena = ControlEscena.GetInstancia();
			escena.CambiarEscena("CONSTRUCCION");
		}


		public void Eliminar() {
			OpcionMazoConstruccion opcion = TraerSeleccionado();
			opcion.Eliminar();
		}


		private OpcionMazoConstruccion TraerSeleccionado() {
			GameObject[] mazos = GameObject.FindGameObjectsWithTag("mazo");
			OpcionMazoConstruccion opcion = null;
			foreach (GameObject mazo in mazos) {
				if (mazo.GetComponent<OpcionMazoConstruccion>().seleccionado)
					opcion = mazo.GetComponent<OpcionMazoConstruccion>();
			}
			return opcion;
		}


	}

}