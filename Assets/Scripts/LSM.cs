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
        if (m <= 0) throw new ArgumentException("Порядок полинома должен быть больше 0");
        if (m >= X.Length) throw new ArgumentException("Порядок полинома должен быть на много меньше количества точек!");
       
        float[,] basic = new float[X.Length, m + 1];
       
        for (int i = 0; i < basic.GetLength(0); i++)
            for (int j = 0; j < basic.GetLength(1); j++)
                basic[i, j] = (float)Math.Pow(X[i], j);

        // Создание матрицы из массива значений базисных функций(МЗБФ)
        Matrix basicFuncMatr = new Matrix(basic);

        // Транспонирование МЗБФ
        Matrix transBasicFuncMatr = basicFuncMatr.Transposition();

        // Произведение транспонированного  МЗБФ на МЗБФ
        Matrix lambda = transBasicFuncMatr * basicFuncMatr;

        // Произведение транспонированого МЗБФ на следящую матрицу 
        Matrix beta = transBasicFuncMatr * new Matrix(Y);

        // Решение СЛАУ путем умножения обратной матрицы лямбда на бету
        Matrix a = lambda.InverseMatrix() * beta;

        // Присвоение значения полю класса 
        coeff = new float[a.Row];
        for (int i = 0; i < coeff.Length; i++)
        {
            coeff[i] = a.Args[i, 0];
        }
    }

    // Функция нахождения среднеквадратичного отклонения
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