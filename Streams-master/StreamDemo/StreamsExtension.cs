using System;
using System.IO;
using System.Text;

namespace StreamDemo
{
    // C# 6.0 in a Nutshell. Joseph Albahari, Ben Albahari. O'Reilly Media. 2015
    // Chapter 15: Streams and I/O
    // Chapter 6: Framework Fundamentals - Text Encodings and Unicode
    // https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=netcore-3.0
    // https://docs.microsoft.com/en-us/dotnet/api/system.io?view=netcore-3.0

    public static class StreamsExtension
    {
        #region Public members

        #region TODO: Implement by byte copy logic using class FileStream as a backing store stream .

        public static int ByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            using var reader = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using var writer = new FileStream(destinationPath, FileMode.OpenOrCreate, FileAccess.Write);

            for (int i = 0; i < reader.Length; i++)
            {
                writer.WriteByte((byte)reader.ReadByte());
            }

            return (int)reader.Length;
        }

        #endregion

        #region TODO: Implement by byte copy logic using class MemoryStream as a backing store stream.

        public static int InMemoryByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            // TODO: step 1. Use StreamReader to read entire file in string

            // TODO: step 2. Create byte array on base string content - use  System.Text.Encoding class

            // TODO: step 3. Use MemoryStream instance to read from byte array (from step 2)

            // TODO: step 4. Use MemoryStream instance (from step 3) to write it content in new byte array

            // TODO: step 5. Use Encoding class instance (from step 2) to create char array on byte array content

            // TODO: step 6. Use StreamWriter here to write char array content in new file

            using var reader = new StreamReader(sourcePath);
            var result = reader.ReadToEnd();

            var byteArray = Encoding.UTF8.GetBytes(result);

            using var memoryStreamReader = new MemoryStream();

            int i = 0;
            while (i < byteArray.Length)
            {
                memoryStreamReader.WriteByte(byteArray[i++]);
            }

            var newByteArray = new byte[memoryStreamReader.Length];
            memoryStreamReader.Seek(0, SeekOrigin.Begin);
            i = 0;
            while (i < memoryStreamReader.Length)
            {
                newByteArray[i++] = Convert.ToByte(memoryStreamReader.ReadByte());
            }

            var charArray = Encoding.UTF8.GetChars(newByteArray);

            using var writer = new StreamWriter(destinationPath, false, Encoding.UTF8);
            writer.Write(charArray);
            return (int)writer.BaseStream.Length;
        }

        #endregion

        #region TODO: Implement by block copy logic using FileStream buffer.
        
        public static int ByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            using var reader = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using var writer = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

            var buffer = new byte[1024];
            int chunkSize = 1;
            while (chunkSize > 0)
            {
                chunkSize = reader.Read(buffer, 0, buffer.Length);
                writer.Write(buffer, 0, chunkSize);
            }
            
            return (int)writer.Length;
        }

        #endregion

        #region TODO: Implement by block copy logic using MemoryStream.
        
        public static int InMemoryByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            // TODO: Use InMemoryByByteCopy method's approach

            // TODO: step 1. Use StreamReader to read entire file in string

            // TODO: step 2. Create byte array on base string content - use  System.Text.Encoding class

            // TODO: step 4. Use MemoryStream instance (from step 3) to write it content in new byte array

            // TODO: step 5. Use Encoding class instance (from step 2) to create char array on byte array content

            // TODO: step 6. Use StreamWriter here to write char array content in new file

            using var reader = new StreamReader(sourcePath);
            var result = reader.ReadToEnd();

            var byteArray = Encoding.UTF8.GetBytes(result);

            using var memoryStreamReader = new MemoryStream(byteArray);
            var newByteArray = memoryStreamReader.ToArray();

            var charArray = Encoding.UTF8.GetChars(newByteArray);

            using var writer = new StreamWriter(destinationPath, false, Encoding.UTF8);
            writer.Write(charArray);

            return (int)writer.BaseStream.Length;
        }

        #endregion

        #region TODO: Implement by block copy logic using class-decorator BufferedStream.

        public static int BufferedCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            int bufferSize = 1024;

            using var readerStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using var readerBuffer = new BufferedStream(readerStream, bufferSize);
            using var writerStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
            using var writerBuffer = new BufferedStream(writerStream, bufferSize);

            int chunkSize = 1;
            byte[] buffer;
            while (chunkSize > 0)
            {
                buffer = new byte[bufferSize];
                chunkSize = readerBuffer.Read(buffer, 0, bufferSize);
                writerBuffer.Write(buffer, 0, chunkSize);
            }

            return (int)writerStream.Length;
        }

        #endregion

        #region TODO: Implement by line copy logic using FileStream and classes text-adapters StreamReader/StreamWriter

        public static int ByLineCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);

            using var reader = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using var writer = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);

            using var streamReader = new StreamReader(reader);
            using var streamWriter = new StreamWriter(writer, Encoding.UTF8);
            string? line;
            int linesCount = 0;

            while (!streamReader.EndOfStream)
            {
                line = streamReader.ReadLine();
                if (streamReader.EndOfStream)
                {
                    streamWriter.Write(line);
                }
                else
                {
                    streamWriter.WriteLine(line);
                }

                linesCount++;
            }

            return linesCount;
        }

        #endregion

        #endregion

        #region Private members

        #region TODO: Implement validation logic

        private static void InputValidation(string sourcePath, string destinationPath)
        {
            if (sourcePath is null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (destinationPath == null)
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException(
                    $"File '{sourcePath}' not found. Parameter name: {nameof(sourcePath)}.");
            }

//            if (!File.Exists(destinationPath))
//            {
//                try
//                {
//                    File.Create(destinationPath);
//                }
//                catch
//                {
//                    throw new FileNotFoundException(
//                        $"File '{destinationPath}' not found and can not be created. Parameter name: {nameof(destinationPath)}.");
//                }
//            }

//            if (new FileInfo(destinationPath).IsReadOnly)
//            {
//                throw new FieldAccessException(
//                    $"File '{destinationPath}' is readonly. Parameter name: {nameof(destinationPath)}.");
//            }
        }

        #endregion

        #endregion
    }
}