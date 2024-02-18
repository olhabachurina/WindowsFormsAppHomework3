using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace WindowsFormsAppHomework3Sp
{
    public partial class Form1 : Form
    {
        private Bank _bank;
        public Form1()
        {
            InitializeComponent();
            SynchronizationContext syncContext = SynchronizationContext.Current;

            // Создаем объект Bank, передавая ListBox и контекст синхронизации
            _bank = new Bank(0, "", 0, lstLog, syncContext);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtMoney.Text, out int money) && int.TryParse(txtPercent.Text, out int percent))
            {
                _bank.Money = money;
                _bank.Name = txtName.Text;
                _bank.Percent = percent;
            }
            else
            {
                MessageBox.Show("Please, enter valid values for money and percent.");
            }
        }
    }
    public class Bank
    {
        private int _money;
        private string _name;
        private int _percent;
        private ListBox _lstLog;
        private SynchronizationContext _syncContext;
        private readonly object _lock = new object();

        public Bank(int money, string name, int percent, ListBox lstLog, SynchronizationContext syncContext)
        {
            _money = money;
            _name = name;
            _percent = percent;
            _lstLog = lstLog;
            _syncContext = syncContext; // Сохраняем контекст синхронизации
        }
        private void UpdateListBox(string text)
        {
            _syncContext.Post(new SendOrPostCallback(o =>
            {
                _lstLog.Items.Add(o.ToString());
            }), text);
        }
        public int Money
        {
            get => _money;
            set
            {
                lock (_lock)
                {
                    if (_money != value) // Проверка на изменение значения
                    {
                        _money = value;
                        Task.Run(() => UpdateListBox($"Money updated: {_money}"));
                    }
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                lock (_lock)
                {
                    if (_name != value) // Проверка на изменение значения
                    {
                        _name = value;
                        Task.Run(() => UpdateListBox($"Name updated: {_name}"));
                    }
                }
            }
        }

        public int Percent
        {
            get => _percent;
            set
            {
                lock (_lock)
                {
                    if (_percent != value) // Проверка на изменение значения
                    {
                        _percent = value;
                        Task.Run(() => UpdateListBox($"Percent updated: {_percent}"));
                    }
                }
            }
        }
    }
}
