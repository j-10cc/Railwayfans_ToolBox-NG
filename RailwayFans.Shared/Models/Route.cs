using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayFans.Models
{
    public class Route
    {
        public int routeID { get; set; }        //交路编号
        public int routeNo { get; set; }        //次序
        public int routeAsp { get; set; }
        public string train { get; set; }       //车次
        public string startTime { get; set; }   //发时
        public int startMinute { get; set; }    //发时分钟数
        public string endTime { get; set; }     //到时
        public int endMinute { get; set; }      //到时分钟数
        public int distance { get; set; }       //距离
        public int location { get; set; }       //始发位置
    }
}
