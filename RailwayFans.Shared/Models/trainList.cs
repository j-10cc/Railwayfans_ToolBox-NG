using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayFans.Models
{
    //列车列表数据
    public class trainList
    {
        public string trainID { get; set; }     //车次
        public int routeID { get; set; }        //交路编号
        public int maxDistance { get; set; }    //当前交路最远距离
        public int maxTime { get; set; }        //当前交路用时
        public string depot { get; set; }       //担当车辆段
        public string cabin { get; set; }       //担当乘务段
        public int cocah { get; set; }          //头车车厢
        public string type { get; set; }        //车辆类型 
    }
}