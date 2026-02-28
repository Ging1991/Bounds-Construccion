using Bounds.Global;
using Bounds.Global.Mazos;
using Bounds.Modulos.Cartas.Ilustradores;
using Bounds.Modulos.Cartas.Tinteros;
using Bounds.Tienda;
using Ging1991.Core.Interfaces;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;
using Ging1991.Persistencia.Lectores.Directos;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class OpcionMazoConstruccion : MonoBehaviour {

		public bool seleccionado;
		public string nombre;
		public IProveedor<string, Sprite> ilustrador;

		public void Inicializar(IProveedor<string, Sprite> ilustrador) {
			this.ilustrador = ilustrador;
			MostrarPredeterminado();
			Mazo mazo = new MazoJugador(nombre);

			SetNombre(mazo.nombre);
			if (mazo.emblema != null)
				GetComponent<ContenedorDeCartas>().Inicializar(ilustrador, new TinteroBounds(), mazo.emblema.cartaID, mazo.emblema.imagen);
		}


		void OnMouseDown() {
			if (!CuadroAceptar.existenCuadros() && !VisorConstruccion.VisorActivo())
				Seleccionar();
		}


		public void Seleccionar() {
			GameObject control = GameObject.Find("OpcionesMazo");
			ConstruccionSeleccion scr = control.GetComponent<ConstruccionSeleccion>();
			scr.DeseleccionarTodo();
			transform.localScale = new Vector3(1.2f, 1.2f, 1);
			seleccionado = true;
		}


		public void Deseleccionar() {
			transform.localScale = new Vector3(1f, 1f, 1);
			seleccionado = false;
		}


		public void MostrarPredeterminado() {
			Direccion direccion = new DireccionDinamica("MAZOS", "PREDETERMINADO.json");
			Direccion direccionInicial = new DireccionRecursos("MAZOS", "PREDETERMINADO.json");

			LectorCadena lectorCadena = new LectorCadena(direccion.Generar(), TipoLector.DINAMICO);
			lectorCadena.InicializarDesdeRecursos(direccionInicial.Generar());

			Vector3 posicion = transform.localPosition;
			posicion.y = 0;
			if (lectorCadena.Leer().valor == nombre)
				posicion.y = -100;
			transform.localPosition = posicion;
		}


		public void SetNombre(string nombre) {
			GetComponentInChildren<Text>().text = nombre;
		}


		public void Eliminar() {
			//lector.EliminarMazo(nombre);
			gameObject.SetActive(false);
		}


	}

}