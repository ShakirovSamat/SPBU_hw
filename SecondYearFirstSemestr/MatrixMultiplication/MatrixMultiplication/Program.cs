using MatrixMultiplication;
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
        public static Matrix CreateMatrix(String message)
        {
            while (true)
            {
                Console.Write(message);
                try
                {
                    return new Matrix(Console.ReadLine());
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

        public static void Main(String[] args)
        {
            Console.WriteLine("*****Matrix Multiplication Application*****");
            Console.WriteLine("This program multiplicates two matrix");

            var firstMatrix = CreateMatrix("Enter path to the first matrix: ");
            var secondMatrix = CreateMatrix("Enter path to the second matrix: ");

           
            var multiplicatedMatrix = Matrix.parallelMultiplicateMatrix(firstMatrix, secondMatrix);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.WriteLine("The result will be saved to Desktop");
            while (true)
            {
                Console.Write("Enter name of the saving file: ");
                var name = Console.ReadLine().Trim();
                try
                {
                    Matrix.writeMatrixToFile(multiplicatedMatrix, path + "\\" + name + ".txt");
                    break;
                }
                catch (FileAlreadyExistException)
                {
                    Console.WriteLine("Such file already exists");
                }
            }

            Console.ReadLine();
        }
    }
}