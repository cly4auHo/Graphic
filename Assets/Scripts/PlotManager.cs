﻿using UnityEngine;
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
    private Function function;
    private LSM lsm;
    
    void Start()
    {
        InitalizeLineRenderer();
    }

    void Update()
    {
        CalculateLSM();
        PopulateGraph();
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

        function = (x, y) =>
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

        var xArr = new float[dots.Length];
        var yArr = new float[dots.Length];

        for (int i = 0; i < dots.Length; i++)
        {
            xArr[i] = dots[i].position.x;
            yArr[i] = dots[i].position.y;
        }

        for (int i = -scale; i < scale; i++)
        {
            var lsmPointPosition = function(i * step, i * step);
            var lgPosition = new Vector3(i * step, LgApproximator.InterpolateLagrangePolynomial(i * step, xArr, yArr, degreeOrder + 1));
            lsmLineRenderer.SetPosition(i + scale, lsmPointPosition);
            langrangeLineRenderer.SetPosition(i + scale, lgPosition);
        }
    }
}