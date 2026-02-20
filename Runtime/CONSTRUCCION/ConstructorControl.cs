using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;
using Ging1991.Persistencia.Lectores.Directos;
using Bounds.Global.Mazos;
using Ging1991.Core;
using Bounds.Modulos.Cartas.Tinteros;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Ilustradores;
using Bounds.Modulos.Visor;
using Bounds.Modulos.Visor.Persistencia;
using Bounds.Persistencia.Lectores;
using Ging1991.Interfaces.Contadores;
using Bounds.Persistencia;
using Bounds.Cofres;
using Bounds.Persistencia.Parametros;
using Bounds.Modulos.Persistencia;
using Ging1991.Core.Interfaces;

namespace Bounds.Contruccion {

	public class ConstructorControl : SingletonMonoBehaviour<ConstructorControl> {

		public IlustradorDeCartas ilustradorDeCartas;
		public DatosDeCartas datosDeCartas;
		public DatosDeEfectos datosDeEfectos;
		public ITintero tintero;

		public List<GameObject> opcionesMazo;
		public GameObject claseVisor;
		public Dictionary<string, bool> casillas;
		public string nombreMazo;
		public CartaMazo vacioPrinpal;
		public CartaMazo cartaPrinpal;
		public Cofre cofre;
		public ParametrosEscena parametros;
		public ParametrosControl parametrosControl;

		public ISelector<int, string> selectorNombres;
		public ISelector<string, string> selectorClases;
		public ISelector<string, string> selectorTipos;
		public ISelector<string, string> selectorInvocaciones;

		public void CrearVisor(LineaRecetaConstruccion linea) {
			Billetera billetera = new Billetera(new DireccionDinamica("CONFIGURACION", "BILLETERA.json").Generar());
			GameObject visor = Instantiate(claseVisor, new Vector3(0, 0, 0), Quaternion.identity);
			GameObject lienzo = GameObject.Find("LienzoVisor");
			visor.transform.SetParent(lienzo.transform);
			visor.transform.localScale = new Vector3(1, 1, 1);
			visor.transform.localPosition = new Vector3(0, 0, 0);
			visor.name = "visor";

			visor.GetComponentInChildren<VisorGeneral>().Inicializar(
				datosDeCartas, datosDeEfectos, ilustradorDeCartas, tintero, selectorClases,
				selectorTipos, selectorInvocaciones, selectorNombres);
			visor.GetComponent<VisorConstruccion>().Mostrar(linea, billetera, cofre);
		}


		public void Escribirnombre(string nombre) {
			nombreMazo = nombre;
		}


		public static string GetNombreMazo() {
			Direccion direccion = new DireccionDinamica("MAZOS", "PREDETERMINADO.json");
			LectorCadena lectorCadena = new LectorCadena(direccion.Generar(), TipoLector.DINAMICO);
			return lectorCadena.Leer().valor;
		}


		void Start() {
			datosDeCartas.Inicializar();
			datosDeEfectos.Inicializar();
			parametrosControl.Inicializar();
			parametros = parametrosControl.parametros;

			ilustradorDeCartas = new IlustradorDeCartas(
				parametrosControl.parametros.direcciones["CARTAS_RECURSO"],
				parametrosControl.parametros.direcciones["CARTAS_DINAMICA"]
			);
			selectorNombres = new TraductorCartaID(parametros.direcciones["CARTA_NOMBRES"]);
			selectorClases = new TraductorTexto(parametros.direcciones["CARTA_CLASES"]);
			selectorTipos = new TraductorTexto(parametros.direcciones["CARTA_TIPOS"]);
			selectorInvocaciones = new TraductorTexto(parametros.direcciones["CARTA_INVOCACIONES"]);

			tintero = new TinteroBounds();

			cofre = new Cofre();

			FindAnyObjectByType<Recetario>().Iniciar(GetNombreMazo());
			FindAnyObjectByType<Paginador>().Iniciar();

			casillas = new Dictionary<string, bool>();
			opcionesMazo = new List<GameObject>();

			ActualizarOpcionesMazo();
			FindAnyObjectByType<Filtro>().Inicializar();
		}


		private void ActualizarOpcionesMazo() {

			foreach (GameObject opcion in opcionesMazo)
				Destroy(opcion);
			opcionesMazo = new List<GameObject>();

			InstanciadorConstruir instanciador = GameObject.Find("Instanciador").GetComponent<InstanciadorConstruir>();
			List<LineaRecetaConstruccion> cartasEnMazo = FindAnyObjectByType<Recetario>().GetCartasEnMazo();

			cartasEnMazo.Sort(delegate (LineaRecetaConstruccion carta1, LineaRecetaConstruccion carta2) {
				int nivel1 = datosDeCartas.lector.LeerDatos(carta1.cartaID).nivel;
				int nivel2 = datosDeCartas.lector.LeerDatos(carta2.cartaID).nivel;
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
				string nombre = datosDeCartas.lector.LeerDatos(linea.cartaID).nombre;
				GameObject opcion = instanciador.CrearOpcionMazo(linea, nombre, new Vector3(20, posicionY, 0));
				Color colorLetra = tintero.GetColor($"NIVEL_{linea.rareza}");
				opcion.GetComponentInChildren<ContadorSimbolo>().SetValor(colorLetra, linea.cantidadEnMazo, linea.cantidadEnCofre, linea.limite);
				opcion.transform.localPosition = new Vector3(20, posicionY - 335, 0);
				opcionesMazo.Add(opcion);
				i++;
			}

			ActualizarContadorMazo();
		}


		public void AgregarCarta(string codigoParcial) {
			foreach (LineaRecetaConstruccion carta in FindAnyObjectByType<Recetario>().GetCartas()) {
				if (carta.GetCodigoParcial() == codigoParcial) {
					if (carta.cantidadEnMazo == carta.limite
							|| carta.cantidadEnMazo == carta.cantidadEnCofre
							|| FindAnyObjectByType<Recetario>().CantidadEnMazoActual(carta.cartaID) >= carta.limite) {
						GetComponent<GestorDeSonidos>().ReproducirSonido("FxRebote");

					}
					else {
						carta.cantidadEnMazo++;
						GetComponent<GestorDeSonidos>().ReproducirSonido("FxAdquisicion");
						ActualizarOpcionesMazo();
						FindAnyObjectByType<Paginador>().Actualizar();
					}
				}
			}
		}


		public void SacarCarta(string codigoParcial) {
			foreach (LineaRecetaConstruccion carta in FindAnyObjectByType<Recetario>().GetCartas()) {
				if (carta.GetCodigoParcial() == codigoParcial) {
					carta.cantidadEnMazo--;
				}
			}
			GetComponent<GestorDeSonidos>().ReproducirSonido("FxAdquisicion");
			ActualizarOpcionesMazo();
			FindAnyObjectByType<Paginador>().Actualizar();
		}


		public void BotonCancelar() {

			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			SceneManager.LoadScene("CONSTRUCCION SELECCION");
		}


		public void ActualizarContadorMazo() {
			Text texto = GameObject.Find("VisorCantidad").GetComponentInChildren<Text>();
			texto.text = $"Mazo: {FindAnyObjectByType<Recetario>().GetCantidadEnMazo()}/40 cartas\nPromedio: {FindAnyObjectByType<Verificador>().GetNivelPromedio()}";
		}


		public static List<int> EliminarDuplicados(List<int> numeros) {
			HashSet<int> numerosUnicos = new HashSet<int>(numeros);
			return new List<int>(numerosUnicos);
		}


	}

}