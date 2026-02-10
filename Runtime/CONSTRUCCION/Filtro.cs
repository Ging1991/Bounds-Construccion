using System.Collections.Generic;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Ging1991.Core;
using Ging1991.Interfaces.Selecciones;
using UnityEngine;

namespace Bounds.Contruccion {

	public class Filtro : MonoBehaviour {

		public DatosDeCartas datosDeCartas;
		public GrupoDeCasillas grupoClases;
		public GrupoDeCasillas grupoInvocaciones;
		public GrupoDeCasillas grupoNiveles;
		private bool iniciado = false;
		private ControladorCasillas controladorClases;
		private ControladorCasillas controladorInvocaciones;
		private ControladorCasillas controladorNiveles;
		public Transform panel;

		public void Inicializar() {
			controladorClases = new();
			controladorInvocaciones = new();
			controladorNiveles = new();

			grupoClases.Iniciar();
			grupoInvocaciones.Iniciar();
			grupoNiveles.Iniciar();

			grupoClases.AgregarObservador(controladorClases);
			grupoInvocaciones.AgregarObservador(controladorInvocaciones);
			grupoNiveles.AgregarObservador(controladorNiveles);

			grupoClases.opcionTodo.Presionar();
			grupoInvocaciones.opcionTodo.Presionar();
			grupoNiveles.opcionTodo.Presionar();

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