using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace EACharge_Out
{
    /// <summary>
    /// Interaction logic for ConnectionSettingsWindow.xaml
    /// </summary>
    public partial class ConnectionSettingsWindow : Window
    {
        Master _master;
        
        public ConnectionSettingsWindow(Master master)
        {
            InitializeComponent();            
            _master = master;
            GetPorts();
            GetInfo();            
        }

        /// <summary>
        /// Выводит в окно инфо о сконфигурированном ранее COM порте
        /// </summary>
        public void GetInfo()
        {
            txtbxAddress.Text = _master.SlaveAddress.ToString();
            cmbbxPorts.Text = _master._SerialPort.PortName;
            cmbbxBaudRate.Text = _master._SerialPort.BaudRate.ToString();
            if (_master._SerialPort.StopBits == StopBits.One)
            {
                switch (_master._SerialPort.Parity)
                {
                    case Parity.None:
                        cmbbxParity.Text = "8N1";
                        break;
                    case Parity.Odd:
                        cmbbxParity.Text = "8O1";
                        break;
                    case Parity.Even:
                        cmbbxParity.Text = "8E1";
                        break;                    
                }               
            }
            else cmbbxParity.Text = "8N2";
        }

        /// <summary>
        /// Получает список доступных COM портов и выводит его в cmbbxPorts
        /// </summary>
        public void GetPorts()
        {
            cmbbxPorts.ItemsSource = SerialPort.GetPortNames();
            if (cmbbxPorts.Items.Count != 0) cmbbxPorts.SelectedIndex = 0; 
        }

        /// <summary>
        /// Сохраняет введенные данные в соответствующие поля SerialPort из master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            _master.SlaveAddress = Convert.ToByte(txtbxAddress.Text);
            _master._SerialPort.PortName = cmbbxPorts.Text;
            _master._SerialPort.BaudRate = Convert.ToInt32(cmbbxBaudRate.Text);
            switch (cmbbxParity.Text)
            {
                case "8N1":
                    _master._SerialPort.Parity = Parity.None;
                    _master._SerialPort.StopBits = StopBits.One;
                    break;
                case "8O1":
                    _master._SerialPort.Parity = Parity.Odd;
                    _master._SerialPort.StopBits = StopBits.One;
                    break;
                case "8E1":
                    _master._SerialPort.Parity = Parity.Even;
                    _master._SerialPort.StopBits = StopBits.One;
                    break;
                case "8N2":
                    _master._SerialPort.Parity = Parity.None;
                    _master._SerialPort.StopBits = StopBits.Two;
                    break;
            }

            // обновляет строку инфо в master и через привязку в UI
            _master.UpdateInfo();

            this.Close();            
        }

        /// <summary>
        /// Отмена, возвращает на основной экран
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        /// <summary>
        /// Получает список доступных COM портов и выводит его в cmbbxPorts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdatePorts_Click(object sender, RoutedEventArgs e)
        {
            GetPorts();
        }

        /// <summary>
        /// Проверяет, что вводятся только цифры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            
            // TODO проверка больше 255
        }

        /// <summary>
        /// Проверяет не оставлено ли поле пустым и если оставлено записывает туда старое значение адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtbxAddress_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == String.Empty)
            txtbxAddress.Text = _master.SlaveAddress.ToString();
        }
    }
}
