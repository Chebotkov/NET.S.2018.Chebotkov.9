using System.IO;
using System.Text;

namespace FileStreamMethods
{
    /// <summary>
    /// Class contains methods for working with files and streams of bytes.
    /// </summary>
    public class FSMLib
    {
        #region Public methods
        /// <summary>
        /// Copies data from one file to another with help of FileStream.
        /// </summary>
        /// <param name="fromFilePath">Path to the first file.</param>
        /// <param name="toFilePath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int CopyFromToWithFileStream(string fromFilePath, string toFilePath)
        {
            byte[] array;
            try
            {
                using (FileStream fileStream = new FileStream(fromFilePath, FileMode.Open))
                {
                    array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                }
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Wrong path.");
            }

            using (FileStream fS = new FileStream(toFilePath, FileMode.OpenOrCreate))
            {
                fS.Write(array, 0, array.Length);
            }

            return array.Length;
        }

        /// <summary>
        /// Copies data from one file to another with help of BufferedStream.
        /// </summary>
        /// <param name="fromFilePath">Path to the first file.</param>
        /// <param name="toFilePath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int CopyFromToWithBufferedStream(string fromFilePath, string toFilePath)
        {
            byte[] array;
            try
            {
                using (Stream stream = new FileStream(fromFilePath, FileMode.Open))
                {
                    using (BufferedStream fileStream = new BufferedStream(stream, (int)stream.Length))
                    {
                        array = new byte[fileStream.Length];
                        fileStream.Read(array, 0, array.Length);
                    }
                }
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Wrong path.");
            }

            using (Stream stream = new FileStream(toFilePath, FileMode.OpenOrCreate))
            {
                using (BufferedStream fS = new BufferedStream(stream))
                {
                    fS.Write(array, 0, array.Length);
                }
            }

            return array.Length;
        }

        /// <summary>
        /// Checks if first file identity to another one.
        /// </summary>
        /// <param name="firstPath">Path to the first file.</param>
        /// <param name="secondPath">Path to the second file.</param>
        /// <returns>True if they are exist the same set of bytes. False if they aren't.</returns>
        public static bool CompareTwoFiles(string firstPath, string secondPath)
        {
            using (FileStream fs = new FileStream(firstPath, FileMode.Open))
            {
                byte[] firstArray = new byte[fs.Length];
                fs.Read(firstArray, 0, firstArray.Length);

                using (FileStream fs2 = new FileStream(secondPath, FileMode.Open))
                {
                    if (fs2.Length != fs.Length)
                    {
                        return false;
                    }

                    byte[] secondArray = new byte[fs.Length];
                    fs.Read(secondArray, 0, secondArray.Length);

                    for (int i = 0; i < fs.Length; i++)
                    {
                        if (firstArray[i] != secondArray[i])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Copies data from one file to another by lines.
        /// </summary>
        /// <param name="firstPath">Path to the first file.</param>
        /// <param name="secondPath">Path to the second file.</param>
        /// <returns>Amount of written strings.</returns>
        public static int CopyByString(string firstPath, string secondPath)
        {
            int amountOfStrings = 0;
            using (StreamReader reader = new StreamReader(firstPath, Encoding.Default))
            {
                using (StreamWriter writer = new StreamWriter(secondPath, false, Encoding.Default))
                {
                    StringBuilder str;
                    while ((str = new StringBuilder(reader.ReadLine())) != null)
                    {
                        writer.WriteLine(str);
                        amountOfStrings++;
                    }
                }
            }

            return amountOfStrings;
        }

        /// <summary>
        /// Copies data from one file to another with help of MemoryStream.
        /// </summary>
        /// <param name="fromFilePath">Path to the first file.</param>
        /// <param name="toFilePath">Path to the second file.</param>
        /// <returns>Amount of copied bytes.</returns>
        public static int CopyFromToWithMemoryStream(string fromFilePath, string toFilePath)
        {
            try
            {
                using (Stream stream = new FileStream(fromFilePath, FileMode.Open))
                {
                    byte[] array = new byte[stream.Length];
                    stream.Read(array, 0, (int)stream.Length);
                    using (MemoryStream memoryStream = new MemoryStream(array))
                    {
                        using (FileStream fs = new FileStream(toFilePath, FileMode.OpenOrCreate))
                        {
                            fs.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                            return (int)fs.Length;
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Wrong path.");
            }
        }

        /// <summary>
        /// Copies data from one file to another with help of FileStream by bytes.
        /// </summary>
        /// <param name="fromFilePath">Path to the first file.</param>
        /// <param name="toFilePath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int CopyFromToWithFileStreamByBytes(string fromFilePath, string toFilePath)
        {
            int countOfbytes = 0;
            try
            {
                using (FileStream fileStream = new FileStream(fromFilePath, FileMode.Open))
                {
                    using (FileStream fS = new FileStream(toFilePath, FileMode.OpenOrCreate))
                    {
                        fileStream.Position = 0;
                        while (fileStream.Position < fileStream.Length)
                        {
                            fS.WriteByte((byte)fileStream.ReadByte());
                            countOfbytes++;
                            fileStream.Position++;
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Wrong path.");
            }

            return countOfbytes;
        }

        /// <summary>
        /// Copies data from one file to another with help of MemoryStream by bytes.
        /// </summary>
        /// <param name="fromFilePath">Path to the first file.</param>
        /// <param name="toFilePath">Path to the second file.</param>
        /// <returns>Number of copied bytes.</returns>
        public static int CopyFromToWithMemoryStreamByBytes(string fromFilePath, string toFilePath)
        {
            int countOfbytes = 0;
            try
            {
                using (FileStream fileStream = new FileStream(fromFilePath, FileMode.Open))
                {
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, (int)fileStream.Length);
                    using (MemoryStream memoryStream = new MemoryStream(array))
                    {
                        memoryStream.Position = 0;
                        while (memoryStream.Position < memoryStream.Length)
                        {
                            using (FileStream fS = new FileStream(toFilePath, FileMode.OpenOrCreate))
                            {
                                fS.WriteByte((byte)memoryStream.ReadByte());
                            }

                            countOfbytes++;
                            memoryStream.Position++;
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Wrong path.");
            }

            return countOfbytes;
        }
        #endregion
    }
}