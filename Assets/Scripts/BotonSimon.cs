using UnityEngine;

public class BotonSimon : MonoBehaviour
{
    [Header("Configuración")]
    // Aquí le diremos a cada botón qué color es (0, 1, 2 o 3)
    public int miColor;

    // Conexión al cerebro principal
    public SimondiceBrain cerebro;

    // Esta función la mandará a llamar nuestro sistema de VR cuando toquemos el botón
    public void AlPresionar()
    {
        Debug.Log("El jugador tocó el botón: " + miColor);

        // Le avisamos al cerebro qué color tocamos
        cerebro.BotonTocadoporJugador(miColor);
    }
}
