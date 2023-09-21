using BenchmarkDotNet.Running;
using MatrixMultiplication;
using MatrixMultiplication.Exceptions;
using System.IO;
using System.Net.Http.Headers;

namespace MatrixMultiplicationApp
{
    class Program
    {
        /// <summary>
        /// gets two dementional array from user
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static String[,] getTwoDementionalArrayFromFile()
        {
            while (true)
            {
                try
                {
                    String path = Console.ReadLine();
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

                    String[] arrayLines = fileData.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    int arrayHight = arrayLines.Length; ;
                    int arrayWidth = arrayLines[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Length;
                    String[,] array = new String[arrayWidth, arrayHight];
                    for (int i = 0; i < arrayHight; ++i)
                    {
                        String[] arrayLineValues = arrayLines[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < arrayWidth; ++j)
                        {
                            array[i, j] = arrayLineValues[j];
                        }
                    }

                    return array;
                }
                catch (Exception e)
                {
                    switch (e)
                    {
                        case FileNotFoundException:
                            Console.WriteLine("Wrong path. File doesn't exist. Try enter matrix again: ");
                            break;
                        case FileIsEmptyException:
                            Console.WriteLine("The file is empty. Try enter matrix again: ");
                            break;
                        case NotAMatrixException:
                            Console.WriteLine("The file doesn't contatin array. Try enter matrix again: ");
                            break;
                        case BadMatrixElementException:
                            Console.WriteLine("Bad element in array. Matrix should contain only integers. Try enter matrix again: ");
                            break;
                        default:
                            Console.WriteLine("Bad input. Try enter matrix again: ");
                            break;
                    }
                }
            }
        }

        public static void Main(String[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
            Console.WriteLine("*****Matrix Multiplication Application*****");
            Console.WriteLine("This program multiplicates two array");

            Console.WriteLine("Enter path to the first array: ");
            var firstTwoDementionalArray = getTwoDementionalArrayFromFile();

            Console.WriteLine("Enter path to the second array: ");
            var secondTwoDementionalArray = getTwoDementionalArrayFromFile();

            var firstMatrix = new Matrix(firstTwoDementionalArray);
            var secondMatrix = new Matrix(secondTwoDementionalArray);

            var multiplicatedMatrix = Matrix.parallelMultiplicateMatrix(firstMatrix, secondMatrix);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Console.WriteLine("The result will be saved to Desktop");
            while (true)
            {
                Console.Write("Enter name of the saving file: ");
                var name = Console.ReadLine().Trim();
                if (name == null) continue;
                
                try
                {
                    Matrix.writeMatrixToFile(multiplicatedMatrix, path + "\\" + name + ".txt");
                    Console.WriteLine("The file have been created and saved");
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