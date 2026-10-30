using UnityEngine;

public class ImanCriptex : MonoBehaviour
{
    private HingeJoint bisagra;
    private JointSpring resorte;
    private bool siendoAgarrado = false;

    void Start()
    {
        bisagra = GetComponent<HingeJoint>();
        bisagra.useSpring = true;

        resorte = bisagra.spring;
        resorte.spring = 15f;
        resorte.damper = 5f;
    }

    void Update()
    {
        if (!siendoAgarrado)
        {
            float anguloActual = bisagra.angle;

            float caraMasCercana = Mathf.Round(anguloActual / 90f) * 90f;

            resorte.targetPosition = caraMasCercana;
            bisagra.spring = resorte;
        }
    }

    public void AlAgarrar()
    {
        siendoAgarrado = true;
        resorte.spring = 0f;
        bisagra.spring = resorte;
    }

    public void AlSoltar()
    {
        siendoAgarrado = false;
        resorte.spring = 15f;
        bisagra.spring = resorte;
    }
}
