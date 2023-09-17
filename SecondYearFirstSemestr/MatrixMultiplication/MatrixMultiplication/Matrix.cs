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

                    if (!int.TryParse(array[i ,j], out MatrixTable[i, j]))
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

        /// <summary>
        /// multiplicates two matrices
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
            var threads = new Thread[firstMatrix.Hight];
            for (int i = 0; i < firstMatrix.Hight; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < secondMatrix.Width; ++j)
                    {
                        for (int b = 0; b < firstMatrix.Width; ++b)
                        {
                            int element = firstMatrix.MatrixTable[localI, b];
                            element *= secondMatrix.MatrixTable[b, j];
                            multiplicatedMatrix[localI, j] += element;
                        }
                    }
                });
                threads[i].Priority = ThreadPriority.Highest;
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
            Console.WriteLine("The file have been created and saved");
        }
    }
}
