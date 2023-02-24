using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System;
using Microsoft.VisualBasic.FileIO;

namespace MPLogFileParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //class properties
        private readonly ParseParameters _parseParam = new ParseParameters();

        public MainWindow()
        {
            InitializeComponent();
            _parseParam.InputFile = txtSelectInput.Text;
            _parseParam.OutputFile = txtSelectOutput.Text;
            if (chkDateTime.IsChecked == true)
            {
                _parseParam.FiltDateTime = true;
            }
        }

        private void txtSelectInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _parseParam.InputFile = txtSelectInput.Text;
        }

        private void txtSelectOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _parseParam.OutputFile = txtSelectOutput.Text;
        }

        private void btnSelectInput_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _parseParam.InputFile = openFileDialog.FileName;
                txtSelectInput.Text = _parseParam.InputFile;
            }


        }

        private void btnSelectOutput_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                _parseParam.OutputFile = saveFileDialog.FileName;
                txtSelectOutput.Text = _parseParam.OutputFile;
            }

        }

        private void chkDateTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            bool b = false;
            if (chkDateTime.IsChecked == true) { b = true; }

            txtDateTimeFrom.IsEnabled = b;
            txtDateTimeTo.IsEnabled = b;
            _parseParam.FiltDateTime = b;

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void btnParse_Click(object sender, RoutedEventArgs e)
        {
            string val = ValidationChecks(); //run validation checks before continuing to parse
            if (val == "")
            {
                this.Close();
                var parser = new Program(_parseParam);
                parser.ProgramMain();
            }
            else
            {
                //at least one error; show messagebox and don't close form
                var result = MessageBox.Show(val, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private string ValidationChecks()
        {
            //Purpose: runs validation checks on all fields on the form
            //Result: returns "" if valid, returns string with error text value if invalid
            if (!File.Exists(_parseParam.InputFile))
                return "Invalid input file specified.";
            if (!Directory.Exists(Path.GetDirectoryName(_parseParam.OutputFile)))
                return "Invalid output directory specified.";
            if (chkDateTime.IsChecked == true)
            {
                if (!_parseParam.DateTimeFrom.SetDateTime(txtDateTimeFrom.Text))
                    return "Invalid DateTime From specified.";
                if (!_parseParam.DateTimeTo.SetDateTime(txtDateTimeTo.Text))
                    return "Invalid DateTime To specified.";
            }
            return "";
        }
    }
}
