using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
	public static class SumChecker
	{
		public static string getCheckSumForDirectory(string path)
		{
			MD5 md5 = MD5.Create();
			var sw = new Stopwatch();
			List<string> sums = new List<string>();
			var dr = new DirectoryInfo(path);
			var directoryHash = md5.ComputeHash(Encoding.UTF8.GetBytes(dr.Name));
			sums.Add(Convert.ToBase64String(directoryHash));

			string[] filesPaths = Directory.GetFiles(path);
			foreach (string file in filesPaths)
			{
				var fileData = File.ReadAllBytes(file);
				var fileHash = md5.ComputeHash(fileData);
				sums.Add(Convert.ToBase64String(fileHash));
			}

			var directoriesPaths = Directory.GetDirectories(path);
			foreach (string directory in directoriesPaths)
			{
				sums.Add(getCheckSumForDirectory(directory));
			}

			sums.Sort();
			string hashsum = "";
			foreach (var sum in sums)
			{
				hashsum += sum.ToString();
			}
			return hashsum;
		}

		public static string getCheckSumForDirectoryParallel(string path)
		{
			MD5 md5 = MD5.Create();
			var sw = new Stopwatch();
			List<string> sums = new List<string>();
			var dr = new DirectoryInfo(path);
			var directoryHash = md5.ComputeHash(Encoding.UTF8.GetBytes(dr.Name));
			sums.Add(Convert.ToBase64String(directoryHash));

			string[] filesPaths = Directory.GetFiles(path);
			List<Task<string>> filesHashes = new List<Task<string>>();
			foreach (string file in filesPaths)
			{
				filesHashes.Add(Task.Run(() => getFileHash(file)));
			}

			foreach (var fileHash in filesHashes)
			{
				sums.Add(fileHash.Result);
			}

			var directoriesPaths = Directory.GetDirectories(path);
			List<Task<string>> tasks = new List<Task<string>>();
			foreach (string directory in directoriesPaths)
			{
				tasks.Add(Task.Run(() => getCheckSumForDirectory(directory)));
			}

			foreach (var task in tasks)
			{
				sums.Add(task.Result);
			}

			sums.Sort();
			string hashsum = "";
			foreach (var sum in sums)
			{
				hashsum += sum.ToString();
			}
			return hashsum;
		}

		private static async Task<string> getFileHash(string path)
		{
			MD5 md5 = MD5.Create();
			long sizeInBytes = new FileInfo(path).Length;
			if (sizeInBytes >= 1024 * 1024 * 10)
			{
				return "";
			}

			Task<byte[]> fileData;
			try
			{
				fileData = File.ReadAllBytesAsync(path);
			}
			catch (Exception e)
			{
				throw new Exception("File Reading error");
			}

			var fileHash = md5.ComputeHash(await fileData);
			return Convert.ToBase64String(fileHash);
		}
	}
}
