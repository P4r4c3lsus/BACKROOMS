using UnityEngine;
using System.Collections;

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

    [Header("Pintura: Encendidos (Brillantes)")]
    public Material verdeEncendido;
    public Material rojoEncendido;
    public Material amarilloEncendido;
    public Material azulEncendido;

    public void IluminarBoton(int numeroColor)
    {
        StartCoroutine(ParpadeoRutina(numeroColor));
    }

    private IEnumerator ParpadeoRutina(int numeroColor)
    {
        // 1. ENCENDEMOS LA LUZ correspondiente
        if (numeroColor == 0) botonVerde.material = verdeEncendido;
        else if (numeroColor == 1) botonRojo.material = rojoEncendido;
        else if (numeroColor == 2) botonAmarillo.material = amarilloEncendido;
        else if (numeroColor == 3) botonAzul.material = azulEncendido;

        // 2. ESPERAMOS medio segundo
        yield return new WaitForSeconds(0.5f);

        // 3. APAGAMOS LA LUZ de regreso
        if (numeroColor == 0) botonVerde.material = verdeApagado;
        else if (numeroColor == 1) botonRojo.material = rojoApagado;
        else if (numeroColor == 2) botonAmarillo.material = amarilloApagado;
        else if (numeroColor == 3) botonAzul.material = azulApagado;
    }

    // --- ZONA DE PRUEBAS DESDE EL INSPECTOR ---

    [ContextMenu("Probar Luz Verde")]
    public void ProbarVerde() { IluminarBoton(0); }

    [ContextMenu("Probar Luz Roja")]
    public void ProbarRojo() { IluminarBoton(1); }

    [ContextMenu("Probar Luz Amarilla")]
    public void ProbarAmarillo() { IluminarBoton(2); }

    [ContextMenu("Probar Luz Azul")]
    public void ProbarAzul() { IluminarBoton(3); }
}

