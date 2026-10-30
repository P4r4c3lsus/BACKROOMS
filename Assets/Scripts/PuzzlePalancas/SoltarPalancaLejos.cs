using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SoltarPalancaLejos : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    [Header("Configuración")]
    [Tooltip("Distancia máxima en metros antes de que se suelte automáticamente")]
    public float distanciaMaxima = 0.4f;

    void Start()
    {        
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {        
        if (grabInteractable.isSelected)
        {            
            IXRSelectInteractor interactor = grabInteractable.firstInteractorSelecting;

            if (interactor != null)
            {                
                float distancia = Vector3.Distance(interactor.transform.position, transform.position);
                
                if (distancia > distanciaMaxima)
                {                    
                    XRInteractionManager manager = grabInteractable.interactionManager;
                    manager.SelectCancel(interactor, grabInteractable);
                }
            }
        }
    }
}
