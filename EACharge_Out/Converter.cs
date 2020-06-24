using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EACharge_Out
{
    /// <summary>
    /// Вспомогательный класс-конвертер
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Преобразует массив ushort в массив byte
        /// </summary>
        /// <param name="uArray">исходный массив ushort</param>
        /// <returns>массив byte</returns>
        public static byte[] ConvertUshortArrayToByteArray(ushort[] uArray)
        {
            byte[] bArray = new byte[uArray.Length * 2];

            int j = 0;

            for (int i = 0; i < uArray.Length; i++)
            {
                byte[] oneUshort = BitConverter.GetBytes(uArray[i]);
                bArray[j] = oneUshort[1];
                bArray[j + 1] = oneUshort[0];
                j += 2;
            }

            return bArray;
        }
        /// <summary>
        /// Преобразует массив byte в массив ushort
        /// </summary>
        /// <param name="bArray">исходный массив byte</param>
        /// <returns>массив ushort</returns>
        public static ushort[] ConvertByteArrayToUshortArray(byte[] bArray)
        {
            ushort[] uArray = new ushort[bArray.Length / 2];

            int j = 0;

            for (int i = 0; i <= bArray.Length - 2; i += 2)
            {
                byte[] array = { bArray[i + 1], bArray[i] };
                uArray[j] = BitConverter.ToUInt16(array, 0);
                j++;
            }

            return uArray;
        }


        /// <summary>
        /// Преобразует массив из двух ushort в float значение
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float ConvertTwoUInt16ToFloat(ushort[] values)
        {
            byte[] array1 = BitConverter.GetBytes(values[1]);
            byte[] array2 = BitConverter.GetBytes(values[0]);
            byte[] array12 = array1.Concat(array2).ToArray(); // LINQ
            return BitConverter.ToSingle(array12, 0);
        }

        /// <summary>
        /// Преобразует float значение в массив из двух ushort
        /// </summary>
        /// <param name="value">исходное float значение</param>
        /// <returns>массив из двух ushort</returns>
        public static ushort[] ConvertFloatToTwoUint16(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort upper = BitConverter.ToUInt16(bytes, 0);
            ushort lower = BitConverter.ToUInt16(bytes, 2);

            ushort[] registers = new ushort[] { upper, lower };

            return registers;
        }

        /// <summary>
        /// Преобразует значение перечисления в baudrate
        /// </summary>
        /// <param name="number">входное число</param>
        /// <returns>baudrate</returns>
        public static int ToBaudRate(ushort number)
        {            
            switch (number)
            {
                case 1: return 1200;
                case 2: return 2400;
                case 3: return 4800;
                case 4: return 9600;
                case 5: return 19200;
                case 6: return 38400;
                case 7: return 57600;
                case 8: return 115200;
                default: return 115200;
            }

        }

        
    }
}
