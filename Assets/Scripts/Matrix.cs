using System;

public class Matrix
{
    public float[,] Args { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public Matrix(float[] x)
    {
        Row = x.Length;
        Col = 1;

        Args = new float[Row, Col];

        for (int i = 0; i < Args.GetLength(0); i++)
            for (int j = 0; j < Args.GetLength(1); j++)
                Args[i, j] = x[i];
    }

    public Matrix(float[,] x)
    {
        Row = x.GetLength(0);
        Col = x.GetLength(1);

        Args = new float[Row, Col];

        for (int i = 0; i < Args.GetLength(0); i++)
            for (int j = 0; j < Args.GetLength(1); j++)
                Args[i, j] = x[i, j];
    }

    public Matrix(Matrix other)
    {
        this.Row = other.Row;
        this.Col = other.Col;

        Args = new float[Row, Col];

        for (int i = 0; i < Row; i++)
            for (int j = 0; j < Col; j++)
                this.Args[i, j] = other.Args[i, j];
    }

    public Matrix Transposition()
    {
        float[,] t = new float[Col, Row];

        for (int i = 0; i < Row; i++)
            for (int j = 0; j < Col; j++)
                t[j, i] = Args[i, j];

        return new Matrix(t);
    }

    public static Matrix operator *(Matrix m, float k)
    {
        Matrix ans = new Matrix(m);

        for (int i = 0; i < ans.Row; i++)
            for (int j = 0; j < ans.Col; j++)
                ans.Args[i, j] = m.Args[i, j] * k;

        return ans;
    }

    public static Matrix operator *(Matrix m1, Matrix m2)
    {
        if (m1.Col != m2.Row) throw new ArgumentException("Multiplication of these two matrices can't be done!");
        float[,] ans = new float[m1.Row, m2.Col];

        for (int i = 0; i < m1.Row; i++)
        {
            for (int j = 0; j < m2.Col; j++)
            {
                for (int k = 0; k < m2.Row; k++)
                {
                    ans[i, j] += m1.Args[i, k] * m2.Args[k, j];
                }
            }
        }

        return new Matrix(ans);
    }

    private Matrix getMinor(int row, int column)
    {
        if (Row != Col) throw new ArgumentException("Matrix should be square!");
        float[,] minor = new float[Row - 1, Col - 1];

        for (int i = 0; i < this.Row; i++)
        {
            for (int j = 0; j < this.Col; j++)
            {
                if ((i != row) || (j != column))
                {
                    if (i > row && j < column) minor[i - 1, j] = this.Args[i, j];
                    if (i < row && j > column) minor[i, j - 1] = this.Args[i, j];
                    if (i > row && j > column) minor[i - 1, j - 1] = this.Args[i, j];
                    if (i < row && j < column) minor[i, j] = this.Args[i, j];
                }
            }
        }
        return new Matrix(minor);
    }

    public static float Determ(Matrix m)
    {
        if (m.Row != m.Col) throw new ArgumentException("Matrix should be square!");
        float det = 0;
        int length = m.Row;

        if (length == 1) det = m.Args[0, 0];
        if (length == 2) det = m.Args[0, 0] * m.Args[1, 1] - m.Args[0, 1] * m.Args[1, 0];

        if (length > 2)
            for (int i = 0; i < m.Col; i++)
                det += (float)Math.Pow(-1, 0 + i) * m.Args[0, i] * Determ(m.getMinor(0, i));

        return det;
    }

    public Matrix MinorMatrix()
    {
        float[,] ans = new float[Row, Col];

        for (int i = 0; i < Row; i++)
            for (int j = 0; j < Col; j++)
                ans[i, j] = (float)Math.Pow(-1, i + j) * Determ(this.getMinor(i, j));

        return new Matrix(ans);
    }

    public Matrix InverseMatrix()
    {
        if (Math.Abs(Determ(this)) <= 0.000000001) throw new ArgumentException("Inverse matrix does not exist!");
        float k = 1 / Determ(this);
        Matrix minorMatrix = this.MinorMatrix();
        return minorMatrix * k;
    }
}