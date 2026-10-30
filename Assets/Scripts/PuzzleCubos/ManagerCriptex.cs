using UnityEngine;

public class ManagerCriptex : MonoBehaviour
{   
    public HingeJoint cubo1;
    public HingeJoint cubo2;
    public HingeJoint cubo3;
    public HingeJoint cubo4;
    
    public float anguloCorrecto1 = 0f;
    public float anguloCorrecto2 = 0f;
    public float anguloCorrecto3 = 0f;
    public float anguloCorrecto4 = 0f;
    
    public float margenError = 15f;
    private bool puzzleResuelto = false;

    void Update()
    {
        if (puzzleResuelto) return;

        bool c1_Ok = EvaluarCubo(cubo1, anguloCorrecto1);
        bool c2_Ok = EvaluarCubo(cubo2, anguloCorrecto2);
        bool c3_Ok = EvaluarCubo(cubo3, anguloCorrecto3);
        bool c4_Ok = EvaluarCubo(cubo4, anguloCorrecto4);

        if (c1_Ok && c2_Ok && c3_Ok && c4_Ok)
        {
            puzzleResuelto = true;
            Debug.Log("Se abrió la puerta.");

        }
    }

    bool EvaluarCubo(HingeJoint cubo, float anguloObjetivo)
    {
        float diferencia = Mathf.Abs(Mathf.DeltaAngle(cubo.angle, anguloObjetivo));
        return diferencia <= margenError;
    }
}
