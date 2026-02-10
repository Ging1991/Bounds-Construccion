using Ging1991.Core.Interfaces;
using Ging1991.Relojes;
using UnityEngine;

namespace Bounds.Contruccion {

	public class MantenerPresionado : MonoBehaviour {

		private IEjecutable acccion;
		public bool estaPresionado, logrado;
		public float tiempoMaximo;
		public float tiempoOriginal;
		public float tiempoActual, diferencia;


		public void Iniciar(IEjecutable acccion) {
			this.acccion = acccion;
			estaPresionado = false;
			logrado = false;
			tiempoMaximo = 0.7f;
			tiempoOriginal = 0;
			tiempoActual = 0;
		}


		void OnMouseDown() {
			estaPresionado = true;
			tiempoOriginal = Time.deltaTime;
			tiempoActual = tiempoOriginal;
		}


		void OnMouseUp() {
			estaPresionado = false;
		}


		void FixedUpdate() {
			if (estaPresionado) {
				tiempoActual += Time.deltaTime;
				diferencia = (tiempoActual - tiempoOriginal);
				if (diferencia > tiempoMaximo)
					EjecutarAccion();
			}
		}


		void EjecutarAccion() {
			acccion.Ejecutar();
			estaPresionado = false;
			logrado = true;
			tiempoOriginal = Time.deltaTime;
			tiempoActual = Time.deltaTime;
		}


	}

}