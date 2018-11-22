using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QDDL.BLL.Wrench
{

    public class WrenchExcelIn
    {

        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();

        public void DaoIn()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ExcelHelp _excelHelper = new ExcelHelp();
            openFileDialog.Filter = "Excel (*.XLS)|*.xls";
            int i = 0, j = 0;
            if ((bool)(openFileDialog.ShowDialog()))
            {
                try
                {

                    DataTable dt = _excelHelper.LoadExcel(openFileDialog.FileName);
                    List<wrench> s = ListWrench(dt);
                    if (s != null && s.Count > 0)
                    {
                        foreach (wrench w in s)
                        {
                            try
                            {
                                if (Wrench.add(w))
                                { i++; }
                                else
                                {
                                    j++;
                                    LogUtil.WriteLog(typeof(string), "序号为 " + w.id.ToString() + "到入失败：");
                                }
                            }
                            catch
                            {
                                j++;
                                LogUtil.WriteLog(typeof(wrench), w.wrenchBarCode + "导入失败！");
                            }
                        }
                        MessageBox.Show("成功导入" + i.ToString() + "条" + "   失败" + j.ToString() + "条");
                    }
                    // Wrench.addlist(ListWrench (dt));
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(typeof(string), "工具批量导入失败：" + ex);
                    MessageBox.Show("批量导入工具信息失败,填写信息有误");
                    return;
                }
            }
        }

        bool filterInWrench(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count <= 0)
                    return false;
                foreach (DataRow dr in dt.Rows)
                {
                    //for (int i = 1; i < dt.Columns.Count - 1; i++)
                    //{
                    //    if (string.IsNullOrEmpty(dr[i].ToString()))
                    //    {
                    //        MessageAlert.Warning("必填信息不能为空,\n导入失败！");
                    //        return false;
                    //    }
                    //}

                    wrench w = Wrench.selectByBarcode(dr[3].ToString());
                    if (w != null && w.guid != null)
                    {
                        MessageAlert.Warning("序号为" + dr[0].ToString() + " 的工具条码 已经存在！导入失败！");
                        return false;
                    }
                    List<wrenchspecies> wpl = WrenchSpecies.selectbyname(dr[1].ToString());
                    if (wpl.Count <= 0)
                    {
                        MessageAlert.Alert("序号为" + dr[0].ToString() + "的工具种类 不存在！  导入失败！");
                        return false;
                    }
                    try
                    {
                        decimal s = Convert.ToDecimal(dr[4].ToString());
                    }
                    catch { MessageAlert.Warning("序号为" + dr[0].ToString() + "的最大量程值有误  导入失败！"); return false; }

                    try
                    {
                        decimal s = Convert.ToDecimal(dr[5].ToString());
                    }
                    catch
                    { MessageAlert.Warning("序号为" + dr[0].ToString() + "的最小量程值有误  导入失败！"); return false; }

                    try
                    {
                        decimal s = Convert.ToDecimal(dr[6].ToString());
                    }
                    catch { MessageAlert.Warning("序号为" + dr[0].ToString() + "的目标值有误  导入失败！"); return false; }

                    try
                    {
                        decimal s = Convert.ToDecimal(dr[7].ToString());
                    }
                    catch { MessageAlert.Warning("序号为" + dr[0].ToString() + "的辅助校验值1有误  导入失败！"); return false; }

                    try
                    {
                        decimal s = Convert.ToDecimal(dr[8].ToString());
                    }
                    catch { MessageAlert.Warning("序号为" + dr[0].ToString() + "的辅助校验值2有误  导入失败！"); return false; }


                    try
                    {
                        DateTime s = Convert.ToDateTime(dr[9].ToString());
                    }
                    catch { MessageAlert.Warning("序号为" + dr[0].ToString() + "的入库时间格式有误  导入失败！"); return false; }

                    try
                    {
                        DateTime s = Convert.ToDateTime(dr[10].ToString());
                    }
                    catch { MessageAlert.Warning("序号为" + dr[0].ToString() + "的最近维护时间格式有误  导入失败！"); return false; }


                    if (Convert.ToDecimal(dr[4].ToString()) < Convert.ToDecimal(dr[5].ToString()))
                    {
                        MessageAlert.Warning("序号为" + dr[0].ToString() + "的量程最大值不能小于最小值！  导入失败！");
                        return false;

                    }
                    if (!(Convert.ToDecimal(dr[5].ToString()) <= Convert.ToDecimal(dr[6].ToString()) && Convert.ToDecimal(dr[6].ToString()) <= Convert.ToDecimal(dr[4].ToString())))
                    {
                        MessageAlert.Warning("序号为" + dr[0].ToString() + "的默认目标值不能超出扳手的量程！  导入失败！");
                        return false;
                    }

                    if (!(Convert.ToDecimal(dr[5].ToString()) <= Convert.ToDecimal(dr[7].ToString()) && Convert.ToDecimal(dr[7].ToString()) <= Convert.ToDecimal(dr[4].ToString())))
                    {
                        MessageAlert.Warning("序号为" + dr[0].ToString() + "的辅助校验值1不能超出扳手的量程！  导入失败！");
                        return false;
                    }
                    if (!(Convert.ToDecimal(dr[5].ToString()) <= Convert.ToDecimal(dr[8].ToString()) && Convert.ToDecimal(dr[8].ToString()) <= Convert.ToDecimal(dr[4].ToString())))
                    {
                        MessageAlert.Warning("序号为" + dr[0].ToString() + "的辅助校验值2不能超出扳手的量程！  导入失败！");
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(dr[11].ToString()))
                    {
                        MessageAlert.Warning("序号为" + dr[0].ToString() + "的生产厂家不能为空！  导入失败！");
                        return false;
                    }

                }

                return true;
            }
            catch { return false; }
        }

        List<wrench> ListWrench(DataTable sourcetabledata)
        {
            List<wrench> listwrench = new List<wrench>();
            List<string> log = new List<string>();
            if (sourcetabledata == null || sourcetabledata.Rows.Count < 1)
                return listwrench;
            try
            {
                if (!filterInWrench(sourcetabledata))
                {
                    return listwrench = null; ;
                }
                foreach (DataRow dr in sourcetabledata.Rows)
                {
                    List<wrenchspecies> wl = WrenchSpecies.selectbyname(dr[1].ToString());
                    wrenchstatus ws = WrenchStatus.selectByStatusDM("001");
                    listwrench.Add(new wrench()
                    {
                        id = dr[0] != DBNull.Value ? Convert.ToInt32(dr[0].ToString()) : 0,
                        wrenchCode = dr[2].ToString(),
                        wrenchBarCode = dr[3].ToString(),
                        rangeMax = Convert.ToDecimal(dr[4].ToString()),
                        rangeMin = Convert.ToDecimal(dr[5].ToString()),
                        targetvalue = Convert.ToDecimal(dr[6].ToString()),
                        targetvalue1 = dr[7] == null ? 0 : Convert.ToDecimal(dr[7].ToString()),
                        targetvalue2 = dr[8] == null ? 0 : Convert.ToDecimal(dr[8].ToString()),
                        factory = dr[11] == null ? " " : dr[11].ToString(),
                        createDate = dr[9] != null ? Convert.ToDateTime(Convert.ToDateTime(dr[9]).ToString("yyyy-MM-dd HH:mm:ss")) : Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        comment = dr[12] == null ? " " : dr[12].ToString(),
                        species = wl.FirstOrDefault().id.ToString(),
                        status = ws.id.ToString(),
                        guid = Guid.NewGuid().ToString(),
                        unit = "N.m",
                        cycletime = 0,
                        isallowcheck = true,
                        lastrepair = dr[10] != null ? Convert.ToDateTime(Convert.ToDateTime(dr[10]).ToString("yyyy-MM-dd HH:mm:ss")) : Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    });
                }
                return listwrench;
            }
            catch { return listwrench = null; }
        }
    }
    public class WrenchExcelOut
    {
        DataTable Header(DataTable dt1)
        {
            DataTable dt = new DataTable();
            dt = dt1;
            List<string> headlist = new List<string>() { "序 号", "扭力扳手编号", "扳手条码号", "扳手种类", "默认校验值(N.m)", "辅助校验值1(N.m)", "辅助校验值2(N.m)", "量程上限(N.m)", "量程下限(N.m)", "设备状态", "最近维护时间", "有效时长（天）", "生产厂家", "备注" };

            foreach (string s in headlist)
            {
                dt.Columns.Add(s);
            }
            return dt;
        }
        public DataTable ToTable(List<ToolModel> tm)
        {
            DataTable dt = new DataTable();
            dt = Header(dt);
            object[] values = new object[14];
            int count = 0;
            foreach (ToolModel t in tm)
            {
                count++;
                values[0] = count.ToString();
                values[1] = t.wrenchCode.ToString() + "\t";
                values[2] = t.wrenchBarCode;
                values[3] = t.speciesName;
                try
                {
                    values[4] = t.targetvalue.ToString("f2");
                    values[5] = t.targetvalue1.ToString("f2");
                    values[6] = t.targetvalue2.ToString("f2");
                    values[7] = t.rangeMax.ToString("f2");
                    values[8] = t.rangeMin.ToString("f2");
                }
                catch { continue; }

                values[9] = t.statusName;
                values[10] = t.lastrepair + "\t";
                values[11] = t.cycletime.ToString("f0");
                values[12] = t.factory;
                values[13] = t.comment;
                dt.Rows.Add(values);
            }
            return dt;
        }

    }
}
