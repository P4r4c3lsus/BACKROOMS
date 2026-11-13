using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ImanCriptex : MonoBehaviour
{
    private HingeJoint bisagra;
    private JointSpring resorte;
    private bool siendoAgarrado = false;
    private XRGrabInteractable grab;

    void Awake()
    {
        bisagra = GetComponent<HingeJoint>();
        bisagra.useSpring = true;

        resorte = bisagra.spring;
        resorte.spring = 15f;
        resorte.damper = 5f;
    }

    private void OnEnable()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (grab == null) return;
        grab.selectEntered.AddListener(Grabbed);
        grab.selectExited.AddListener(Released);
    }

    private void OnDisable()
    {
        if (grab == null) return;
        grab.selectEntered.RemoveListener(Grabbed);
        grab.selectExited.RemoveListener(Released);
    }

    private void Grabbed(SelectEnterEventArgs args) => AlAgarrar();
    private void Released(SelectExitEventArgs args) => AlSoltar();

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
