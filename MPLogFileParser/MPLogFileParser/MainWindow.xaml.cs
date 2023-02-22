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
        public MainWindow()
        {
            InitializeComponent();
            _inputFile = txtSelectInput.Text;
            _outputFile = txtSelectOutput.Text;
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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void btnParse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var Parser = new Parser(_inputFile, _outputFile);
            Parser.ParserMain();
            
        }
    }
}
