using System.Collections.Generic;
using Bounds.Modulos.Cartas.Ilustradores;
using UnityEngine;

namespace Bounds.Contruccion {

	public class Pagina : MonoBehaviour, ISeleccionarCartaID {

		public List<GameObject> opciones;
		public IlustradorDeCartas ilustrador;

		public void Iniciar() { }


		public void Cargar(List<LineaRecetaConstruccion> cartas) {
			ilustrador = ConstructorControl.Instancia.ilustradorDeCartas;

			opciones.Sort((opcion1, opcion2) => {
				int numero1 = ExtraerNumero(opcion1.name);
				int numero2 = ExtraerNumero(opcion2.name);
				return numero1.CompareTo(numero2);
			});

			int ExtraerNumero(string name) {
				string numero = System.Text.RegularExpressions.Regex.Match(name, @"\d+$").Value;
				return int.TryParse(numero, out int result) ? result : 0;
			}

			DeshabilitarTodos();
			int indice = 0;

			Limitador limitador = new Limitador();
			foreach (LineaRecetaConstruccion linea in cartas) {
				OpcionCofre opcion = opciones[indice].GetComponent<OpcionCofre>();
				opcion.gameObject.SetActive(true);
				opcion.Iniciar(linea, this, limitador.GetLimite(linea.cartaID), ConstructorControl.Instancia.cartaGenerador);
				indice++;
			}

		}


		private void DeshabilitarTodos() {
			foreach (GameObject opcion in opciones) {
				opcion.SetActive(false);
			}
		}


		public void SeleccionarCartaID(string codigo) {
			FindAnyObjectByType<ConstructorControl>().AgregarCarta(codigo);
		}


	}

}