using UnityEngine;
using UnityEngine.InputSystem;

public class ListaLigadV : MonoBehaviour
{
    public class NodoWaypoint
    {
        public string nombreWaypoint;
        public NodoWaypoint siguiente;

        public NodoWaypoint(string nombre)
        {
            nombreWaypoint = nombre;
            siguiente = null;
        }
    }

   
    public NodoWaypoint inicioLista;
    public NodoWaypoint objetivoActual;

    public int contadorNodos = 1;
    public float temporizadorSimulacion = 0f;
    public float tiempoPorPaso = 2f;

    void Start()
    {
        AgregarNodo();
        AgregarNodo();
        AgregarNodo();

        objetivoActual = inicioLista;

        ImprimirRutaActual();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                AgregarNodo();
                ImprimirRutaActual();
            }

            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                SacarNodo();
                ImprimirRutaActual();
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                VoltearLista();
                ImprimirRutaActual();
            }
        }

        if (objetivoActual != null)
        {
            temporizadorSimulacion += Time.deltaTime;

            if (temporizadorSimulacion >= tiempoPorPaso)
            {
                temporizadorSimulacion = 0f;
                EjecutarComportamientosAgente();
            }
        }
    }

    void AgregarNodo()
    {
        NodoWaypoint nuevoNodo = new NodoWaypoint("Punto " + contadorNodos);
        contadorNodos++;

        if (inicioLista == null)
        {
            inicioLista = nuevoNodo;
        }
        else
        {
            NodoWaypoint actual = inicioLista;
            while (actual.siguiente != null)
            {
                actual = actual.siguiente;
            }
            actual.siguiente = nuevoNodo;
        }
        Debug.Log($"Se agregado nuevo {nuevoNodo.nombreWaypoint}.");
    }

    void SacarNodo()
    {
        if (inicioLista == null) return;

        if (inicioLista.siguiente == null)
        {
            Debug.Log($" sacó el {inicioLista.nombreWaypoint}La lista ahora está vacía.");
            inicioLista = null;
            objetivoActual = null;
            return;
        }

        NodoWaypoint actual = inicioLista;
        while (actual.siguiente.siguiente != null)
        {
            actual = actual.siguiente;
        }

        Debug.Log($" Se saco {actual.siguiente.nombreWaypoint} de la lista.");
        actual.siguiente = null;
    }

    void VoltearLista()
    {
        if (inicioLista == null || inicioLista.siguiente == null) return;

        NodoWaypoint previo = null;
        NodoWaypoint actual = inicioLista;
        NodoWaypoint siguiente = null;

        while (actual != null)
        {
            siguiente = actual.siguiente;
            actual.siguiente = previo;
            previo = actual;
            actual = siguiente;
        }

        inicioLista = previo;
        objetivoActual = inicioLista;
        Debug.Log("se volteoel orden de la lista");
    }

    void EjecutarComportamientosAgente()
    {
        if (Random.value < 0.2f)
        {
            Debug.LogWarning($"Obstáculo detectado camino a {objetivoActual.nombreWaypoint} rodeando el objeto");
            return;
        }

        if (objetivoActual.siguiente == null)
        {
            Debug.Log($" {objetivoActual.nombreWaypoint}. Disminuyendo la velocidad c detiene");
            objetivoActual = null;
        }
        else
        {
            Debug.Log($" llegado a {objetivoActual.nombreWaypoint}vamos al siguiente waypoint");
            objetivoActual = objetivoActual.siguiente;
        }
    }

    void ImprimirRutaActual()
    {
        if (inicioLista == null)
        {
            Debug.Log("Ruta actual ");
            return;
        }

        string ruta = "Ruta actual: ";
        NodoWaypoint actual = inicioLista;
        while (actual != null)
        {
            ruta += actual.nombreWaypoint;
            if (actual.siguiente != null) ruta += " - ";
            actual = actual.siguiente;
        }
        Debug.Log(ruta);
    }
}