using UnityEngine;

public class ImanCriptex : MonoBehaviour
{
    private HingeJoint bisagra;
    private JointSpring resorte;

    void Start()
    {
        bisagra = GetComponent<HingeJoint>();
        
        bisagra.useSpring = true;
        
        resorte = bisagra.spring;
        resorte.spring = 10f;
        resorte.damper = 10f;
    }

    void Update()
    {        
        float anguloActual = bisagra.angle;
        
        float caraMasCercana = Mathf.Round(anguloActual / 90f) * 90f;
        
        resorte.targetPosition = caraMasCercana;
        bisagra.spring = resorte;
    }
}
