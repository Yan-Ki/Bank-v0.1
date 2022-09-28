using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.ComponentModel;

namespace Bank
{
    internal class BankRepository
    {
        static Random random;
        static List<int> IdDb;// Коллекция для хранения ID



        /// <summary>
        /// Статический конструктор
        /// </summary>
        static BankRepository()
        {
            BankRepository.random = new Random();
            BankRepository.IdDb = new List<int>();
            bankAccountsBlock = new ObservableCollection<BankAccount<Client>>();

            BankAccount<Client>.log += e => Logs.Add(e); //Подписка на событие для логов
            BankAccount<Client>.del += TransIn;//Подписка на событие для переводов
            BankAccount<Client>.d += e => bankAccountsBlock.Add(e);//Подписка на событие для блокировки
        }

        public ObservableCollection<BankAccount<Client>> bankAccounts { get; set; }  //Коллекция для хранения счетов клиентов

        public static ObservableCollection<string> Logs { get; set; } //Коллекция для хранения логов

        public static ObservableCollection<BankAccount<Client>> bankAccountsBlock { get; set; } //Коллекция для хранения заблокированных счетов

        /// <summary>
        /// Конструктор
        /// </summary>
        public BankRepository()
        {
            bankAccounts = new ObservableCollection<BankAccount<Client>>();
            Logs = new ObservableCollection<string>();
            Filling();
        }
        /// <summary>
        /// Метод наполнения банка клиентами
        /// </summary>
        private void Filling()
        {
            for (int i = 1; i <= 10; i++)
            {
                int Id = random.Next(1000, 9999);
                while (IdDb.Contains(Id))
                {
                    Id = random.Next(1000, 9999);
                }
                switch (random.Next(1, 4))
                {

                    case 1:
                        bankAccounts.Add(new BankAccount<Client>(Id, random.Next(1, 9) * 1000, new Client(Id.ToString(), $"+7_927_{random.Next(100, 999)}_{random.Next(1000, 9999)}")));
                        break;

                    case 2:
                        bankAccounts.Add(new BankAccount<Client>(Id, random.Next(1, 9) * 1000, new ClientVIP(Id.ToString(), $"+7_927_{random.Next(100, 999)}_{random.Next(1000, 9999)}")));
                        break;
                    case 3:
                        bankAccounts.Add(new BankAccount<Client>(Id, random.Next(1, 9) * 1000, new ClientCompany(Id.ToString(), $"8_{random.Next(10, 99) * 10}_{random.Next(100000, 999999)}")));
                        break;

                }
            }
        }
        /// <summary>
        /// Метод добавление нового сотрудника юзером
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        
        public void FillingClient<T>(string Name, string Phone, int AccValue)
            where T : Client, new()
        {
            
            int Id = random.Next(1000, 9999);
            while (IdDb.Contains(Id))
            {
                Id = random.Next(1000, 9999);
            }
            T client = new T
            {
                Name = Name,
                Phone = Phone
            };
            bankAccounts.Add(new BankAccount<Client>(Id, AccValue, client));

        }

        /// <summary>
        /// Метод списания со счёта
        /// </summary>
        /// <param name="money"></param>
        /// <param name="client1"></param>
        /// <param name="client2"></param>
        public void TransOut(int money, BankAccount<Client> client1, BankAccount<Client> client2)
        {
            if (bankAccountsBlock.Contains(client1) || bankAccountsBlock.Contains(client2))
            {
                MessageBox.Show("Счёт получателя заблокирован за подозрительные переводы");

            }
            else
            {
                client1.TransferOut(money, client2);
            }

        }
        /// <summary>
        /// Метод зачисления на счёт
        /// </summary>
        /// <param name="money"></param>
        /// <param name="client2"></param>
        public static void TransIn(int money, BankAccount<Client> client2)
        {
            client2.TransferIn(money);
        }
       
        
    }
}
