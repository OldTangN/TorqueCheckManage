using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Visifire.Charts;

namespace LT.BLL.MyChart
{
   public  class ColumnSeries
    {
       List<string> _valuex= new List<string>();
       List<string> _valuey = new List<string>();
       public ColumnSeries(List <string >valuex,List <string >valuey) {
           this._valuex = valuex;
           this._valuey = valuey;
       }
       public Grid  CreateChar() 
       {
           try
           {
               Chart chart = new Chart();
               chart.Width = 700;
               chart.Height = 400;
               chart.Margin = new System.Windows.Thickness(10, 5, 10, 5);
               chart.ScrollingEnabled = false;
               chart.ToolBarEnabled = false;
               chart.View3D = true;
               Title title = new Title();
               title.Padding = new System.Windows.Thickness(0, 10, 5, 0);
               chart.Titles.Add(title);
               Axis yAxis = new Axis();
               yAxis.AxisMinimum = 1;
               yAxis.Suffix = "次";
               chart.AxesY.Add(yAxis);
               DataSeries dataSeries = new DataSeries();
               dataSeries.RenderAs = RenderAs.StackedColumn;
               DataPoint dataPoint;
               for (int i = 0; i < _valuex.Count; i++)
               {
                   dataPoint = new DataPoint();
                   dataPoint.AxisXLabel = _valuex[i];
                   dataPoint.YValue = double.Parse(_valuey[i]);
                   dataSeries.DataPoints.Add(dataPoint);
               }
               chart.Series.Add(dataSeries);
               Grid gr = new Grid();
               gr.Children.Add(chart);
               return gr;
           }
           catch { return null; }
       }
    }
   
}
