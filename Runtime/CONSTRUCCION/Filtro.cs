using System.Collections.Generic;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Persistencia.Lectores;
using Ging1991.Core;
using Ging1991.Interfaces.Selecciones;
using UnityEngine;

namespace Bounds.Contruccion {

	public class Filtro : MonoBehaviour {

		public DatosDeCartas datosDeCartas;
		public GrupoDeCasillas grupoClases;
		public GrupoDeCasillas grupoInvocaciones;
		public GrupoDeCasillas grupoNiveles;
		public GrupoDeCasillas grupoColecciones;
		private bool iniciado = false;
		private ControladorCasillas controladorClases;
		private ControladorCasillas controladorInvocaciones;
		private ControladorCasillas controladorNiveles;
		private ControladorCasillas controladorColecciones;
		public Transform panel;
		private List<string> cartasEnero;
		private List<string> cartasMeta;
		private List<string> cartasExplosion;

		public void Inicializar() {
			controladorClases = new();
			controladorInvocaciones = new();
			controladorNiveles = new();
			controladorColecciones = new();

			grupoClases.Iniciar();
			grupoInvocaciones.Iniciar();
			grupoNiveles.Iniciar();
			grupoColecciones.Iniciar();

			grupoClases.AgregarObservador(controladorClases);
			grupoInvocaciones.AgregarObservador(controladorInvocaciones);
			grupoNiveles.AgregarObservador(controladorNiveles);
			grupoColecciones.AgregarObservador(controladorColecciones);

			grupoClases.opcionTodo.Presionar();
			grupoInvocaciones.opcionTodo.Presionar();
			grupoNiveles.opcionTodo.Presionar();
			grupoColecciones.opcionTodo.Presionar();

			LectorColeccion lectorColeccion = new(ConstructorControl.Instancia.carpetaColecciones.Generar("ENERO2026"));
			cartasEnero = new();
			cartasEnero.AddRange(lectorColeccion.Leer().comunes);
			cartasEnero.AddRange(lectorColeccion.Leer().infrecuentes);
			cartasEnero.AddRange(lectorColeccion.Leer().raras);
			cartasEnero.AddRange(lectorColeccion.Leer().miticas);
			cartasEnero.AddRange(lectorColeccion.Leer().secretas);

			lectorColeccion = new(ConstructorControl.Instancia.carpetaColecciones.Generar("META"));
			cartasMeta = new();
			cartasMeta.AddRange(lectorColeccion.Leer().comunes);
			cartasMeta.AddRange(lectorColeccion.Leer().infrecuentes);
			cartasMeta.AddRange(lectorColeccion.Leer().raras);
			cartasMeta.AddRange(lectorColeccion.Leer().miticas);
			cartasMeta.AddRange(lectorColeccion.Leer().secretas);

			lectorColeccion = new(ConstructorControl.Instancia.carpetaColecciones.Generar("EXPLOSION"));
			cartasExplosion = new();
			cartasExplosion.AddRange(lectorColeccion.Leer().comunes);
			cartasExplosion.AddRange(lectorColeccion.Leer().infrecuentes);
			cartasExplosion.AddRange(lectorColeccion.Leer().raras);
			cartasExplosion.AddRange(lectorColeccion.Leer().miticas);
			cartasExplosion.AddRange(lectorColeccion.Leer().secretas);

			iniciado = true;
		}


		public List<LineaRecetaConstruccion> GetCartasFiltradas() {
			List<LineaRecetaConstruccion> filtradas = new();

			foreach (LineaRecetaConstruccion carta in FindAnyObjectByType<Recetario>().GetCartas()) {
				if (CumpleCriterio(carta.cartaID))
					filtradas.Add(carta);
			}

			return filtradas;
		}


		private bool CumpleCriterio(int cartaID) {
			if (!iniciado)
				return true;

			CartaBD carta = datosDeCartas.lector.LeerDatos(cartaID);

			// CLASES
			if (grupoClases.opcionTodo.valor == false) {
				if (controladorClases.valores[carta.clase] == false)
					return false;
			}

			// INVOCACIONES
			if (grupoInvocaciones.opcionTodo.valor == false) {
				if (carta.clase != "CRIATURA")
					return false;
				if (!controladorInvocaciones.valores.ContainsKey(carta.datoCriatura.perfeccion))
					return false;
				if (controladorInvocaciones.valores[carta.datoCriatura.perfeccion] == false)
					return false;
			}

			// NIVELES
			if (grupoNiveles.opcionTodo.valor == false) {
				string nivelCadena = (carta.nivel > 9) ? "10" : $"{carta.nivel}";
				return controladorNiveles.valores[nivelCadena];
			}

			// COLECCIONES
			if (grupoColecciones.opcionTodo.valor == false) {

				if (controladorColecciones.valores["ENERO2026"] && !cartasEnero.Contains($"{cartaID}_GEMINI"))
					return false;
				if (controladorColecciones.valores["META"] && !cartasMeta.Contains($"{cartaID}_META"))
					return false;
				if (controladorColecciones.valores["EXPLOSION"] && !(cartasExplosion.Contains($"{cartaID}_META") || cartasExplosion.Contains($"{cartaID}_GEMINI")))
					return false;
			}

			return true;
		}


		public void BotonMostrarFiltro() {
			panel.localPosition = new Vector3(0, 75, 0);
			Bloqueador.BloquearGrupo("GLOBAL", true);
		}


		public void BotonOcultarFiltro() {
			panel.localPosition = new Vector3(-2000, 0, 0);
			Bloqueador.BloquearGrupo("GLOBAL", false);
			FindAnyObjectByType<Paginador>().Iniciar();
		}


		private class ControladorCasillas : IAlternadorObservador {

			public Dictionary<string, bool> valores;

			public ControladorCasillas() {
				valores = new();
			}

			public void AlternadorPresionado(Alternador alternador) {
				valores[alternador.codigo] = alternador.valor;
			}

		}

	}

}