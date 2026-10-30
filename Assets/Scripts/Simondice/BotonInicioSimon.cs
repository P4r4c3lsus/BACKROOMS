using UnityEngine;

public class BotonInicioSimon : MonoBehaviour
{
    [Header("Conexión al Cerebro")]
    public SimondiceBrain cerebro;

    // Esta función la llamará tu mano VR al tocar/presionar el botón
    public void AlPresionar()
    {
        Debug.Log("¡Botón de Inicio presionado!");

        // Le damos la orden al cerebro de arrancar
        cerebro.IniciarJuego();
    }
}