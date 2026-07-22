using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;
using Ging1991.Persistencia.Lectores.Directos;
using Ging1991.Core;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Ilustradores;
using Bounds.Modulos.Visor.Persistencia;
using Bounds.Persistencia;
using Bounds.Cofres;
using Bounds.Persistencia.Parametros;
using Bounds.Modulos.Persistencia;
using Ging1991.Core.Interfaces;
using Bounds.Musica;
using Ging1991.Musica;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Ging1991.Interfaces.Salida;
using Ging1991.Ventanas;
using Ging1991.Persistencia.Proveedores;
using Bounds.Mazos;
using Bounds.Cartas;
using Bounds.Persistencia.proveedores;
using Bounds.Visor;

namespace Bounds.Contruccion {

	public class ConstructorControl : SingletonMonoBehaviour<ConstructorControl> {

		public IlustradorDeCartas ilustradorDeCartas;
		public IProveedor<int, CartaBD> proveedorCartas;
		public IProveedor<string, EfectoTraduccion> selectorHabilidades;

		public List<GameObject> opcionesMazo;
		public GameObject claseVisor;
		public Dictionary<string, bool> casillas;
		public string nombreMazo;
		public CartaMazo vacioPrinpal;
		public CartaMazo cartaPrinpal;
		public Cofre cofre;
		public ParametrosEscena parametros;
		public ParametrosControl parametrosControl;

		public IProveedor<int, string> selectorNombres;
		public IProveedor<int, string> selectorEfectos;
		public IProveedor<int, string> selectorAmbientacion;
		public IProveedor<string, string> selectorClases;
		public IProveedor<string, string> selectorTipos;
		public IProveedor<string, string> selectorInvocaciones;
		public IProveedor<string, string> selectorSistema;
		public DireccionRecursos carpetaColecciones;
		public InstanciadorConstruir instanciador;
		public GestorDeSonidos gestorDeSonidos;
		public ControlUIBounds personalizarUI;
		public VisorConstruccion visorConstruccion;
		public VentanaControl ventanaControl;
		public CartaGenerador cartaGenerador;
		public VisorGenerador visorGenerador;

		private void InicializarMusica(string direccion) {
			MusicaAmbiental musicaAmbiental = MusicaAmbiental.Instancia;
			if (musicaAmbiental.actual != "GENERAL") {
				musicaAmbiental.Inicializar(new ProveedorAudios(new DireccionRecursos(direccion)));
				musicaAmbiental.Reproducir("GENERAL");
			}
		}


		void Start() {
			parametrosControl.Inicializar();
			parametros = parametrosControl.parametros;

			InicializarMusica(parametros.direcciones["MUSICA_AMBIENTAL"]);

			personalizarUI.Personalizar(parametros.direcciones["SISTEMA"], parametros.direcciones["COLORES"]);

			ilustradorDeCartas = new IlustradorDeCartas(
				parametrosControl.parametros.direcciones["CARTAS_RECURSO"],
				parametrosControl.parametros.direcciones["CARTAS_DINAMICA"]
			);
			selectorNombres = new TraductorCartaID(parametros.direcciones["CARTA_NOMBRES"]);
			selectorEfectos = new TraductorCartaID(parametros.direcciones["CARTA_EFECTOS"]);
			selectorAmbientacion = new TraductorCartaID(parametros.direcciones["CARTA_AMBIENTACION"]);
			selectorClases = new ProveedorTexto(parametros.direcciones["CARTA_CLASES"], TipoLector.RECURSOS);
			selectorSistema = new ProveedorTexto(parametros.direcciones["SISTEMA"], TipoLector.RECURSOS);
			selectorTipos = new ProveedorTexto(parametros.direcciones["CARTA_TIPOS"], TipoLector.RECURSOS);
			selectorInvocaciones = new ProveedorTexto(parametros.direcciones["CARTA_INVOCACIONES"], TipoLector.RECURSOS);
			carpetaColecciones = new(parametros.direcciones["COLECCIONES"]);
			proveedorCartas = new LectorCartas(new DireccionRecursos(parametrosControl.parametros.direcciones["CARTAS_DATOS"]));
			selectorHabilidades = new LectorHabilidades(parametrosControl.parametros.direcciones["CARTAS_HABILIDADES"]);
			//musicaDeFondo.Inicializar(parametrosControl.parametros.direcciones["MUSICA_TIENDA"]);
			gestorDeSonidos.Inicializar(new DireccionRecursos(parametrosControl.parametros.direcciones["SONIDOS"]));

			cartaGenerador.Inicializar(
				ilustradorDeCartas,
				proveedorCartas,
				new ProveedorColores(
					parametrosControl.parametros.direcciones["COLORES"],
					TipoLector.RECURSOS
				)
			);

			cofre = new(parametros.direcciones["COFRE"], parametros.direcciones["COFRE_RECURSOS"]);

			FindAnyObjectByType<Filtro>().Inicializar();
			FindAnyObjectByType<Recetario>().Iniciar(GetNombreMazo());
			FindAnyObjectByType<Paginador>().Iniciar();

			casillas = new Dictionary<string, bool>();
			opcionesMazo = new List<GameObject>();
			visorGenerador.Inicializar(
				proveedorCartas,
				selectorHabilidades,
				ilustradorDeCartas,
				new ProveedorColores(
					parametrosControl.parametros.direcciones["COLORES"],
					TipoLector.RECURSOS
				),
				selectorSistema,
				selectorClases,
				selectorTipos,
				selectorInvocaciones,
				selectorNombres,
				selectorAmbientacion,
				selectorEfectos
			);
			ActualizarOpcionesMazo();
		}


		public void CrearVisor(LineaRecetaConstruccion linea) {
			Billetera billetera = new Billetera(new DireccionDinamica("CONFIGURACION", "BILLETERA.json").Generar());
			visorConstruccion.gameObject.SetActive(true);
			visorConstruccion.Mostrar(linea, billetera, cofre, visorGenerador);
		}


		public void Escribirnombre(string nombre) {
			nombreMazo = nombre;
		}


		public static string GetNombreMazo() {
			Direccion direccion = new DireccionDinamica("MAZOS", "PREDETERMINADO.json");
			LectorCadena lectorCadena = new LectorCadena(direccion.Generar(), TipoLector.DINAMICO);
			return lectorCadena.Leer().valor;
		}


		private void ActualizarOpcionesMazo() {

			foreach (GameObject opcion in opcionesMazo)
				Destroy(opcion);
			opcionesMazo = new List<GameObject>();

			List<LineaRecetaConstruccion> cartasEnMazo = FindAnyObjectByType<Recetario>().GetCartasEnMazo();

			cartasEnMazo.Sort(delegate (LineaRecetaConstruccion carta1, LineaRecetaConstruccion carta2) {
				int nivel1 = proveedorCartas.GetElemento(carta1.cartaID).nivel;
				int nivel2 = proveedorCartas.GetElemento(carta2.cartaID).nivel;
				if (nivel1 == nivel2)
					return 0;
				else if (nivel1 > nivel2)
					return -1;
				else
					return 1;
			});

			int i = 0;
			foreach (LineaRecetaConstruccion linea in cartasEnMazo) {
				float posicionY = -105 * i + 285;
				GameObject opcion = instanciador.CrearOpcionMazo(linea, new Vector3(20, posicionY, 0));
				opcion.transform.localPosition = new Vector3(-10, posicionY - 335, 0);

				CartaBD cartaBD = proveedorCartas.GetElemento(linea.cartaID);

				Color colorTinta = cartaGenerador.proveedorColores.GetElemento($"TINTA_{linea.rareza}");
				Color colorNivelRelleno = cartaGenerador.proveedorColores.GetElemento($"NIVEL_{linea.rareza}");
				CartaEnMazo cartaEnMazo = opcion.GetComponent<CartaEnMazo>();
				cartaEnMazo.SetNivel(
					cartaBD.nivel,
					colorTinta,
					colorNivelRelleno
				);

				cartaEnMazo.SetCantidad(linea.cantidadEnMazo, linea.cantidadEnCofre, linea.limite, colorNivelRelleno);
				string nombre = visorGenerador.proveedorNombres.GetElemento(linea.cartaID);
				if (string.IsNullOrEmpty(nombre))
					nombre = cartaBD.nombre;
				cartaEnMazo.SetNombre(nombre, colorTinta);

				string claseColor = (cartaBD.clase != "CRIATURA") ? cartaBD.clase : cartaBD.datoCriatura.perfeccion;
				cartaEnMazo.SetFondo(
					cartaGenerador.proveedorColores.GetElemento($"RELLENO_{claseColor}"),
					cartaGenerador.proveedorColores.GetElemento($"RELLENO_CLARO_{claseColor}")
				);

				opcionesMazo.Add(opcion);
				i++;
			}

			ActualizarContadorMazo();
		}


		public void AgregarCarta(string codigoParcial) {
			foreach (LineaRecetaConstruccion carta in FindAnyObjectByType<Recetario>().GetCartas()) {
				if (carta.GetCodigoIndividual() == codigoParcial) {
					if (carta.cantidadEnMazo == carta.limite
							|| carta.cantidadEnMazo == carta.cantidadEnCofre
							|| FindAnyObjectByType<Recetario>().CantidadEnMazoActual(carta.cartaID) >= carta.limite) {

						gestorDeSonidos.ReproducirSonido("FxRebote");
					}
					else {
						carta.cantidadEnMazo++;
						gestorDeSonidos.ReproducirSonido("FxAdquisicion");
						ActualizarOpcionesMazo();
						FindAnyObjectByType<Paginador>().Actualizar();
					}
				}
			}
		}


		public void SacarCarta(string codigoParcial) {
			foreach (LineaRecetaConstruccion carta in FindAnyObjectByType<Recetario>().GetCartas()) {
				if (carta.GetCodigoIndividual() == codigoParcial) {
					carta.cantidadEnMazo--;
				}
			}
			gestorDeSonidos.ReproducirSonido("FxAdquisicion");
			ActualizarOpcionesMazo();
			FindAnyObjectByType<Paginador>().Actualizar();
		}


		public void BotonCancelar() {

			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			SceneManager.LoadScene("CONSTRUCCION SELECCION");
		}


		public void ActualizarContadorMazo() {
			string ret = selectorSistema.GetElemento("MAZO_CONTENIDO");
			ret = ret.Replace("[CANTIDAD]", $"{FindAnyObjectByType<Recetario>().GetCantidadEnMazo()}");
			ret = ret.Replace("[MAXIMO]", "40");
			ret = ret.Replace("[PROMEDIO]", $"{FindAnyObjectByType<Verificador>().GetNivelPromedio()}");
			GameObject.Find("VisorCantidad").GetComponentInChildren<Text>().text = ret;
		}


		public static List<int> EliminarDuplicados(List<int> numeros) {
			HashSet<int> numerosUnicos = new HashSet<int>(numeros);
			return new List<int>(numerosUnicos);
		}


	}

}