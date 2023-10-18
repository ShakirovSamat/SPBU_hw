using MatrixMultiplication;
using MatrixMultiplication.Exceptions;

namespace MatrixMultiplicationTests
{
    public class Tests
    {
        [Test]
        public void NonMultiplicableMatricesTest()
        {
            Matrix matrix1 = new Matrix(5, 5);
            Matrix matrix2 = new Matrix(6, 6);
            Assert.Throws<MatrixMultiplicationException>(() => Matrix.parallelMultiplicateMatrix(matrix1, matrix2));
        }

        [Test]
        public void BadElementInMAtrixTest()
        {
            String[,] array = new String[,] { { "1", "5.5" }, { "1", "1" } };
            Assert.Throws<BadMatrixElementException>(() => new Matrix(array));
        }

        [Test]
        public void NotAMatrixTest()
        {
            String[,] array = new String[,] { { "1", String.Empty }, { "1", "1" } };
            Assert.Throws<NotAMatrixException>(() => new Matrix(array));
        }
        [Test]
        public void SimpleMatrixMultiplicationTest()
        {
            int[,] array = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Matrix matrix = new Matrix(array);
            Matrix result = Matrix.parallelMultiplicateMatrix(matrix, matrix);
            bool isRight = true;
            for (int i = 0; i < result.Hight; ++i)
            {
                for (int j = 0; j < result.Width; ++j)
                {
                    Assert.AreEqual(result.MatrixTable[i, j], 3);
                }
            }
        }

        [Test]
        public void MatrixMultiolicationTest()
        {
            int[,] array1 = new int[,] { { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } };
            int[,] array2 = new int[,] { { 4, 5, 6 }, { 4, 5, 6 }, { 4, 5, 6 } };
            Matrix matrix1 = new Matrix(array1);
            Matrix matrix2 = new Matrix(array2);
            Matrix result = Matrix.parallelMultiplicateMatrix(matrix1, matrix2);
            int[,] expectedResult = new int[,] { { 24, 30, 36 }, { 24, 30, 36 }, { 24, 30, 36 } };
            bool isRight = true;
            for (int i = 0; i < result.Hight; ++i)
            {
                for (int j = 0; j < result.Width; ++j)
                {
                    Assert.AreEqual(result.MatrixTable[i, j], expectedResult[i, j]);
                }
            }
        }
    }
}