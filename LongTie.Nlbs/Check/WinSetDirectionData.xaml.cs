using LT.BLL.Plc;
using LT.Comm;
using System;
using System.Collections.Generic;
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

namespace LongTie.Nlbs.Check
{
    /// <summary>
    /// Interaction logic for WinSetDirectionData.xaml
    /// </summary>
    public partial class WinSetDirectionData : Window
    {
        EncoderPlc encoderPlc = null;
        public WinSetDirectionData(EncoderPlc EncoderPlc)
        {
            InitializeComponent();
            encoderPlc = EncoderPlc;
        }

        private void bt_set_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (!encoderPlc.Open())
                {
                    MessageAlert.Alert("编码器链接失败,无法设置！");
                    return;
                }
                int direction = Convert.ToInt32(this.tb_direction.Text.Trim());
                encoderPlc.setDirectorData(direction.ToString("x4").ToUpper());
                encoderPlc.setReDirectorData(Convert.ToInt32 (tb_redirection.Text .Trim ()).ToString ("x4").ToUpper ());
                MessageAlert.Alert("设置成功");
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (encoderPlc.Open())
            {
                this.tb_direction.Text = Convert.ToInt32(encoderPlc.getDirectionData(), 16).ToString();
                this.tb_redirection.Text = Convert.ToInt32(encoderPlc.getReDirectionData(), 16).ToString();
            }
       
        }
        bool Validate()
        {
        
            if (string.IsNullOrWhiteSpace(this.tb_redirection.Text.Trim()) || string.IsNullOrWhiteSpace(this.tb_direction.Text.Trim()))
            {
                MessageAlert.Alert("正反向间隙输入值不能为空");
                return false;
            }

            try
            {

                Convert.ToInt32(this.tb_direction.Text.Trim());
                Convert.ToInt32(this.tb_redirection.Text.Trim());
                return true;
            }
            catch
            {
                MessageAlert.Alert("请输入正整数的正反向间隙值");
                return false;
            }
        }

        private void smallredlight_Click(object sender, RoutedEventArgs e)
        {
           if (encoderPlc.Open())
            encoderPlc.SmallRedLight();
        }

        private void smallgreenlight_Click(object sender, RoutedEventArgs e)
        {
            if (encoderPlc.Open())
            encoderPlc.SmallGreenLight();
        }

        private void smalllighton_Click(object sender, RoutedEventArgs e)
        {
            if (encoderPlc.Open())
            encoderPlc.SamllLightOn();
        }

        private void smalllightoff_Click(object sender, RoutedEventArgs e)
        {
            if (encoderPlc.Open())
            encoderPlc.SamllLightOff();
        }

        private void bigredlight_Click(object sender, RoutedEventArgs e)
        {
            if (encoderPlc.Open())
            encoderPlc.BigRedLight();
        }

        private void biggreenlight_Click(object sender, RoutedEventArgs e)
        {
            encoderPlc.BigGreenLight();
        }

        private void biglighton_Click(object sender, RoutedEventArgs e)
        {
            encoderPlc.BigLightOn();
        }

        private void biglightoff_Click(object sender, RoutedEventArgs e)
        {
            encoderPlc.BigLightOff();
        }
    }
}
