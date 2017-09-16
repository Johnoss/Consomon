using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public static class GenericTypeOperations<T>
    {
        public static T[] ShuffleArray(T[] array)
        {
            Random rnd = new Random();
            T[] result = array.OrderBy(x => rnd.Next()).ToArray();
            return result;
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }
    }

    public static class GenericOperations
    {
        static Random rnd = new Random();
        public static bool CalculateTypeAdvantage(MobType caster, MobType target)
        {
            int totalTypes = Enum.GetNames(typeof(MobType)).Length;

            int strongIndex = (int)caster + 1;
            if (strongIndex == totalTypes)
                strongIndex = 0; //make a cycle

            //Console.WriteLine("DEBUG comparing {0} with {1}, looking for strong index {2}", caster, target, (MobType)strongIndex);
            return (MobType)strongIndex == target;
        }

        public static float FetchRandomPercentage(int upperLimit = 1, int lowerLimit = 0)
        {
            return GetRandom().Next(lowerLimit * 100, upperLimit * 100) / 100f;
        }

        public static Random GetRandom()
        {
            return rnd;
        }


        /// <summary>
        /// Get ratio of value between min and max. Result is between 0 and 1
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static double GetRatio(double min, double max, double value)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// Get proportion (result is between between min and max) by given ratio. Ratio must be between 0 and 1
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <param name="ratio">Ratio</param>
        /// <returns></returns>
        public static double GetProportion(double min, double max, double ratio)
        {
            return ratio * (max - min) + min;
        }

        /// <summary>
        /// Get proportion of value between min and max. Result is between min and max
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        /// 

        public static int RoundToClosestInt(double value, int roundTo = Settings.RoundRewards)
        {
            return (int)Math.Round(value / roundTo) * roundTo;
        }
    }


    public static class Extensions
    {
        public static T Clone<T>(this object item)
        {
            if (item != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();

                formatter.Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);

                T result = (T)formatter.Deserialize(stream);

                stream.Close();

                return result;
            }
            else
                return default(T);
        }
    }
}
