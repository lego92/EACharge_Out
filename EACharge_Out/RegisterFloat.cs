using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EACharge_Out
{
    class RegisterFloat : RegisterBase, IValue<float>
    {
        private float _value;
        public float Value
        {
            get => _value;
            set
            {
                SetField(ref _value, value, "Value");
            }
        }

        public RegisterFloat(ushort address,  ushort length, string name) : base(address, length, name)
        {
            Value = 0;
        }       
    }
}
