using MatrixMultiplication;
using System.Diagnostics;

namespace MatrixMultiplicationTests
{
    public class Tests
    {
        Matrix matrix;
        int n;
        [SetUp]
        public void Setup()
        {
            matrix = new Matrix(500, 500);
            Matrix.fillMatrix(matrix, 1);
            n = 10;
        }

        [Test]
        public void AvarageTimeOfParallelMultiplication()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < n; ++i)
            {
                Matrix.parallelMultiplicateMatrix(matrix, matrix);
            }

            sw.Stop();
            TestContext.Out.WriteLine("Parallel multiplication time: " + sw.Elapsed.TotalSeconds / n);
            Assert.Pass();
        }

        [Test]
        public void AvarageTimeOfSimplelMultiplication()
        {
            Matrix.fillMatrix(matrix, 1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < n; ++i)
            {
                Matrix.multiplicateMatrix(matrix, matrix);
            }

            sw.Stop();
            TestContext.Out.WriteLine("Simple multiplication time: " + sw.Elapsed.TotalSeconds / n);
            Assert.Pass();
        }

        [Test]
        public void ExpectedValueTest()
        {

        }
    
    }
}