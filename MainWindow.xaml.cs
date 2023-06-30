using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isInputed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Очистка введеного числа
            Input.Text = "0";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Очистка всего введеного
            Input.Text = "0"; 
            Verh.Text = string.Empty;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Ввод цифр
            Button button = (Button)sender;
            if (button.Content.ToString() == "0" && Input.Text == "0")
                Input.Text = "0";
            else 
            {
                if (Input.Text == "0" || !isInputed)
                    Input.Text = "";
                Input.Text += button.Content;
                isInputed = true;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Очистка последней цифры
            Input.Text = Input.Text.Substring(0, Input.Text.Length - 1);
            if (Input.Text == "")
                Input.Text = "0";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Стандартные операции + - * /
            Button button = (Button)sender;
            if (isInputed)
            {
                Calculate();
                Verh.Text += $"{Input.Text}{button.Content}";
                isInputed = false;
            }
            else
            {
                if (Verh.Text.Length != 0)
                {
                    Verh.Text = Verh.Text.Substring(0, Verh.Text.Length - 1);
                    Verh.Text += button.Content;
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // Запятая
            if (!Input.Text.Contains(',')) 
            {
                if (!isInputed)
                    Input.Text = "0";
                Input.Text += ",";
                isInputed = true;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // =
            Calculate();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            // 1/x
            string b = Input.Text.Replace(",", ".");
            string for_calc = "1/" + b;
            string result = new DataTable().Compute(for_calc, null).ToString();
            if (!DivisionOnZeroCheck(result))
                Input.Text = result;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            // x^2
            string b = Input.Text.Replace(",", ".");
            string for_calc = b + "*" + b;
            string result = new DataTable().Compute(for_calc, null).ToString();
            Input.Text = result;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            // √x
            double b = double.Parse(Input.Text.Replace(",", "."), CultureInfo.InvariantCulture);
            
            string result = Math.Sqrt(b).ToString();
            Input.Text = result;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            // +/-
            if (Input.Text != "0" && Input.Text.Length != 0)
            {
                if (Input.Text.Contains("-")) 
                    Input.Text = Input.Text.TrimStart('-');
                else 
                    Input.Text = Input.Text.Insert(0, "-");
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            // % 
            if (Verh.Text.Contains("-") || Verh.Text.Contains("+"))
            {
                string verh_without_operations = Verh.Text.TrimEnd(new char[] { '-', '+' });
                verh_without_operations = verh_without_operations.Replace(",", ".");
                string for_calc = $"{verh_without_operations}*({Input.Text.Replace(",", ".")}/100)";
                string result = new DataTable().Compute(for_calc, null).ToString();
                Input.Text = result;
            }
            else if (Verh.Text.Contains("/") || Verh.Text.Contains("*"))
            {
                string verh_without_operations = Verh.Text.TrimEnd(new char[] { '/', '*' });
                verh_without_operations = verh_without_operations.Replace(",", ".");
                string for_calc = $"({Input.Text.Replace(",", ".")}/100)";
                string result = new DataTable().Compute(for_calc, null).ToString();
                Input.Text = result;
            }
        }

        private void Calculate()
        {
            // Функция вычисления 
            Verh.Text += Input.Text;
            string for_calc = Verh.Text.Replace(",", ".");
            string result = new DataTable().Compute(for_calc, null).ToString();
            if (!DivisionOnZeroCheck(result))
                Input.Text = result;
            Verh.Text = "";
            isInputed = true;
        }

        private bool DivisionOnZeroCheck(string num_str)
        {
            // Проверка деления на ноль
            if (num_str == "∞")
            {
                MessageBox.Show("Деление на ноль невозможно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else
                return false;
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            // Добавление в память (MS)
            M.Items.Add(Input.Text);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            // Очистка памяти (MC)
            M.Items.Clear();
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            // Вызов из памяти (MR)
            if (M.SelectedItem != null)
                Input.Text = M.SelectedItem.ToString();
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            // Прибавление введеного числа в выбранное в памяти (M+)
            if (M.SelectedItem != null)
            {
                int selected_index = M.SelectedIndex;
                string memory_value = M.SelectedItem.ToString();
                string for_calc = $"{memory_value}+{Input.Text}";
                for_calc = for_calc.Replace(",", ".");
                M.Items[selected_index] = new DataTable().Compute(for_calc, null).ToString();
                M.SelectedIndex = selected_index;
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            // Вычитание введеного числа из выбранного в памяти (M-)
            if (M.SelectedItem != null)
            {
                int selected_index = M.SelectedIndex;
                string memory_value = M.SelectedItem.ToString();
                string for_calc = $"{memory_value}-{Input.Text}";
                for_calc = for_calc.Replace(",", ".");
                M.Items[selected_index] = new DataTable().Compute(for_calc, null).ToString();
                M.SelectedIndex = selected_index;
            }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            // Удаление из памяти выбранной ячейки (MD)
            if (M.SelectedItem != null)
            {
                int selected_index = M.SelectedIndex;
                M.Items.RemoveAt(selected_index);
                M.SelectedItem = null;
            }
        }
    }
}
