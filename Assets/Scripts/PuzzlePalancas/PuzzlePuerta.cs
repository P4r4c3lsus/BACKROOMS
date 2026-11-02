using UnityEngine;

public class PuzzlePuerta : MonoBehaviour
{
    public HingeJoint bisagra1;
    public HingeJoint bisagra2;
    public HingeJoint bisagra3;
    public GameObject puerta;
    
    public float velocidadRotacion = 90f;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    private bool puertaAbierta;
    private int estado1;
    private int estado2;
    private int estado3;

    void Start()
    {        
        rotacionCerrada = puerta.transform.rotation;
        estado1 = EstadoPalanca(bisagra1.angle, 0);
        estado2 = EstadoPalanca(bisagra2.angle, 0);
        estado3 = EstadoPalanca(bisagra3.angle, 0);


        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0f, 90f, 0f);
    }

    void Update()
    {
        bool p1Abajo = bisagra1.angle > 30f;
        bool p3Abajo = bisagra3.angle > 30f;
        bool p2Arriba = bisagra2.angle < -30f;

        SonarPalanca(bisagra1, ref estado1);
        SonarPalanca(bisagra2, ref estado2);
        SonarPalanca(bisagra3, ref estado3);

        bool debeAbrir = p1Abajo && p2Arriba && p3Abajo;
        if (debeAbrir != puertaAbierta)
        {
            puertaAbierta = debeAbrir;
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("LeverDoor");
        }

        if (debeAbrir)
        {            
            puerta.transform.rotation = Quaternion.RotateTowards(puerta.transform.rotation, rotacionAbierta, velocidadRotacion * Time.deltaTime);
        }
        else
        {            
            puerta.transform.rotation = Quaternion.RotateTowards(puerta.transform.rotation, rotacionCerrada, velocidadRotacion * Time.deltaTime);
        }
    }
    // Mantiene el estado entre extremos para evitar sonidos por vibraciones.
    private int EstadoPalanca(float angulo, int anterior)
    {
        if (angulo > 30f) return 1;
        if (angulo < -30f) return -1;
        return anterior;
    }

    private void SonarPalanca(HingeJoint bisagra, ref int estado)
    {
        int nuevoEstado = EstadoPalanca(bisagra.angle, estado);
        if (nuevoEstado == estado) return;
        estado = nuevoEstado;
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("LeverMove");
    }
}

//https://docs.unity3d.com/6000.3/Documentation/Manual/class-HingeJoint.html