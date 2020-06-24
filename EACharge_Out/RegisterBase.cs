using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace EACharge_Out
{
    /// <summary>
    /// Базовый класс для любого типа регистра
    /// </summary>
    public abstract class RegisterBase : INotifyPropertyChanged 
    {
        /// <summary>
        /// Номер/адрес регистра
        /// </summary>
        public ushort Address { get; private set; }

        /// <summary>
        /// Размер регистра - количество 16 битных "блоков"
        /// </summary>
        public ushort Length { get; private set; }

        /// <summary>
        /// Название регистра
        /// </summary>
        public string Name { get; private set; }


        public RegisterBase(ushort address, ushort length)
        {
            Address = address;            
            Length = length;
            Name = "Unknown";
        }

        public RegisterBase(ushort address, ushort length, string name) : this(address, length)
        {           
            Name = name;
        }

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

    }
}
