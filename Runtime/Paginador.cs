using System;
using System.Collections.Generic;
using Bounds.Cofres;
using Ging1991.Core;
using Ging1991.Interfaces.Personalizacion;
using UnityEngine;
using UnityEngine.UI;

namespace Bounds.Contruccion {

	public class Paginador : SingletonMonoBehaviour<Paginador> {

		public int pagina;
		public int maxPagina = 0;
		private List<LineaRecetaConstruccion> cartasTotales;
		public TextoUI paginaTexto;

		public void Iniciar() {
			pagina = 1;
			Actualizar();
		}


		public void Actualizar() {
			cartasTotales = FindAnyObjectByType<Recetario>().GetCartas();
			List<LineaRecetaConstruccion> cartas = SeleccionarPorPagina();

			cartas.Sort(delegate (LineaRecetaConstruccion carta1, LineaRecetaConstruccion carta2) {
				if (carta1.cartaID == carta2.cartaID)
					return 0;
				else if (carta1.cartaID < carta2.cartaID)
					return -1;
				else
					return 1;
			});

			FindAnyObjectByType<Pagina>().Cargar(cartas);
			ActualizarVisorPagina();
			ActualizarContadorCofre();
		}

		public void ActualizarContadorCofre() {
			string ret = ConstructorControl.Instancia.selectorSistema.GetElemento("COFRE_CONTENIDO");
			ret = ret.Replace("[CANTIDAD]", $"{FindAnyObjectByType<Recetario>().GetCantidadEnCofre()}");
			ret = ret.Replace("[MAXIMO]", "660");
			Text texto = GameObject.Find("ContadorCofre").GetComponentInChildren<Text>();
			texto.text = ret;
		}



		private List<LineaRecetaConstruccion> SeleccionarPorPagina() {
			List<LineaRecetaConstruccion> filtradas = FindAnyObjectByType<Filtro>().GetCartasFiltradas();
			const int cartasPorPagina = 10;

			int inicio = (pagina - 1) * cartasPorPagina;
			int fin = inicio + cartasPorPagina;

			if (inicio >= filtradas.Count)
				return new List<LineaRecetaConstruccion>();

			fin = Math.Min(fin, filtradas.Count);
			return filtradas.GetRange(inicio, fin - inicio);
		}


		private void ActualizarVisorPagina() {
			CalcularMaxPagina();
			string ret = ConstructorControl.Instancia.selectorSistema.GetElemento("PAGINA [ACTUAL]/[MAXIMO]");

			if (cartasTotales.Count == 0) {
				ret = ret.Replace("[ACTUAL]", "1");
				ret = ret.Replace("[MAXIMO]", "1");
			}
			else {
				ret = ret.Replace("[ACTUAL]", $"{pagina}");
				ret = ret.Replace("[MAXIMO]", $"{maxPagina}");
			}
			paginaTexto.SetTexto(ret);
		}


		private void CalcularMaxPagina() {
			int cantidad = 0;
			foreach (CartaCofreBD carta in FindAnyObjectByType<Filtro>().GetCartasFiltradas()) {
				cantidad++;
			}
			maxPagina = (cantidad + 9) / 10;
		}


		public void PaginaSiguiente() {
			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			CalcularMaxPagina();
			pagina++;
			if (pagina > maxPagina)
				pagina = 1;
			Actualizar();
		}


		public void PaginaAnterior() {
			if (CuadroAceptar.existenCuadros() || VisorConstruccion.VisorActivo())
				return;

			CalcularMaxPagina();
			pagina--;
			if (pagina < 1)
				pagina = maxPagina;
			Actualizar();
		}


	}

}