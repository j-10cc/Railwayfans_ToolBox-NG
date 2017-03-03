using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayFans.Models
{
    //动车组数据库Model
    public class EMU
    {
        public string ID { get; set; }      //列车ID
        public string Type { get; set; }    //列车类型
        public string Agency { get; set;}   //配属路局
        public string Dep { get; set; }     //配属动车所
        public string Factory { get; set; } //生产厂家
        public string Info { get; set; }    //车辆附加信息
        public string listID { get; set; }  //编组类型
    }
}
