using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EACharge_Out
{
    class RegisterUshort : RegisterBase, IValue<ushort>
    {
        private ushort _value;
        public ushort Value
        {
            get => _value;
            set
            {
                SetField(ref _value, value, "Value");
            }
        }

        public RegisterUshort(ushort address, string name, ushort length) : base(address, length, name)
        {
            Value = 0;
        }        
    }
}
