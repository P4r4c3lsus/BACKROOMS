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

    void Start()
    {        
        rotacionCerrada = puerta.transform.rotation;


        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0f, 90f, 0f);
    }

    void Update()
    {
        bool p1Abajo = bisagra1.angle > 30f;
        bool p3Abajo = bisagra3.angle > 30f;
        bool p2Arriba = bisagra2.angle < -30f;

        if (p1Abajo && p2Arriba && p3Abajo)
        {            
            puerta.transform.rotation = Quaternion.RotateTowards(puerta.transform.rotation, rotacionAbierta, velocidadRotacion * Time.deltaTime);
        }
        else
        {            
            puerta.transform.rotation = Quaternion.RotateTowards(puerta.transform.rotation, rotacionCerrada, velocidadRotacion * Time.deltaTime);
        }
    }
}

//https://docs.unity3d.com/6000.3/Documentation/Manual/class-HingeJoint.html