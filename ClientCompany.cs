using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bank
{
    internal class ClientCompany : Client
    {
        public ClientCompany( string Name, string Phone) : base( Name, Phone)
        {
            this.Name = $"Очередная шаражкина контора_{Name}";
            this.Status = "Шарага";
            Type = Type.ClientCompany;
            Debug.WriteLine(Type);
        }
        public ClientCompany()
        {
            this.Status = "Шарага";
            Type = Type.ClientCompany;
            

        }
        

    }
}
