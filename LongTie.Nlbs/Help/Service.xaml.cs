using System;
using System.Collections.Generic;
using System.IO;
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

namespace LongTie.Nlbs.Help
{
    /// <summary>
    /// Interaction logic for Service.xaml
    /// </summary>
    public partial class Service 
    {
        public Service()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadText();
        }
        public void LoadText()
        {

            string textFile = @"售后服务.txt";
            FileStream fs;
            if (File.Exists(textFile))
            {
                fs = new FileStream(textFile, FileMode.Open, FileAccess.Read);
                using (fs)
                {
                    TextRange text = new TextRange(richbox.Document.ContentStart, richbox.Document.ContentEnd);
                    text.Load(fs, DataFormats.Text);
                }

            }
        }
    }
}
