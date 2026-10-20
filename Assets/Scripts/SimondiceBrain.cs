using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimondiceBrain : MonoBehaviour
{
    [Header("Los 4 Botones Físicos")]
    public MeshRenderer botonVerde;
    public MeshRenderer botonRojo;
    public MeshRenderer botonAmarillo;
    public MeshRenderer botonAzul;

    [Header("Pintura: Apagados")]
    public Material verdeApagado;
    public Material rojoApagado;
    public Material amarilloApagado;
    public Material azulApagado;

    [Header("Pintura: Encendidos")]
    public Material verdeEncendido;
    public Material rojoEncendido;
    public Material amarilloEncendido;
    public Material azulEncendido;

    [Header("Estado del Juego")]
    public List<int> secuenciaSimon = new List<int>();
    public int pasoActualJugador = 0;
    public bool turnoDelJugador = false;

    [Header("Configuración Escape Room")]
    public int rondasParaGanar = 5;
    private int rondasCompletadas = 0;
    private bool juegoTerminado = false;

    [ContextMenu("EMPEZAR NUEVO JUEGO")]
    public void IniciarJuego()
    {
        if (juegoTerminado) return;

        secuenciaSimon.Clear();
        rondasCompletadas = 0;
        IniciarNuevaRonda();
    }

    public void IniciarNuevaRonda()
    {
        turnoDelJugador = false;
        pasoActualJugador = 0;

        int nuevoColor = Random.Range(0, 4);
        secuenciaSimon.Add(nuevoColor);

        StartCoroutine(ReproducirSecuencia());
    }

    private IEnumerator ReproducirSecuencia()
    {
        yield return new WaitForSeconds(1f);

        foreach (int colorActual in secuenciaSimon)
        {
            yield return StartCoroutine(ParpadeoRutina(colorActual));
            yield return new WaitForSeconds(0.2f);
        }

        turnoDelJugador = true;
    }

    public void BotonTocadoporJugador(int colorPresionado)
    {
        if (!turnoDelJugador || juegoTerminado) return;

        StartCoroutine(ParpadeoRutina(colorPresionado));

        if (colorPresionado == secuenciaSimon[pasoActualJugador])
        {
            pasoActualJugador++;

            if (pasoActualJugador >= secuenciaSimon.Count)
            {
                turnoDelJugador = false;
                rondasCompletadas++;

                if (rondasCompletadas >= rondasParaGanar)
                {
                    Debug.Log("¡GANASTE! Puzzle superado.");
                    StartCoroutine(RutinaVictoria());
                }
                else
                {
                    Debug.Log("¡Ronda superada! Llevas " + rondasCompletadas + " de " + rondasParaGanar);
                    Invoke("IniciarNuevaRonda", 1.5f);
                }
            }
        }
        else
        {
            turnoDelJugador = false;
            Debug.Log("¡Error! Fin del juego.");
            StartCoroutine(RutinaError());
        }
    }

    private IEnumerator ParpadeoRutina(int numeroColor)
    {
        if (numeroColor == 0) botonVerde.material = verdeEncendido;
        else if (numeroColor == 1) botonRojo.material = rojoEncendido;
        else if (numeroColor == 2) botonAmarillo.material = amarilloEncendido;
        else if (numeroColor == 3) botonAzul.material = azulEncendido;

        yield return new WaitForSeconds(0.5f);

        if (numeroColor == 0) botonVerde.material = verdeApagado;
        else if (numeroColor == 1) botonRojo.material = rojoApagado;
        else if (numeroColor == 2) botonAmarillo.material = amarilloApagado;
        else if (numeroColor == 3) botonAzul.material = azulApagado;
    }

    private IEnumerator RutinaError()
    {
        botonVerde.material = rojoEncendido;
        botonRojo.material = rojoEncendido;
        botonAmarillo.material = rojoEncendido;
        botonAzul.material = rojoEncendido;

        yield return new WaitForSeconds(1.5f);

        botonVerde.material = verdeApagado;
        botonRojo.material = rojoApagado;
        botonAmarillo.material = amarilloApagado;
        botonAzul.material = azulApagado;

        secuenciaSimon.Clear();
        rondasCompletadas = 0;
    }

    private IEnumerator RutinaVictoria()
    {
        juegoTerminado = true;

        botonVerde.material = verdeEncendido;
        botonRojo.material = verdeEncendido;
        botonAmarillo.material = verdeEncendido;
        botonAzul.material = verdeEncendido;

        yield return null; // Solo esperamos un frame y dejamos todo encendido
    }
}

