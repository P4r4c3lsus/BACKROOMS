using UnityEngine;

public class BotonSimon : MonoBehaviour
{
    [Header("Configuración")]
    public int miColor;

    public SimondiceBrain cerebro;

    public void AlPresionar()
    {
        Debug.Log("El jugador tocó el botón: " + miColor);

        cerebro.BotonTocadoporJugador(miColor);
    }
}
