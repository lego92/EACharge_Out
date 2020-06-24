using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EACharge_Out
{
    public class UintToBaudRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = System.Convert.ToUInt16(value);
            switch (number)
            {
                case 1: return "1.2k";
                case 2: return "2.4k";
                case 3: return "4.8k";
                case 4: return "9.6k";
                case 5: return "19.2k";
                case 6: return "38.4k";
                case 7: return "57.6k";
                case 8: return "115.2k";                                
                default: return "Неизвестно";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baudRate = value.ToString();
            switch (baudRate)
            {
                case "1.2k": return 1 ;
                case "2.4k": return 2 ;
                case "4.8k": return 3;
                case "9.6k": return 4;
                case "19.2k": return 5;
                case "38.4k": return 6;
                case "57.6k": return 7;
                case "115.2k": return 8;
                default: return "Неизвестно";
            }
        }
    }
}
