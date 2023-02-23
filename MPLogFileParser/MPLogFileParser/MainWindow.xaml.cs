
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;

namespace MPLogFileParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _inputFile;
        private string _outputFile;
        private bool _filtDateTime;
        private int[] _dateTimeFrom;
        private int[] _dateTimeTo;
        public MainWindow()
        {
            InitializeComponent();
            _inputFile = txtSelectInput.Text;
            _outputFile = txtSelectOutput.Text;
            if (chkDateTime.IsChecked == true) 
            { 
                _filtDateTime = true; 
                
            }
            _dateTimeFrom = new int[4];
            _dateTimeTo = new int[4];


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSelectInput_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _inputFile = openFileDialog.FileName;
                txtSelectInput.Text = _inputFile;
            }
                

        }

        private void btnSelectOutput_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                _outputFile = saveFileDialog.FileName;
                txtSelectOutput.Text = _outputFile;
            }
                
        }

        private void chkDateTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            bool b = false;
            if (chkDateTime.IsChecked == true) { b = true; }

            txtDateTimeFrom.IsEnabled = b;
            txtDateTimeTo.IsEnabled = b;
            _filtDateTime = b;

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void btnParse_Click(object sender, RoutedEventArgs e)
        {
            string val = ValidationChecks();
            if (val == "")
            {
                this.Close();
                var Parser = new Parser(_inputFile, _outputFile);
                Parser.ParserMain();
            }
            else
            {
                var result = MessageBox.Show(val, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            
        }

        private string ValidationChecks()
        {
            //Purpose: runs validation checks on all fields on the form
            if (!File.Exists(_inputFile))
                return "Invalid input file specified.";
            if (!Directory.Exists(Path.GetDirectoryName(_outputFile)))
                return "Invalid output directory specified.";
            if (chkDateTime.IsChecked == true)
            {
                if (!ValidateDateTime(txtDateTimeFrom.Text, _dateTimeFrom))
                    return "Invalid DateTime From specified.";
                if (!ValidateDateTime(txtDateTimeTo.Text, _dateTimeTo))
                    return "Invalid DateTime To specified.";
            }

            return "";

        }

        private bool ValidateDateTime(string dateTime, int[] dateTimeVals)
        {
            //define DateTime validation checks
            int[] min = { 0, 0, 0, 0 };
            int[] max = { 31, 23, 59, 59 };
            int numVal = 4;
            int[] numChar = { 1, 2 };

            //perform checks
            string[] dateTimeArr = dateTime.Split(":");
            if (dateTimeArr.Length != numVal)
                return false;
            for (int i = 0; i < numVal; i++)
            {
                if (dateTimeArr[i].Length < numChar[0] || dateTimeArr[i].Length > numChar[1])
                    return false;
                int o;
                if (!int.TryParse(dateTimeArr[i], out o))
                    return false;
                if (o < min[i] || o > max[i])
                    return false;
                dateTimeVals[i] = o;
            }

            return true;
        }

        private void txtSelectInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _inputFile = txtSelectInput.Text;
        }

        private void txtSelectOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _outputFile = txtSelectOutput.Text;
        }
    }
}
