using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace EACharge_Out
{
    public class EAChargeMonitor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public ObservableCollection<RegisterBase> Registers { get; set; }        

        public int NumberOfMeasuringRegisters { get; set; } = 7;
        public int NumberOfSettingRegisters { get; set; } = 9;

        public EAChargeMonitor()
        {
            Registers = new ObservableCollection<RegisterBase>()
            {
                new RegisterFloat(1000, 4, "Resistance"),
                //new RegisterFloat(1000, "Reserved", 4),
                new RegisterFloat(1008, 4, "Voltage"),
                new RegisterFloat(1012, 4,"Capacitance"),
                new RegisterFloat(1016, 4,"Voltage plus"),
                new RegisterFloat(1020, 4, "Voltage minus"),
                new RegisterFloat(1036, 4, "Resistance plus"),
                new RegisterFloat(1040, 4, "Resistance minus"),
                //
                new RegisterUshort(3005, "PreAlarm", 1),
                new RegisterUshort(3007, "Alarm", 1),
                new RegisterUshort(3015, "Device Address", 1),
                new RegisterUshort(3016, "BaudRate", 1),
                new RegisterUshort(3017, "Parity", 1),
                new RegisterUshort(3018, "Delay", 1),
                //new RegisterUshort(8003, "Factory Reset", 1),
                new RegisterString(9800, "Device Name", 10),
                new RegisterUshort(9820, "ID", 1),
                new RegisterUshort(9821, "Firmware Version", 1),
            };
        }


        public void ParseResponse(RegisterBase register, ushort[] data)
        {
            switch (register)
            {
                case IValue<float> r:
                    float floatToWrite;
                    switch (register.Name)
                    {
                        case "Resistance":
                        case "Resistance plus":
                        case "Resistance minus":
                            float myFloat = Converter.ConvertTwoUInt16ToFloat(data) / 1000; // перевод в кОм из Ом
                            floatToWrite = myFloat;
                            break;
                        case "Capacitance":
                            myFloat = Converter.ConvertTwoUInt16ToFloat(data) * 1000000; // перевод в мкФ из Ф
                            floatToWrite = myFloat;
                            break;
                        default:
                            myFloat = Converter.ConvertTwoUInt16ToFloat(data);
                            floatToWrite = myFloat;
                            break;
                    }
                    r.Value = floatToWrite;
                    break;
                case IValue<string> r:
                    byte[] byteString = Converter.ConvertUshortArrayToByteArray(data);
                    r.Value = Encoding.Default.GetString(byteString);
                    break;
                case IValue<ushort> r:
                    r.Value = data[0];
                    break;
            }
        }
    }
}

