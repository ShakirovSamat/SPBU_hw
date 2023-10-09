using BenchmarkDotNet.Attributes;
using MatrixMultiplication.Exceptions;

namespace MatrixMultiplication
{
    public class Matrix
    {
        public int[,]? MatrixTable { private set; get; }
        public int Width { private set; get; }
        public int Hight { private set; get; }

        public Matrix(int width, int hight)
        {
            Width = width;
            Hight = hight;
            MatrixTable = new int[width, hight];
        }

        public Matrix(String[,] array)
        {
            Hight = array.GetLength(0);
            Width = array.GetLength(1);
            MatrixTable = new int[Hight, Width];
            for (int i = 0; i < Hight; ++i)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (array[i, j] == String.Empty)
                    {
                        throw new NotAMatrixException();
                    }

                    if (!int.TryParse(array[i, j], out MatrixTable[i, j]))
                    {
                        throw new BadMatrixElementException();
                    }
                }
            }
        }

        public Matrix(int[,] array)
        {
            Hight = array.GetLength(0);
            Width = array.GetLength(1);
            MatrixTable = array;
        }

        /// <summary>
        /// fills the matrix with a number
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="number"></param>
        public static void fillMatrix(Matrix matrix, int number)
        {
            if (matrix == null || matrix.MatrixTable == null) return;

            for (int i = 0; i < matrix.Hight; ++i)
            {
                for (int j = 0; j < matrix.Width; ++j)
                {
                    matrix.MatrixTable[i, j] = number;
                }
            }
        }

        [Benchmark]
        /// <summary>
        /// multiplicates two matrices by parallel algorithm
        /// </summary>
        /// <param name="firstMatrix"></param>
        /// <param name="secondMatrix"></param>
        /// <returns></returns>
        /// <exception cref="MatrixMultiplicationException"></exception>
        public static Matrix parallelMultiplicateMatrix(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix == null || secondMatrix == null || firstMatrix.MatrixTable == null || secondMatrix.MatrixTable == null) return null;

            if (firstMatrix.Width != secondMatrix.Hight) throw new MatrixMultiplicationException();

            var multiplicatedMatrix = new int[firstMatrix.Hight, secondMatrix.Width];

            var threadsCount = Math.Min(Environment.ProcessorCount, firstMatrix.Hight);
            var threads = new Thread[threadsCount];
            var linesForEachThread = (int)Math.Ceiling((double)firstMatrix.Hight / threadsCount);

            for (int i = 0; i < threads.Length; ++i)
            {
                int localI = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = linesForEachThread * localI; j < linesForEachThread * (localI + 1) && j < firstMatrix.Hight; ++j)
                    {
                        for (int a = 0; a < secondMatrix.Width; ++a)
                        {
                            for (int b = 0; b < firstMatrix.Width; ++b)
                            {
                                int element = firstMatrix.MatrixTable[j, b];
                                element *= secondMatrix.MatrixTable[b, a];
                                multiplicatedMatrix[j, a] += element;
                            }
                        }

                    }
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            return new Matrix(multiplicatedMatrix);
        }

        /// <summary>
        /// multiplicates two matrices
        /// </summary>
        /// <param name="firstMatrix"></param>
        /// <param name="secondMatrix"></param>
        /// <returns></returns>
        /// <exception cref="MatrixMultiplicationException"></exception>
        public static Matrix multiplicateMatrix(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix == null || secondMatrix == null || firstMatrix.MatrixTable == null || secondMatrix.MatrixTable == null) return null;

            if (firstMatrix.Width != secondMatrix.Hight) throw new MatrixMultiplicationException();

            var multiplicatedMatrix = new int[firstMatrix.Hight, secondMatrix.Width];
            for (int i = 0; i < firstMatrix.Hight; ++i)
            {
                for (int j = 0; j < secondMatrix.Width; ++j)
                {
                    for (int b = 0; b < firstMatrix.Hight; ++b)
                    {
                        int element = firstMatrix.MatrixTable[i, b];
                        element *= secondMatrix.MatrixTable[b, j];
                        multiplicatedMatrix[i, j] += element;
                    }
                }
            }

            return new Matrix(multiplicatedMatrix);
        }

        /// <summary>
        /// writes matrix to the file
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="path"></param>
        /// <exception cref="FileAlreadyExistException"></exception>
        public static void writeMatrixToFile(Matrix matrix, String path)
        {
            if (matrix == null || matrix.MatrixTable == null) return;

            if (File.Exists(path)) throw new FileAlreadyExistException();

            File.Create(path).Close();
            StreamWriter writer = new StreamWriter(path);
            for (var i = 0; i < matrix.Hight; ++i)
            {
                for (var j = 0; j < matrix.Width; ++j)
                {
                    writer.Write(matrix.MatrixTable[i, j] + " ");
                }
                writer.Write("\n");
            }
            writer.Close();
        }
    }
}
