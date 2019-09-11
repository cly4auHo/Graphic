using System;

static class LgApproximator
{
    public static float InterpolateLagrangePolynomial(float x, float[] xValues, float[] yValues, int size)
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