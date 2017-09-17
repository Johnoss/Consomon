using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public static class Serialization
    {


        public static void WriteToBinaryFile<T>(string path, string file, T objectToWrite)
        {
            string filepath = $"{path}\\{file}";

            using (Stream stream = File.OpenWrite(filepath))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string path, string file)
        {
            string filePath = $"{path}\\{file}";
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public static bool CheckSaveFile(string path, string file)
        {
            return (new FileInfo($"{path}\\{file}").Length > 0);
        }
    }
}
