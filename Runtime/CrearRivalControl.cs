using Ging1991.Persistencia.Direcciones;
using Ging1991.Core;
using Ging1991.Musica;
using Bounds.Sistema.Parametros;
using Bounds.Sistema;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bounds.Conexiones.Servicios;
using System.Threading.Tasks;
using UnityEngine;
using Bounds.Mazos;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Bounds.Contruccion {

	public class CrearRivalControl : SingletonMonoBehaviour<ConstructorControl> {

		public ControlBounds controlBounds;
		private ParametrosGlobales parametros;
		public GestorDeSonidos gestorDeSonidos;
		public Text nombreJugador;
		public Text nombreMazo;
		public Text mensaje;
		public GameObject boton;

		void Start() {
			parametros = controlBounds.InicializarEscena("GENERAL");
			gestorDeSonidos.Inicializar(new DireccionRecursos(parametros.direccionesGeneradas["SONIDOS"]));
		}

		public void BotonCrear() {
			boton.SetActive(false);
			_ = CrearRivalProceso();
		}

		public async Task CrearRivalProceso() {
			mensaje.text = $"Esperando al servidor...";

			string jugador = nombreJugador.text;
			string mazo = nombreMazo.text;
			if (jugador == "" || mazo == "") {
				mensaje.text = "Complete ambos campos: Jugador y Mazo";
				return;
			}

			Mazo mazoOBJ = new MazoJugador(MazoJugador.GetPredeterminado());
			List<string> listaCartas = new();
			foreach (var carta in mazoOBJ.cartas) {
				listaCartas.Add(carta.GetCodigo());
			}

			string cartas = String.Join(",", listaCartas);
			ServicioCrearRival api = new(jugador, mazo, cartas);
			ServicioCrearRival.Salida salida = await api.EjecutarServicio();
			mensaje.text = $"Resultado: {salida.resultado}\nMensaje: {salida.mensaje}";
			boton.SetActive(true);
		}

		public void Volver() {
			SceneManager.LoadScene("CONSTRUCCION SELECCION");
		}


	}

}