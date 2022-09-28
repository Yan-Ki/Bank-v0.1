using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace Bank
{

    internal class BankAccount<T> : IEquatable<BankAccount<T>>, INotifyPropertyChanged
        where T : Client
    {
        
        public delegate void D(BankAccount<Client> client);
        public static event D d;//Событие для блокировки счетов


        public delegate void Del(int money, BankAccount<Client> client);
        public static event Del del;//Событие для переводов

        public delegate void Deleg(string log);
        public static event Deleg log;//Событие для логов

        public event PropertyChangedEventHandler PropertyChanged;

        public T ClientAcc { get; set; }
        public int Id { get; private set; }
        public int accValue;
        public int AccValue
        {
            get { return this.accValue; }

            private set
            {
                this.accValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AccValue)));
            }
        }
        public double deposit;
        public double Deposit
        {
            get { return this.deposit; }

            private set
            {
                this.deposit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Deposit)));
            }
        }
        public int count { get; private set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        /// <param name="AccValue"></param>
        /// <param name="ClientAcc"></param>
        public BankAccount(int id, int AccValue, T ClientAcc)
        {
            this.Id = id;
            this.AccValue = AccValue;
            this.Deposit = 0;
            this.ClientAcc = ClientAcc;
            this.count = 0;
            log?.Invoke($"Создание счёта: ID: {Id} Статус:{ClientAcc.Type} Счёт:{AccValue} ");

        }
        /// <summary>
        /// Списание со счёта
        /// </summary>
        /// <param name="money"></param>
        /// <param name="client2"></param>
        public void TransferOut(int money, BankAccount<Client> client2)
        {
            if (count < 3)
            {
                if (client2.ClientAcc.GetType() == typeof(ClientCompany) && this.ClientAcc.GetType() != typeof(ClientCompany))
                {
                    count++;
                }
                this.AccValue = AccValue - money;
                del?.Invoke(money, client2);
                log?.Invoke($"Исходящий перевод: ID: {Id} Статус:{ClientAcc.Type} Передано:{money} ");
            }
            else
            {
                d?.Invoke(client2);
                count = 0;
                log?.Invoke($"Блокировка входящего перевода: ID: {Id} Статус:{ClientAcc.Type}  ");
            }


        }
        /// <summary>
        /// Зачисление на счёт
        /// </summary>
        /// <param name="money"></param>
        public void TransferIn(int money)
        {
            this.AccValue = AccValue + money;
            log?.Invoke($"Входящий перевод: ID: {Id} Статус:{ClientAcc.Type} Получено:{money} ");
        }

        /// <summary>
        /// Метод открытия вклада
        /// </summary>
        /// <param name="money"></param> 
        public void AddValue(int money)
        {
            if (money > AccValue || money <= 0)
            {
                MessageBox.Show("Введите сумму вклада в пределах счёта клиента!");
            }
            else
            {
                double percent;

                switch (ClientAcc.Status)
                {
                    case "Чел типа VIP": percent = 0.1; break;
                    case "Шарага": percent = 0.05; break;
                    default: percent = 0.12; break;

                }

                this.Deposit = (money * percent) + money;
                this.AccValue -= money;
                MessageBox.Show($"Вклад открыт под {percent * 100}% годовых");
                log?.Invoke($"Открытие вклада: ID: {Id} Статус:{ClientAcc.Type} Вклад:{Deposit} ");
            }

        }

        public bool Equals(BankAccount<T> other)
        {
            return this.Id == other.Id;
        }
    }
}

