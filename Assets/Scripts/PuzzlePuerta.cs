using UnityEngine;

public class PuzzlePuerta : MonoBehaviour
{
    public HingeJoint bisagra1;
    public HingeJoint bisagra2;
    public HingeJoint bisagra3;
    public GameObject puerta;

    void Update()
    {
        bool p1Abajo = bisagra1.angle > 30f;
        bool p3Abajo = bisagra3.angle > 30f;
        bool p2Arriba = bisagra2.angle < -30f;

        if (p1Abajo && p2Arriba && p3Abajo)
        {
            puerta.SetActive(false);
        }
        else
        {
            puerta.SetActive(true);
        }
    }
}

//https://docs.unity3d.com/6000.3/Documentation/Manual/class-HingeJoint.html