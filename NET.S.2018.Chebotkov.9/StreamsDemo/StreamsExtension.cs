using System.IO;
using System.Text;

namespace StreamsDemo
{
    /// <summary>
    /// Class contains methods for working with files and streams of bytes.
    /// </summary>
    public static class StreamsExtension
    {

        #region Public members

        #region TODO: Implement by byte copy logic using class FileStream as a backing store stream .

        /// <summary>
        /// Copies data by bytes from one file to another using FileStream.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int ByByteCopy(string sourcePath, string destinationPath)
        {
            int countOfbytes = 0;
            InputValidation(sourcePath, destinationPath);
            using (FileStream fileStream = new FileStream(sourcePath, FileMode.Open))
            {
                using (FileStream fS = new FileStream(destinationPath, FileMode.OpenOrCreate))
                {
                    /*fileStream.Position = 0;
                    while (fileStream.Position < fileStream.Length)
                    {
                        fS.WriteByte((byte)fileStream.ReadByte());
                        countOfbytes++;
                        fileStream.Position++;
                    }*/

                    // ↑this and this↓ are the same things...

                    int b;
                    while ((b = fileStream.ReadByte()) != -1)
                    {
                        countOfbytes++;
                        fS.WriteByte((byte)b);
                    }
                }
            }

            return countOfbytes;
        }

        #endregion

        #region TODO: Implement by byte copy logic using class MemoryStream as a backing store stream.

        /// <summary>
        /// Copies data from one file to another with help of MemoryStream by bytes.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int InMemoryByByteCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);
            using (StreamReader reader = new StreamReader(sourcePath, Encoding.Default))
            {
                string str = reader.ReadToEnd();
                byte[] array = Encoding.Default.GetBytes(str);
                using (MemoryStream memoryStream = new MemoryStream(array))
                {
                    byte[] newArray = new byte[memoryStream.Length];
                    memoryStream.Read(newArray, 0, newArray.Length);
                    using (StreamWriter fS = new StreamWriter(destinationPath, true, Encoding.Default))
                    {
                        fS.Write(Encoding.Default.GetChars(newArray));
                        return newArray.Length;
                    }
                }
            }
        }

        #endregion

        #region TODO: Implement by block copy logic using FileStream buffer.

        /// <summary>
        /// Copies data from one file to another with help of FileStream.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int ByBlockCopy(string sourcePath, string destinationPath)
        {
            byte[] array;
            int offset = 0;
            int length = 1024;
            //I should think about it. Something wrong with offset.
            InputValidation(sourcePath, destinationPath);

            using (FileStream fileStream = new FileStream(sourcePath, FileMode.Open))
            {
                while (true)
                {
                    if (offset > fileStream.Length)
                    {
                        break;
                    }
                    else if (fileStream.Length - offset > -1 && offset + length > fileStream.Length)
                    {
                        length = (int)(fileStream.Length - offset);
                    }
                    
                    array = new byte[length];
                    fileStream.Read(array, offset, array.Length);

                    using (FileStream fS = new FileStream(destinationPath, FileMode.OpenOrCreate))
                    {
                        fS.Write(array, offset, array.Length);
                    }
                   
                    offset += length;
                }
            }

            return offset;
        }

        #endregion

        #region TODO: Implement by block copy logic using MemoryStream.

        /// <summary>
        /// Copies data from one file to another with help of MemoryStream.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>Amount of copied bytes.</returns>
        public static int InMemoryByBlockCopy(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);
            using (Stream stream = new FileStream(sourcePath, FileMode.Open))
            {
                byte[] array = new byte[stream.Length];
                stream.Read(array, 0, (int)stream.Length);
                using (MemoryStream memoryStream = new MemoryStream(array))
                {
                    using (FileStream fs = new FileStream(destinationPath, FileMode.OpenOrCreate))
                    {
                        fs.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                        return (int)fs.Length;
                    }
                }
            }
        }

        #endregion

        #region TODO: Implement by block copy logic using class-decorator BufferedStream.

        /// <summary>
        /// Copies data from one file to another with help of BufferedStream.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int BufferedCopy(string sourcePath, string destinationPath)
        {
            byte[] array;
            InputValidation(sourcePath, destinationPath);
            using (Stream stream = new FileStream(sourcePath, FileMode.Open))
            {
                using (BufferedStream fileStream = new BufferedStream(stream, (int)stream.Length))
                {
                    array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                }
            }

            using (Stream stream = new FileStream(destinationPath, FileMode.OpenOrCreate))
            {
                using (BufferedStream fS = new BufferedStream(stream))
                {
                    fS.Write(array, 0, array.Length);
                }
            }

            return array.Length;
        }

        #endregion

        #region TODO: Implement by line copy logic using FileStream and classes text-adapters StreamReader/StreamWriter

        /// <summary>
        /// Copies data from one file to another by lines.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>Amount of written strings.</returns>
        public static int ByLineCopy(string sourcePath, string destinationPath)
        {
            int amountOfStrings = 0;
            InputValidation(sourcePath, destinationPath);
            using (StreamReader reader = new StreamReader(sourcePath, Encoding.Default))
            {
                using (StreamWriter writer = new StreamWriter(destinationPath, false, Encoding.Default))
                {
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        writer.WriteLine(str);
                        amountOfStrings++;
                    }
                }
            }

            return amountOfStrings;
        }

        #endregion

        #region TODO: Implement content comparison logic of two files 

        /// <summary>
        /// Checks if first file identity to another one.
        /// </summary>
        /// <param name="sourcePath">Path to the first file.</param>
        /// <param name="destinationPath">Path to the second file.</param>
        /// <returns>True if they are exist the same set of bytes. False if they aren't.</returns>
        public static bool IsContentEquals(string sourcePath, string destinationPath)
        {
            InputValidation(sourcePath, destinationPath);
            using (FileStream fs = new FileStream(sourcePath, FileMode.Open))
            {
                byte[] firstArray = new byte[fs.Length];
                fs.Read(firstArray, 0, firstArray.Length);

                using (FileStream fs2 = new FileStream(destinationPath, FileMode.Open))
                {
                    fs.Position = 0;
                    fs2.Position = 0;
                    byte[] secondArray = new byte[fs.Length];
                    fs.Read(secondArray, 0, secondArray.Length);

                    if (firstArray.Length != secondArray.Length)
                    {
                        return false;
                    }
                    
                    while (fs.Position < fs.Length || fs2.Position < fs2.Length)
                    {
                        if (fs.ReadByte() != fs2.ReadByte())
                        {
                            return false;
                        }
                    }

                    /*for (int i = 0; i < firstArray.Length; i++)
                    {
                        if (firstArray[i] != secondArray[i])
                        {
                            return false;
                        }
                    }*/
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region Private members

        #region TODO: Implement validation logic

        private static void InputValidation(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath) || !File.Exists(destinationPath))
            {
                throw new FileNotFoundException("File doesn't exist.");
            }
        }

        #endregion

        #endregion

    }
}
