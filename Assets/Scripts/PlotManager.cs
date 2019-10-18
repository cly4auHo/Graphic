using UnityEngine;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour
{
    [SerializeField] private int scale = 200;
    [SerializeField] private float screenSize = 10;
    [SerializeField] private Transform[] dots;
    [SerializeField] private int degreeOrder = 3;
    [SerializeField] private LineRenderer lsmLineRenderer;
    [SerializeField] private LineRenderer langrangeLineRenderer;

    delegate Vector2 Function(float x, float y);
    private Function lsmfunction;
    private Function lagrangefunction;

    private LSM lsm;
    private LgApproximator lagrangeproximator;

    void Start()
    {
        InitalizeLineRenderer();
    }

    void Update()
    {
        CalculateLSM();
        CalculateLagrange();
        PopulateGraph();
    }

    void CalculateLagrange()
    {
        var xArr = new float[dots.Length];
        var yArr = new float[dots.Length];

        for (int i = 0; i < dots.Length; i++)
        {
            xArr[i] = dots[i].position.x;
            yArr[i] = dots[i].position.y;
        }

        lagrangeproximator = new LgApproximator(xArr, yArr, degreeOrder + 1);
        
        lagrangefunction = (x, y) =>
        {
            float yValue = 0;

            for (int i = 0; i < degreeOrder; i++)
            {
                yValue = lagrangeproximator.InterpolateLagrangePolynomial(x);
            }
            return new Vector2(x, yValue);
        };
    }

    void CalculateLSM()
    {
        var xArr = new float[dots.Length];
        var yArr = new float[dots.Length];

        for (int i = 0; i < dots.Length; i++)
        {
            xArr[i] = dots[i].position.x;
            yArr[i] = dots[i].position.y;
        }

        lsm = new LSM(xArr, yArr);
        lsm.Polynomial(degreeOrder);

        lsmfunction = (x, y) =>
        {
            float yValue = 0;

            for (int i = 0; i < degreeOrder; i++)
            {
                yValue += Mathf.Pow(x, i) * lsm.Coeff[i];
            }
            return new Vector2(x, yValue);
        };
    }

    void InitalizeLineRenderer()
    {
        lsmLineRenderer.positionCount = scale * 2;
        langrangeLineRenderer.positionCount = scale * 2;
    }

    void PopulateGraph()
    {
        float step = screenSize / scale;

        for (int i = -scale; i < scale; i++)
        {
            var lsmPointPosition = lsmfunction(i * step, i * step);
            var lgPosition = lagrangefunction(i * step, i * step);

            lsmLineRenderer.SetPosition(i + scale, lsmPointPosition);
            langrangeLineRenderer.SetPosition(i + scale, lgPosition);
        }
    }
}
