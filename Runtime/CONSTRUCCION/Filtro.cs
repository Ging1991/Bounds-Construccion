using System.Collections.Generic;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Persistencia;
using Bounds.Persistencia.Lectores;
using Ging1991.Core;
using Ging1991.Core.Interfaces;
using Ging1991.Interfaces.Entrada;
using UnityEngine;

namespace Bounds.Contruccion {

	public class Filtro : MonoBehaviour {

		public IProveedor<int, CartaBD> proveedorCartas;
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

		private List<int> cartasExplosion;
		private List<int> cartasOceano;
		private List<int> cartasBosque;
		private List<int> cartasMeta;
		private List<int> cartasEnero;
		private List<int> cartasPrincipiante;
		private List<int> cartasColeccion = new();

		public void Inicializar() {
			proveedorCartas = ConstructorControl.Instancia.proveedorCartas;

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

			cartasBosque = GenerarListaCartasID("BOSQUE");
			cartasExplosion = GenerarListaCartasID("EXPLOSION");
			cartasOceano = GenerarListaCartasID("OCEANO");
			cartasMeta = GenerarListaCartasID("META");
			cartasEnero = GenerarListaCartasID("ENERO2026");
			cartasPrincipiante = GenerarListaCartasID("PRINCIPIANTE");

			iniciado = true;
		}

		public List<int> GenerarListaCartasID(string codigo) {
			Coleccion coleccion = new Coleccion(codigo, ConstructorControl.Instancia.carpetaColecciones.Generar(codigo));
			List<int> cartasID = new();
			foreach (var carta in coleccion.GetListaCompleta())
				cartasID.Add(carta.cartaID);

			return cartasID;
		}


		public List<LineaRecetaConstruccion> GetCartasFiltradas() {
			List<LineaRecetaConstruccion> filtradas = new();
			if (grupoColecciones.opcionTodo.valor == false) {
				cartasColeccion.Clear();
				if (controladorColecciones.valores["BOSQUE"])
					cartasColeccion.AddRange(cartasBosque);
				if (controladorColecciones.valores["EXPLOSION"])
					cartasColeccion.AddRange(cartasExplosion);
				if (controladorColecciones.valores["OCEANO"])
					cartasColeccion.AddRange(cartasOceano);
				if (controladorColecciones.valores["META"])
					cartasColeccion.AddRange(cartasMeta);
				if (controladorColecciones.valores["ENERO2026"])
					cartasColeccion.AddRange(cartasEnero);
				if (controladorColecciones.valores["PRINCIPIANTE"])
					cartasColeccion.AddRange(cartasPrincipiante);
			}


			foreach (LineaRecetaConstruccion carta in FindAnyObjectByType<Recetario>().GetCartas()) {
				if (CumpleCriterio(carta.cartaID))
					filtradas.Add(carta);
			}

			return filtradas;
		}


		private bool CumpleCriterio(int cartaID) {
			if (!iniciado)
				return true;

			CartaBD carta = proveedorCartas.GetElemento(cartaID);

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
				if (!controladorNiveles.valores[nivelCadena])
					return false;
			}

			// COLECCIONES
			if (grupoColecciones.opcionTodo.valor == false) {
				if (!cartasColeccion.Contains(cartaID))
					return false;
			}

			return true;
		}


		public void BotonMostrarFiltro() {
			panel.localPosition = new Vector3(0, 0, 0);
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