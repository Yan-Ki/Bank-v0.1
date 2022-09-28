using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bank
{
    internal class Client
    {
        public Type Type { get; set; }
       
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }

        public Client ( string Name, string phone)
        {
            
            this.Name = $"Обычный холоп_{Name}";
            this.Phone = phone;
            this.Status = "Обычный чел";
            Type= Type.Client;
            Debug.WriteLine(Type);

        }
        public Client()
        {
            this.Status = "Обычный чел";
        }
       

    }
}
