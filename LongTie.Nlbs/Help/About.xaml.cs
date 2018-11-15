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
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
          // this.richbox.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText); 
            LoadText();
        }
        //public static void LoadFromRTF(this RichTextBox richTextBox, string rtf)
        //{
        //    if (string.IsNullOrEmpty(rtf))
        //    {
        //        throw new ArgumentNullException();
        //    }
        //    TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (StreamWriter sw = new StreamWriter(ms))
        //        {
        //            sw.Write(rtf);
        //            sw.Flush();
        //            ms.Seek(0, SeekOrigin.Begin);
        //            textRange.Load(ms, DataFormats.Rtf);
        //        }
        //    }
        //}
        public void LoadText()
        {

            string textFile = @"关于本软件.dat";
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
