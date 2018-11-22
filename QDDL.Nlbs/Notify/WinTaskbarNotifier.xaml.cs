using QDDL.Model;
using QDDL.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFTaskbarNotifier;

namespace QDDL.Nlbs.Notify
{
    /// <summary>
    /// Interaction logic for WinTaskbarNotifier.xaml
    /// </summary>
    public class NotifyObject
    {
        public NotifyObject(List<WrenchNotice> wrench)
        {
           // this.dataGrid1.
            this.wrench = wrench;
        }
        private List<WrenchNotice> wrench;
        public List<WrenchNotice> Wrench
        {
            get { return this.wrench; }
            set { this.wrench = value; }
        }
    }

    public partial class WinTaskbarNotifier : TaskbarNotifier
    {
        public WinTaskbarNotifier()
        {
            InitializeComponent();
          
        }

        private ObservableCollection<NotifyObject> notifyContent;
        /// <summary>
        /// A collection of NotifyObjects that the main window can add to.
        /// </summary>
        public ObservableCollection<NotifyObject> NotifyContent
        {
            get
            {
                if (this.notifyContent == null)
                {
                    // Not yet created.
                    // Create it.
                    this.NotifyContent = new ObservableCollection<NotifyObject>();
                }

                return this.notifyContent;
            }
            set
            {
                this.notifyContent = value;
            }
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            this.ForceHidden();
        }


    }
}
