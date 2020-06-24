using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EACharge_Out
{
    class RegisterString : RegisterBase, IValue<string>
    {
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                SetField(ref _value, value, "Value");
            }
        }

        public RegisterString(ushort address, string name, ushort length) : base(address, length, name)
        {
            Value = String.Empty;
        }        
    }
}
