using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileStreamMethods
{
    /// <summary>
    /// Class contains methods for working with files and streams of bytes.
    /// </summary>
    public class FSMLib
    {
        /// <summary>
        /// Copies data from one file to another with help of FileStream.
        /// </summary>
        /// <param name="fromFilePath">Path to the first file.</param>
        /// <param name="toFilePath">Path to the second file.</param>
        /// <returns>Number of copied files.</returns>
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
        /// <returns>Number of copied files.</returns>
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
                    while ( (str = new StringBuilder(reader.ReadLine())) != null)
                    {
                        writer.WriteLine(str);
                        amountOfStrings++;
                    }
                }
            }

            return amountOfStrings;
        }


    }
}
