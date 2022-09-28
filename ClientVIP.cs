using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bank
{
    internal class ClientVIP : Client
    {
        public ClientVIP( string Name, string Phone) : base( Name, Phone)
        {
            this.Name = $"VIP холоп_{Name}";
            this.Status = "Чел типа VIP";
            Type = Type.ClientVIP;
            Debug.WriteLine(Type);
        }
        public ClientVIP()
        {
           
            this.Status = "Чел типа VIP";
            Type = Type.ClientVIP;
            
        }
    }

}
