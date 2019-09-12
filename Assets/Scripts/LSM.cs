using System;
using System.Linq;

public class LSM
{   
    public float[] X { get; set; }
    public float[] Y { get; set; }

    private float[] coeff;
    public float[] Coeff { get { return coeff; } }
    public float? Delta { get { return getDelta(); } }
   
    public LSM(float[] x, float[] y)
    {
        if (x.Length != y.Length) throw new ArgumentException("X and Y arrays should be equal!");
        X = new float[x.Length];
        Y = new float[y.Length];

        for (int i = 0; i < x.Length; i++)
        {
            X[i] = x[i];
            Y[i] = y[i];
        }
    }
   
    public void Polynomial(int m)
    {
        if (m <= 0) throw new ArgumentException("Degree of polynomial must be more than 0");
        if (m >= X.Length) throw new ArgumentException("Order degree of polynomial must be less than a quantity of dots!");
       
        float[,] basic = new float[X.Length, m + 1];
       
        for (int i = 0; i < basic.GetLength(0); i++)
            for (int j = 0; j < basic.GetLength(1); j++)
                basic[i, j] = (float)Math.Pow(X[i], j);

        // create a Matrix
        Matrix basicFuncMatr = new Matrix(basic);

        //transpose of Matrix
        Matrix transBasicFuncMatr = basicFuncMatr.Transposition();
      
        Matrix lambda = transBasicFuncMatr * basicFuncMatr;

        Matrix beta = transBasicFuncMatr * new Matrix(Y);

        // solution of a system of linear equations
        Matrix a = lambda.InverseMatrix() * beta;
       
        coeff = new float[a.Row];
        for (int i = 0; i < coeff.Length; i++)
        {
            coeff[i] = a.Args[i, 0];
        }
    }

    private float? getDelta()
    {
        if (coeff == null) return null;
        float[] dif = new float[Y.Length];
        float[] f = new float[X.Length];

        for (int i = 0; i < X.Length; i++)
        {
            for (int j = 0; j < coeff.Length; j++)
            {
                f[i] += coeff[j] * (float)Math.Pow(X[i], j);
            }
            dif[i] = (float)Math.Pow((f[i] - Y[i]), 2);
        }
        return (float)Math.Sqrt(dif.Sum() / X.Length);
    }
}