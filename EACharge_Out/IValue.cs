using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EACharge_Out
{
    interface IValue<T>
    {
        T Value { get; set; }
    }
}
