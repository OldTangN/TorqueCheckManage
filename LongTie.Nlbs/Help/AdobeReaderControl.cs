using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LongTie.Nlbs.Help
{
    public partial class AdobeReaderControl : UserControl
    {
        public AdobeReaderControl(string filename)
        {
            InitializeComponent();
            this.axAcroPDF1.LoadFile(filename);
           // axAcroPDF1.setShowToolbar(false);
            axAcroPDF1.setShowScrollbars(false);

        }
    }
}
