using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO.Packaging;
using System.Xml.Linq;

namespace Streams
{
	public static class StreamTask
	{
		/// <summary>
		/// Parses Resources\Planets.xlsx file and returns the planet data: 
		///   Jupiter     69911.00
		///   Saturn      58232.00
		///   Uranus      25362.00
		///    ...
		/// See Resources\Planets.xlsx for details
		/// </summary>
		/// <param name="xlsxFileName">Source file name.</param>
		/// <returns>Sequence of PlanetInfo</returns>
		public static IEnumerable<PlanetInfo> ReadPlanetInfoFromXlsx(string xlsxFileName)
		{
			// TODO : Implement ReadPlanetInfoFromXlsx method using System.IO.Packaging + Linq-2-Xml

			// HINT : Please be as simple & clear as possible.
			//        No complex and common use cases, just this specified file.
			//        Required data are stored in Planets.xlsx archive in 2 files:
			//         /xl/sharedStrings.xml      - dictionary of all string values
			//         /xl/worksheets/sheet1.xml  - main worksheet

			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates hash of stream using specified algorithm.
		/// </summary>
		/// <param name="stream">Source stream</param>
		/// <param name="hashAlgorithmName">
		///     Hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET).
		/// </param>
		/// <returns></returns>
		public static string CalculateHash(this Stream stream, string hashAlgorithmName)
		{
			using var algorithm = HashAlgorithm.Create(hashAlgorithmName);
			if (algorithm is null)
			{
				throw new ArgumentException($"{nameof(hashAlgorithmName)} is invalid.");
			}

			var data = algorithm.ComputeHash(stream);
			return BitConverter.ToString(data).Replace("-", string.Empty);
		}

		/// <summary>
		/// Returns decompressed stream from file. 
		/// </summary>
		/// <param name="fileName">Source file.</param>
		/// <param name="method">Method used for compression (none, deflate, gzip).</param>
		/// <returns>output stream</returns>
		public static Stream DecompressStream(string fileName, DecompressionMethods method)
		{
			var bytes = File.ReadAllBytes(fileName);
			var memoryStream = new MemoryStream();
			switch (method)
			{
				case DecompressionMethods.GZip:
					using (var decompressor = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
					{
						decompressor.CopyTo(memoryStream);
					}

					memoryStream.Seek(0, SeekOrigin.Begin);
					return memoryStream;

				case DecompressionMethods.Deflate:
					using (var decompressor = new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress))
					{
						decompressor.CopyTo(memoryStream);
					}

					memoryStream.Seek(0, SeekOrigin.Begin);
					return memoryStream;

				case DecompressionMethods.Brotli:
					using (var decompressor = new BrotliStream(new MemoryStream(bytes), CompressionMode.Decompress))
					{
						decompressor.CopyTo(memoryStream);
					}

					memoryStream.Seek(0, SeekOrigin.Begin);
					return memoryStream;

				case DecompressionMethods.None:
					return new MemoryStream(bytes);

				default:
					throw new ArgumentException();
			}
		}

		/// <summary>
		/// Reads file content encoded with non Unicode encoding
		/// </summary>
		/// <param name="fileName">Source file name</param>
		/// <param name="encoding">Encoding name</param>
		/// <returns>Unicoded file content</returns>
		public static string ReadEncodedText(string fileName, string encoding)
		{
			using var reader = new StreamReader(fileName, Encoding.GetEncoding(encoding));
			return reader.ReadToEnd();
		}
	}
}