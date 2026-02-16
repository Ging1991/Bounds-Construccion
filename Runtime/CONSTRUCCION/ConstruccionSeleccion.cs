using Bounds.Infraestructura;
using Bounds.Modulos.Persistencia;
using Bounds.Persistencia.Parametros;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;
using Ging1991.Persistencia.Lectores.Directos;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bounds.Contruccion {

	public class ConstruccionSeleccion : MonoBehaviour {

		public MusicaDeFondo musicaDeFondo;
		public ParametrosControl parametrosControl;

		void Start() {
			Debug.Log(parametrosControl.parametros.inicializado);
			parametrosControl.Inicializar();
			musicaDeFondo.Inicializar(new DireccionRecursos("Musica", "Fondo").Generar());
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
			SceneManager.LoadScene(parametrosControl.parametros.escenaPadre);
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