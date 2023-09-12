
using MatrixMultiplication.Exceptions;

namespace MatrixMultiplicationApp
{
    class Program
    {
        /// <summary>
        /// gets matrix from user
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int[,] getMatrix(String message)
        {
            while (true)
            {
                Console.Write(message);
                try
                {
                    return readMatrixFromFile(Console.ReadLine());
                }
                catch (Exception e)
                {
                    switch (e)
                    {
                        case FileNotFoundException:
                            Console.WriteLine("Wrong path. File doesn't exist");
                            break;
                        case FileIsEmptyException:
                            Console.WriteLine("The file is empty");
                            break;
                        case NotAMatrixException:
                            Console.WriteLine("The file doesn't contatin matrix");
                            break;
                        case BadMatrixElementException:
                            Console.WriteLine("Bad element in matrix. Matrix should contain only integers");
                            break;
                        default:
                            Console.WriteLine("Bad input");
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     reads matrix consisting of integers from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="FileIsEmptyException"></exception>
        /// <exception cref="NotAMatrixException"></exception>
        /// <exception cref="BadMatrixElementException"></exception>
        public static int[,] readMatrixFromFile(String path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            StreamReader streamReader = new StreamReader(path);
            String fileData = streamReader.ReadToEnd();
            if (fileData.Length == 0)
            {
                streamReader.Close();
                throw new FileIsEmptyException();
            }

            String[] matrixLines = fileData.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            int matrixHight = matrixLines.Length;
            int matrixWidth = matrixLines[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Length;
            int[,] matrix = new int[matrixHight, matrixWidth];
            for (int i = 0; i < matrixHight; ++i)
            {
                String[] numbers = matrixLines[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (numbers.Length != matrixWidth)
                {
                    streamReader.Close();
                    throw new NotAMatrixException();
                }

                for (int j = 0; j < matrixWidth; ++j)
                {
                    if (!Int32.TryParse(numbers[j], out matrix[i, j]))
                    {
                        streamReader.Close();
                        throw new BadMatrixElementException();
                    }
                }
            }

            streamReader.Close();
            return matrix;
        }

        public static int[,] multiplicateMatrix(int[,] firstMatrix, int[,] secondMatrix)
        {
            if (firstMatrix.GetLength(1) != secondMatrix.GetLength(2)) throw new MatrixMultiplicationException();

            return null;
        }

        public static void Main(String[] args)
        {
            Console.WriteLine("*****Matrix Multiplication Application*****");
            Console.WriteLine("This program multiplicates two matrix");

            int[,] firstMatrix = getMatrix("Enter path to the first matrix: ");
            int[,] secondMatrix = getMatrix("Enter path to the second matrix: ");
        }
    }
}