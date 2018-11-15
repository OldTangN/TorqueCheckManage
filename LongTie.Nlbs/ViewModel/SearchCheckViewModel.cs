using LT.BLL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LongTie.Nlbs.ViewModel
{
    public class SearchCheckViewModel : INotifyPropertyChanged
    {
               #region Fileds
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ImportCmd { get; set; }
        public ICommand ExportCmd { get; set; }
        private ExcelHelp _excelHelper;
        private DataView _dataGridSource;
        #endregion

        #region Construct
        public SearchCheckViewModel()
        {
            _excelHelper = new ExcelHelp();
            _dataGridSource = new DataView();
            ImportCmd = new RelayCommand<object>(ImportCmdExcute, ImportCmdCanExcute);
            ExportCmd = new RelayCommand<object>(ExportCmdExcute, ExportCmdCanExcute);
        }
        #endregion

        #region Propertys
        public DataView DataGridSource
        {
            get { return _dataGridSource; }
            set
            {
                _dataGridSource = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DataGridSource"));
                }
            }
        }
        #endregion

        #region Method
        private bool ImportCmdCanExcute(object praram)
        {
            return true;
        }
        private void ImportCmdExcute(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel (*.XLS)|*.xls"; ;
            if ((bool)(openFileDialog.ShowDialog()))
            {
                string excelName = openFileDialog.FileName;
                DataTable dt = _excelHelper.LoadExcel(excelName);
                DataGridSource = dt.DefaultView;

            }
        }

        private bool ExportCmdCanExcute(object param)
        {
            if (_dataGridSource.Count < 1)
            {
                return false;
            }
            return true;
        }
        private void ExportCmdExcute(object param)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel (*.XLS)|*.xls"; ;
            if ((bool)(saveFileDialog.ShowDialog()))
            {
                try
                {
                    _excelHelper.SaveToExcel(saveFileDialog.FileName, _dataGridSource.Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }
                MessageBox.Show("导出成功");

            }
        }


        #endregion
    }
}
