using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using BankAcc;
using Clients;
using Functions;



namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BankRepository Sber;
        public MainWindow()
        {
            InitializeComponent();
            Sber = new BankRepository();
            ListViewClients.ItemsSource = Sber.bankAccounts;
            ComboBoxClients.ItemsSource = Sber.bankAccounts;

            ListBoxLog.ItemsSource = BankRepository.Logs;
            ListBoxBlock.ItemsSource = BankRepository.bankAccountsBlock;
           
        }
        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TexBoxAccValue.Text) || string.IsNullOrEmpty(TextBoxName.Text) || string.IsNullOrEmpty(TextBoxPhone.Text) ||
                    string.IsNullOrEmpty(ComboBoxStatus.Text)) throw new MyException();

                if (TextBoxName.Text.IsNumberContains()) throw new MyException(TextBoxName.Text, 1);
                if (!TextBoxPhone.Text.IsNumberContains()) throw new MyException(TextBoxPhone.Text, 2);

                int AccValue = Convert.ToInt32(TexBoxAccValue.Text);

                if (AccValue < 0) throw new FormatException();

                switch (ComboBoxStatus.Text)
                {
                    case "Физическое лицо": Sber.FillingClient<Client>(TextBoxName.Text, TextBoxPhone.Text, AccValue); break;
                    case "Физическое лицо(VIP)": Sber.FillingClient<ClientVIP>(TextBoxName.Text, TextBoxPhone.Text, AccValue); break;
                    case "Юридическое лицо": Sber.FillingClient<ClientCompany>(TextBoxName.Text, TextBoxPhone.Text, AccValue); break;
                }

            }
           
            catch (MyException error) when (error.Code == 2)
            {
                MessageBox.Show($"Ошибка в телефоне клиента ");
                Debug.WriteLine(error);
            }
            catch (MyException error) when (error.Code == 1)
            {
                MessageBox.Show($"Ошибка в имени клиента {error.Message}");
                Debug.WriteLine(error);
            }
            catch (FormatException error)
            {
                MessageBox.Show($"Ошибка ввода счёта клиента");
                Debug.WriteLine(error);
            }
            catch (MyException error)
            {
                MessageBox.Show($"Заполните все строки");
                Debug.WriteLine(error);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ошибка ввода");
                Debug.WriteLine(error);
            }



        }
        /// <summary>
        /// Выбор клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewClients_Chang(object sender, SelectionChangedEventArgs e)
        {
            TexBoxMoney.Text = Convert.ToString((ListViewClients.SelectedItem as BankAccount<Client>).AccValue);

        }
        /// <summary>
        /// Вклад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenDeposit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BankAccount<Client> client = ListViewClients.SelectedItem as BankAccount<Client>;
                if ((string.IsNullOrEmpty(TexBoxMoney.Text)) || !TexBoxMoney.Text.IsNumberContains()) throw new MyException();
                int money = Convert.ToInt32(TexBoxMoney.Text);
                client.AddValue(money);
            }
            catch (MyException error)
            {
                MessageBox.Show($"Неверный формат денежного вклада");
                Debug.WriteLine(error);
            }
            catch (NullReferenceException error)
            {
                MessageBox.Show($"Выберите клиента");
                Debug.WriteLine(error);
            }

        }
        /// <summary>
        /// Перевод
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BankAccount<Client> client1 = ListViewClients.SelectedItem as BankAccount<Client>;
                BankAccount<Client> client2 = ComboBoxClients.SelectedItem as BankAccount<Client>;
               
                if ((string.IsNullOrEmpty(TexBoxSend.Text)) || !TexBoxSend.Text.IsNumberContains()) throw new MyException();
                int money = Convert.ToInt32(TexBoxSend.Text);
                Sber.TransOut(money, client1, client2);
            }
            catch (NullReferenceException error)
            {
                MessageBox.Show($"Выберите кому и куда переводить");
                Debug.WriteLine(error);
            }
            catch (MyException error)
            {
                MessageBox.Show($"Неверный формат денежного перевода");
                Debug.WriteLine(error);
            }


        }

        ///// <summary>
        ///// Для ввода только типа int
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !IsTextAllowed(e.Text);
        //}
        //private static readonly Regex onlyNumbers = new Regex("[^0-9]"); //regex that matches disallowed text
        //private static bool IsTextAllowed(string text)
        //{
        //    return !onlyNumbers.IsMatch(text);
        //}

    }
}