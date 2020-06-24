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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace EACharge_Out
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsUIBlocked { get; set; }

        Master master;

        object selectedRWControl;

        Dictionary<object, RegisterBase> tableOfRegisters;

        List<object> rwControls; // элементы управления через которые записываются / считываются данные

        public MainWindow()
        {
            InitializeComponent();
            master = new Master();
            this.DataContext = master.EAChargeMonitor;
            txtblckStatus.DataContext = master;
            GetAllRWControls();
            GetTableOfRegisters();

            if(!master.ArePortsExist())
            {
                BlockUI();                
                master.UpdateInfo();
                MessageBox.Show("В системе не обнаружены COM порты", "Ошибка");
            }
        }

        public void BlockUI()
        {
            foreach (var child in MyGrid2.Children)
            {
                if (!(child is TextBlock)) (child as UIElement).IsEnabled = false;
            }

            foreach (var child in MyGrid1.Children)
            {
                if (!(child is TextBlock)) (child as UIElement).IsEnabled = false;
            }
            IsUIBlocked = true;
        }

        public void UnblockUI()
        {
            foreach (var child in MyGrid2.Children)
            {
                if (!(child is TextBlock)) (child as UIElement).IsEnabled = true;
            }

            foreach (var child in MyGrid1.Children)
            {
                if (!(child is TextBlock)) (child as UIElement).IsEnabled = true;
            }
            IsUIBlocked = false;
        }

        public void GetAllRWControls()
        {
            rwControls = new List<object>();

            foreach (var child in MyGrid2.Children)
            {
                if (child is TextBox) rwControls.Add(child as TextBox);
            }

            foreach (var child in MyGrid1.Children)
            {
                if (child is TextBox) rwControls.Add(child as TextBox);
                else if (child is ComboBox) rwControls.Add(child as ComboBox);
            }

        }

        public void GetTableOfRegisters()
        {
            tableOfRegisters = new Dictionary<object, RegisterBase>
            {
                {txtbxResistance, master.EAChargeMonitor.Registers[0] },
                {txtbxVoltage, master.EAChargeMonitor.Registers[1] },
                {txtbxCapacitance, master.EAChargeMonitor.Registers[2] },
                {txtbxVoltagePlus, master.EAChargeMonitor.Registers[3] },
                {txtbxVoltageMinus, master.EAChargeMonitor.Registers[4] },
                {txtbxResistancePlus, master.EAChargeMonitor.Registers[5] },
                {txtbxResistanceMinus, master.EAChargeMonitor.Registers[6] },
                {txtbxPreAlarm, master.EAChargeMonitor.Registers[7] },
                {txtbxAlarm, master.EAChargeMonitor.Registers[8] },
                {txtbxAddress, master.EAChargeMonitor.Registers[9] },
                {cmbbxBaudRate, master.EAChargeMonitor.Registers[10] },
                {cmbbxParity, master.EAChargeMonitor.Registers[11] },
                {txtbxDelay, master.EAChargeMonitor.Registers[12] },
                {txtbxDeviceName, master.EAChargeMonitor.Registers[13] },
                {txtbxID, master.EAChargeMonitor.Registers[14] },
                {txtbxFirmwareVersion, master.EAChargeMonitor.Registers[15] }
            };
        }



        private void LastFocusedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            selectedRWControl = sender;
        }

        private void BtnReadAllSettingRegisters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                master.ReadAllSettingRegisters();
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("Время ожидания истекло");
            }
            catch (Modbus.SlaveException ex)
            {
                MessageBox.Show("Ошибка чтения");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Выбранный порт занят", "Ошибка");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("Порт с указанным именем не существует", "Ошибка");
                if (!master.ArePortsExist())
                {
                    MessageBox.Show("В системе нет доступных COM портов", "Ошибка");
                    BlockUI();
                }

            }
        }

        private void BtnReadOneSettingRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedRWControl is TextBox)
                {
                    master.ReadRegisters(tableOfRegisters[selectedRWControl as TextBox]);
                }
                else if (selectedRWControl is ComboBox)
                {
                    master.ReadRegisters(tableOfRegisters[selectedRWControl as ComboBox]);
                }
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("Время ожидания истекло");
            }
            catch (Modbus.SlaveException ex)
            {
                MessageBox.Show("Ошибка чтения");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Выбранный порт занят", "Ошибка");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("Порт с указанным именем не существует", "Ошибка");
                if (!master.ArePortsExist())
                {
                    MessageBox.Show("В системе нет доступных COM портов", "Ошибка");
                    BlockUI();
                }
            }
        }

        private void BtnWriteOneSettingRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedRWControl is TextBox)
                    master.WriteRegisters(tableOfRegisters[selectedRWControl as TextBox]);
                else if (selectedRWControl is ComboBox)
                    master.WriteRegisters(tableOfRegisters[selectedRWControl as ComboBox]);
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("Время ожидания истекло.");
            }
            catch (Modbus.SlaveException ex)
            {
                MessageBox.Show("Ошибка записи", "Ошибка");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Выбранный порт занят", "Ошибка");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("Порт с указанным именем не существует", "Ошибка");
                if (!master.ArePortsExist())
                {
                    MessageBox.Show("В системе нет доступных COM портов", "Ошибка");
                    BlockUI();
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            master.Reset();
        }

        private void BtnReadAllMeasuringRegisters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                master.ReadAllMeasuringRegisters();
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("Время ожидания истекло", "Таймаут");
            }
            catch (Modbus.SlaveException ex)
            {
                MessageBox.Show("Ошибка чтения", "Ошибка");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Выбранный порт занят", "Ошибка");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("Порт с указанным именем не существует", "Ошибка");
                if (!master.ArePortsExist())
                {
                    MessageBox.Show("В системе нет доступных COM портов", "Ошибка");
                    BlockUI();
                }
            }
        }

        private void BtnReadOneMeasuringRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tableOfRegisters.ContainsKey(selectedRWControl as TextBox))
                {
                    master.ReadRegisters(tableOfRegisters[selectedRWControl as TextBox]);
                }
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("Время ожидания истекло");
            }
            catch (Modbus.SlaveException ex)
            {
                MessageBox.Show("Ошибка чтения");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Выбранный порт занят", "Ошибка");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("Порт с указанным именем не существует", "Ошибка");
                if (!master.ArePortsExist())
                {
                    MessageBox.Show("В системе нет доступных COM портов", "Ошибка");
                    BlockUI();
                }
            }

        }

        private void CmbbxBaudRate_LostFocus(object sender, RoutedEventArgs e)
        {
            selectedRWControl = sender;
        }

        private void CmbbxParity_LostFocus(object sender, RoutedEventArgs e)
        {
            selectedRWControl = sender;
        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            ConnectionSettingsWindow connectionSettingsWindow = new ConnectionSettingsWindow(master);
            connectionSettingsWindow.ShowDialog();
            if (IsUIBlocked && master.ArePortsExist() && master._SerialPort.PortName != " ")
            {
                UnblockUI();                
            }
        }

        private async void BtnSearchOfDevice_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow ww = new WaitingWindow();
            ww.Show();
            if (await Task.Run(() => master.FindDevice()))
            {
                ww.Close();
                MessageBox.Show("Устройство найдено. Параметры устройства выведены на информационную панель.", "Поиск устройства");
            }
            else
            {
                ww.Close();
                MessageBox.Show("Устройство не найдено", "Поиск устройства");
            }

        }

        private void BtnFactorySetting_Click(object sender, RoutedEventArgs e)
        {
            master.FactorySetting();
        }

        private void BtnStartStopPoll_Click(object sender, RoutedEventArgs e)
        {
            if (master.IsPoll)
            {
                master.StopPoll();
                btnReadAllMeasuringRegisters.IsEnabled = true;
                btnReadOneMeasuringRegister.IsEnabled = true;
                numUpDownInterval.IsEnabled = true;
                btnStartStopPoll.Content = "Начать опрос";
            }
            else
            {
                master.StartPoll(Convert.ToInt32(numUpDownInterval.Text));
                btnReadAllMeasuringRegisters.IsEnabled = false;
                btnReadOneMeasuringRegister.IsEnabled = false;
                numUpDownInterval.IsEnabled = false;
                btnStartStopPoll.Content = "Остановить опрос";

            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            // TODO проверка больше 255
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
