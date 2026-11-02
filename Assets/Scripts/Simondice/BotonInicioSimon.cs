using UnityEngine;

public class BotonInicioSimon : MonoBehaviour
{
    [Header("Conexión al Cerebro")]
    public SimondiceBrain cerebro;

    public void AlPresionar()
    {
        Debug.Log("¡Botón de Inicio presionado!");

        cerebro.IniciarJuego();
    }
}