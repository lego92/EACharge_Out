using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace EACharge_Out
{
    class UintToParityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = System.Convert.ToUInt16(value);
            switch (number)
            {
                case 0: return "8N1";
                case 1: return "8O1";
                case 2: return "8E1";
                case 3: return "8N2";
                default: return "Неизвестно";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parity = value.ToString();
            switch (parity)
            {
                case "8N1": return 0;
                case "8O1": return 1;
                case "8E1": return 2;
                case "8N2": return 3;
                default: return "Неизвестно";
            }
        }
    }
}
