using LT.BLL;
using LT.Comm;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongTie.Nlbs.Check
{
    class Class1
    {

        int _checktype = 3;//3 三点校验-  1-一点校验
        int _checkcount = 0;//记录校验次数//
        int _checkarr = 0;//记录校验组数
        bool _checkover = false; //校验完成 false 未完成
        private bool _oneresult = false;//一次校验结果 成功失败
        private bool _checkresult = false;//校验结果  最中总结过
        int _checkindex = 0;//校验数据位置
        bool _testertype = false;//校验仪true t1  false t2
        int _listid = 1;
        List<ShowCheckresult> showcheckset = new List<ShowCheckresult>();
        ReadCheckTester rct1 = null;
        ReadCheckTester rct2 = null;
        systemcheckset sysset = null;
        public Class1(ReadCheckTester RCT1, ReadCheckTester RCT2)
        {
            rct1 = RCT1;
            rct2 = RCT2;
            rct1.HandDataBack += new ReadCheckTester.DeleteDataBack(Hand1show);
            rct2.HandDataBack += new ReadCheckTester.DeleteDataBack(Hand2show);
        }

        void Hand1show(Object sender, EventArgs e)
        {
            if (!_checkover)
            {
                ReadCheckTester t = (ReadCheckTester)sender;
                if (_testertype)
                    testerType(t.back());
            }

        }

        void Hand2show(Object sender, EventArgs e)
        {
            if (!_checkover)
            {
                ReadCheckTester t = (ReadCheckTester)sender;
                if (!_testertype)
                    testerType(t.back());
            }
        }


        void testerType(string data)
        {
            decimal allowmin = 0;
            decimal allowmax = 0;
            ShowCheckresult cr = CheckResult(cheeckDataValidata(Convert.ToDecimal(data)), allowmin, allowmax);
            if (cr == null)
            {
                return;
            }
            checkControl();
        }
        void checkControl()
        {
            if (isLastCheckArry())
            {
                if (isLastCheckData())
                {
                    if (isLastData())
                    {
                        //扳手合格，显示结果 校验完成
                        _checkover = true;
                        _checkresult = true;
                        return;
                    }
                    else
                    {
                        ///改变目标值继续
                        _checkindex++;
                        _checkcount = 0;
                        _checkarr = 0;
                    }
                }
                else
                {
                    //扳手不和个 停止校验
                    _checkresult = false;
                    _checkover = true;
                    return;
                }
            }
            else
            {
                if (isLastCheckData())
                {
                    if (isLastData())
                    {
                        //扳手合格，显示结果
                        _checkover = true;
                        _checkresult = true;
                        return;
                    }
                    else
                    {
                        ///改变目标值继续
                        _checkindex++;
                        _checkcount = 0;
                        _checkarr = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 绑定显示数据
        /// </summary>
        void CheckBinddata()
        {

        }
        /// <summary>
        /// 获取设置校验次数组数
        /// </summary>
        void getSystemSet()
        {
            List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
            if (scsl != null || scsl.Count > 0)
                sysset = scsl.FirstOrDefault();
            sysset = null;
        }


        decimal cheeckDataValidata(decimal backdata)
        {
            try
            {
                if (sysset == null || sysset.throwvalue == null || sysset.throwvalue <= 0)
                    return backdata;

                decimal targetvalue = Convert.ToDecimal(GetTargetValue());
                decimal throwdatemin = targetvalue * (1 - (1 - ((sysset.throwvalue ?? 0) / 100)));
                decimal throwdatemax = targetvalue * (1 + (1 - ((sysset.throwvalue ?? 0) / 100)));
                if (Math.Abs(backdata) > throwdatemax || Math.Abs(backdata) < throwdatemin)
                    return 0;
                return backdata;
            }
            catch { return backdata; }

        }
        double GetTargetValue()
        {
            return .9;
        }

        /// <summary>
        /// 显示校验数据
        /// </summary>
        /// <param name="checkdata"></param>
        /// <param name="allowmin"></param>
        /// <param name="allwmax"></param>
        /// <returns></returns>
        ShowCheckresult CheckResult(decimal checkdata, decimal allowmin, decimal allwmax)
        {
            ShowCheckresult sc = new ShowCheckresult();
            try
            {
                if (checkdata == 0)
                    return null;
                if (checkdata < 0)
                {
                    sc.isturn = false;
                    checkdata = Math.Abs(checkdata);
                }
                else { sc.isturn = true; }
                sc.id = 1;
                sc.checkdata = checkdata;
                sc.setdata = Convert.ToDecimal(GetTargetValue());
                decimal min = sc.setdata + (sc.setdata * allowmin / 100);
                decimal max = sc.setdata + (sc.setdata * allwmax / 100);
                sc.normalrang = min.ToString("f2") + "~" + max.ToString("f2");
                sc.errorrang = Convert.ToDecimal(((checkdata - sc.setdata) / sc.setdata).ToString("f4"));
                sc.normalmin = (allowmin / 100).ToString("f2");
                sc.normalmax = (allwmax / 100).ToString("f2");
                sc.error = (sc.errorrang * 100).ToString("f2") + "%";
                if (sc.checkdata <= max && sc.checkdata >= min)
                {
                    sc.result = "√";
                    _checkcount++;
                }
                else
                {
                    sc.result = "×";
                    _checkcount = 0;
                    _checkarr++;
                }
                return sc;
            }
            catch
            {
                return null;
            }
        }


        void AddToList(ShowCheckresult sc)
        {
            if (sc == null)
                return;
            sc.id = _listid;
            showcheckset.Add(sc);
            _listid++;
        }
        /// <summary>
        /// 单循环是否结束
        /// </summary>
        /// <returns></returns>
        bool isLastCheckData()
        {
            if (sysset != null && sysset.count != null)
            {
                if (_checkcount >= sysset.count)
                    return true;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 组数是否结束
        /// </summary>
        /// <returns></returns>
        bool isLastCheckArry()
        {
            if (sysset != null && sysset.arry != null)
            {
                if (_checkarr >= sysset.arry)
                    return true;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 校验是否完成
        /// </summary>
        /// <returns></returns>
        bool isLastData()
        {
            if (_checkindex >= _checktype - 1)
                return true;
            return false;

        }


    }
}
