using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication
{
    [RankColumn]
    public class Benchmark
    {
        [Benchmark]
        public void parallelmatrixMultiplication()
        {
            Matrix matrix = new Matrix(400, 400);
            Matrix.fillMatrix(matrix, 1);
            Matrix.parallelMultiplicateMatrix(matrix, matrix);
        }

        [Benchmark]
        public void matrixMultiplication()
        {
            Matrix matrix = new Matrix(400, 400);
            Matrix.fillMatrix(matrix, 1);
            Matrix.multiplicateMatrix(matrix, matrix);
        }
    }
}
