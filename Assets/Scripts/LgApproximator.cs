using System;

public class LgApproximator
{
    private float[] xValues;
    private float[] yValues;
    int size;

    public LgApproximator(float[] xValues, float[] yValues, int size)
    {
        this.xValues = xValues;
        this.yValues = yValues;
        this.size = size;
    }

    public float InterpolateLagrangePolynomial(float x)
    {
        float lagrangePol = 0;

        for (int i = 0; i < size; i++)
        {
            float basicsPol = 1;

            for (int j = 0; j < size; j++)
            {
                if (j != i)
                {
                    basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                }
            }
            lagrangePol += basicsPol * yValues[i];
        }
        return lagrangePol;
    }
}