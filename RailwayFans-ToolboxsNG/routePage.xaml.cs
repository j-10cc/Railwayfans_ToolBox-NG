using RailwayFans.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RailwayFans_ToolboxsNG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class routePage : Page
    {
        public routePage()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.ContentPanelCanvas.Children.Clear();
            int routeID=0,maxDistance=0, maxTime=0;
            using (var conn = DbContext.GetDbConnection())  //获取车次、交路编号、当前交路最长距离、交路用时、担当路局和车辆信息
            {
                var dbtrainList = conn.Table<trainList>();
                List<trainList> itemList = new List<trainList>();
                itemList = dbtrainList.Where(a => a.trainID == tbTrain.Text).ToList();
                foreach (trainList train in itemList)
                {
                    String trainID = train.trainID;
                    routeID = train.routeID;
                    maxDistance = train.maxDistance;
                    maxTime = train.maxTime;
                    string depot = train.depot;
                    string cabin = train.cabin;
                    int cocah = train.cocah;
                    string type = train.type;
                }

                var dbOrder = conn.Table<routeOrder>();
                List<routeOrder> order = new List<routeOrder>();
                order=dbOrder.Where(a => a.routeID == routeID).ToList();
                foreach (routeOrder or in order)
                {
                    int stationY;
                    stationY = (int)((ContentPanelCanvas.ActualHeight) * or.order / maxDistance);       //车站线=渲染高度*车站距交路位置/最长交路距离
                    Line station = new Line() { X1 = 80, X2 = ContentPanelCanvas.ActualWidth, Y1 = stationY, Y2 = stationY };   //定义车站线
                    for (int i=1;i<=maxTime;i++)
                    {
                        Line day = new Line() { X1 = ContentPanelCanvas.ActualWidth / i, X2 = ContentPanelCanvas.ActualWidth / i, Y1 = 0, Y2 = ContentPanelCanvas.ActualHeight};    //交路大于1天时定义天数分割线
                        day.Stroke=new SolidColorBrush(Colors.Black);
                        this.ContentPanelCanvas.Children.Add(day);
                    }
                    station.Stroke = new SolidColorBrush(Colors.Black);
                    this.ContentPanelCanvas.Children.Add(station);
                    TextBlock tbStation = new TextBlock()
                    {
                        Margin = new Thickness(10, stationY - 10, 0, 0)     //定义车站标签，位于车站线Y坐标下10像素处
                    };
                    tbStation.Text = or.station;
                    this.ContentPanelCanvas.Children.Add(tbStation);    //添加车站标签
                }

                var dbRoute = conn.Table<Route>();
                List<Route> route = new List<Route>();
                route = dbRoute.Where(a => a.routeID == routeID).OrderBy(b=>b.routeNo).ToList();
                maxTime = maxTime * 1440;       //交路最大时间换算为分钟数
                int flag = 2, lasts_Lo = 0;     //flag标识是否存在相邻交路同为上下行，lasts_Lo标识前一车次停车Y坐标位置
                TextBlock tbStartTime = new TextBlock(), tbEndTime = new TextBlock();
                foreach (Route rou in route)
                {
                    Line startLine = new Line(), endLine = new Line(), e_sLine = new Line(); //定义停车间隔线
                    int trainX1, trainX2, trainY1, trainY2;
                    trainX1 = (int)((ContentPanelCanvas.ActualWidth) * rou.startMinute / maxTime);      //车次发车X坐标=渲染宽度*发车时间分钟数/最大交路时间
                    trainX2 = (int)((ContentPanelCanvas.ActualWidth) * rou.endMinute / maxTime);        //车次停车X坐标=渲染宽度*停车时间分钟数/最大交路时间

                    if (rou.routeAsp < 0)       //routeAsp<0为背离北京方向，下行
                    {
                        trainY1 = (int)((ContentPanelCanvas.ActualHeight) * rou.location / maxDistance);    //车次发车Y坐标=渲染高度*始发位置(当前交路方向距同方向交路最远位置差)/最大交路距离
                        trainY2 = (int)((ContentPanelCanvas.ActualHeight) * (rou.distance + rou.location) / maxDistance); //车次停车Y坐标=渲染高度*(始发位置+里程)/最大交路距离

                        if (rou.routeNo == 1)           //当前交路为始发交路
                        {
                            endLine = new Line() { X1 = trainX2, X2 = trainX2, Y1 = trainY2, Y2 = trainY2 + 15 };       //不画发车间隔线，只画停车间隔线

                            tbStartTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX1 - 8, trainY1 - 20, 0, 0)    //发车时间标签，发车X坐标左6像素位置，发车Y坐标上20像素位置
                            };
                            tbStartTime.Text = rou.startTime;
                            this.ContentPanelCanvas.Children.Add(tbStartTime);

                            tbEndTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX2 - 40, trainY2, 0, 0)         //停车时间标签，停车X坐标左40像素位置，发车Y坐标位置
                            };
                            tbEndTime.Text = rou.endTime;
                            this.ContentPanelCanvas.Children.Add(tbEndTime);
                            if (rou.train.ToString().Length <= 2)
                            {
                                TextBlock tbID = new TextBlock()
                                {
                                    Margin = new Thickness((trainX1 + trainX2) / 2 - 23, (trainY1 + trainY2) / 2 - 6, 0, 0)
                                };
                                tbID.Text = rou.train;
                                this.ContentPanelCanvas.Children.Add(tbID);
                            }
                            else
                            {
                                TextBlock tbID = new TextBlock()
                                {
                                    Margin = new Thickness((trainX1 + trainX2) / 2 - 45, (trainY1 + trainY2) / 2 - 6, 0, 0)
                                };
                                tbID.Text = rou.train;
                                this.ContentPanelCanvas.Children.Add(tbID);
                            }
                            
                        }

                        else if (rou.routeNo==route.Count())
                        {
                            if (flag > 0)       //若不存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 - 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 - 15, Y2 = trainY1 - 15 };
                            }
                            else                //存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 + 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 + 15, Y2 = trainY1 + 15 };
                            }

                            tbStartTime = new TextBlock()
                            {
                                Margin = new Thickness(6 + trainX1, trainY1 - 20, 0, 0)     //发车时间标签，发车X坐标右6像素位置，发车Y坐标上20像素位置
                            };
                            tbStartTime.Text = rou.startTime;
                            this.ContentPanelCanvas.Children.Add(tbStartTime);

                            tbEndTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX2-8, trainY2, 0, 0)         //停车时间标签，停车X坐标左40像素位置，发车Y坐标位置
                            };
                            tbEndTime.Text = rou.endTime;
                            this.ContentPanelCanvas.Children.Add(tbEndTime);

                            TextBlock tbID = new TextBlock()
                            {
                                Margin = new Thickness((trainX1 + trainX2) / 2 + 6, (trainY1 + trainY2) / 2 - 6, 0, 0)
                            };
                            tbID.Text = rou.train;
                            this.ContentPanelCanvas.Children.Add(tbID);
                        }

                        else       //当不为始发交路时
                        {
                            endLine = new Line() { X1 = trainX2, X2 = trainX2, Y1 = trainY2, Y2 = trainY2 + 15 };   //停车间隔线右侧统一

                            if (flag > 0)       //若不存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 - 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 - 15, Y2 = trainY1 - 15 };
                            }
                            else                //存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 + 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 + 15, Y2 = trainY1 + 15 };
                            }

                            tbStartTime = new TextBlock()
                            {
                                Margin = new Thickness(6 + trainX1, trainY1 - 20, 0, 0)     //发车时间标签，发车X坐标右6像素位置，发车Y坐标上20像素位置
                            };
                            tbStartTime.Text = rou.startTime;
                            this.ContentPanelCanvas.Children.Add(tbStartTime);

                            tbEndTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX2 - 40, trainY2, 0, 0)         //停车时间标签，停车X坐标左40像素位置，发车Y坐标位置
                            };
                            tbEndTime.Text = rou.endTime;
                            this.ContentPanelCanvas.Children.Add(tbEndTime);

                            TextBlock tbID = new TextBlock()
                            {
                                Margin = new Thickness((trainX1 + trainX2) / 2 + 6, (trainY1 + trainY2) / 2 - 6, 0, 0)
                            };
                            tbID.Text = rou.train;
                            this.ContentPanelCanvas.Children.Add(tbID);
                        }
                        
                        lasts_Lo = trainX2;
                        flag = rou.routeAsp;
                    }
                    else            //routeAsp>0为朝向北京方向，上行
                    {
                        trainY1 = (int)((ContentPanelCanvas.ActualHeight) * (maxDistance - rou.location) / maxDistance);        //车次发车Y坐标=渲染高度*(最大交路距离-始发位置)/最大交路距离
                        trainY2 = (int)((ContentPanelCanvas.ActualHeight) * (maxDistance - rou.location - rou.distance) / maxDistance);     //车次停车Y坐标=渲染高度*(最大交路距离-始发位置)/最大交路距离

                        if (rou.routeNo == 1)           //当前交路为始发交路
                        {
                            endLine = new Line() { X1 = trainX2, X2 = trainX2, Y1 = trainY2, Y2 = trainY2 - 15 };       //不画发车间隔线，只画停车间隔线

                            tbStartTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX1 - 8, trainY1, 0, 0)      //发车时间标签，发车X坐标左6像素位置，发车Y坐标位置
                            };
                            tbStartTime.Text = rou.startTime;
                            this.ContentPanelCanvas.Children.Add(tbStartTime);

                            tbEndTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX2 - 40, trainY2 - 20, 0, 0)    //停车时间标签，停车X坐标左40像素位置，发车Y坐标上20像素位置
                            };
                            tbEndTime.Text = rou.endTime;
                            this.ContentPanelCanvas.Children.Add(tbEndTime);

                            if (rou.train.ToString().Length <= 2)
                            {
                                TextBlock tbID = new TextBlock()
                                {
                                    Margin = new Thickness((trainX1 + trainX2) / 2 - 23, (trainY1 + trainY2) / 2 - 6, 0, 0)
                                };
                                tbID.Text = rou.train;
                                this.ContentPanelCanvas.Children.Add(tbID);
                            }
                            else
                            {
                                TextBlock tbID = new TextBlock()
                                {
                                    Margin = new Thickness((trainX1 + trainX2) / 2 - 45, (trainY1 + trainY2) / 2 - 6, 0, 0)
                                };
                                tbID.Text = rou.train;
                                this.ContentPanelCanvas.Children.Add(tbID);
                            }
                        }

                        else if(rou.routeNo==route.Count())
                        {
                            if (flag < 0)       //若不存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 + 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 + 15, Y2 = trainY1 + 15 };
                            }
                            else                 //存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 - 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 - 15, Y2 = trainY1 - 15 };
                            }

                            tbStartTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX1 + 6, trainY1, 0, 0)       //发车时间标签，发车X坐标右6像素位置，发车Y坐标像素位置
                            };
                            tbStartTime.Text = rou.startTime;
                            this.ContentPanelCanvas.Children.Add(tbStartTime);

                            tbEndTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX2-8, trainY2 - 20, 0, 0)    //停车时间标签，停车X坐标左40像素位置，发车Y坐标上20像素位置
                            };
                            tbEndTime.Text = rou.endTime;
                            this.ContentPanelCanvas.Children.Add(tbEndTime);

                            TextBlock tbID = new TextBlock()
                            {
                                Margin = new Thickness((trainX1 + trainX2) / 2 + 6, (trainY1 + trainY2) / 2 - 6, 0, 0)
                            };
                            tbID.Text = rou.train;
                            this.ContentPanelCanvas.Children.Add(tbID);
                        }
                        else    //当不为始发交路时
                        {
                            endLine = new Line() { X1 = trainX2, X2 = trainX2, Y1 = trainY2, Y2 = trainY2 - 15 };       //停车间隔线右侧统一
                            if (flag < 0)       //若不存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 + 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 + 15, Y2 = trainY1 + 15 };
                            }

                            else                 //存在同方向连续交路
                            {
                                startLine = new Line() { X1 = trainX1, X2 = trainX1, Y1 = trainY1, Y2 = trainY1 - 15 };
                                e_sLine = new Line() { X1 = lasts_Lo, X2 = trainX1, Y1 = trainY1 - 15, Y2 = trainY1 - 15 };
                            }

                            tbStartTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX1 + 6, trainY1, 0, 0)       //发车时间标签，发车X坐标右6像素位置，发车Y坐标像素位置
                            };
                            tbStartTime.Text = rou.startTime;
                            this.ContentPanelCanvas.Children.Add(tbStartTime);

                            tbEndTime = new TextBlock()
                            {
                                Margin = new Thickness(trainX2 - 40, trainY2 - 20, 0, 0)    //停车时间标签，停车X坐标左40像素位置，发车Y坐标上20像素位置
                            };
                            tbEndTime.Text = rou.endTime;
                            this.ContentPanelCanvas.Children.Add(tbEndTime);

                            TextBlock tbID = new TextBlock()
                            {
                                Margin = new Thickness((trainX1 + trainX2) / 2 + 6, (trainY1 + trainY2) / 2 - 6, 0, 0)
                            };
                            tbID.Text = rou.train;
                            this.ContentPanelCanvas.Children.Add(tbID);
                        }

                        lasts_Lo = trainX2;
                        flag = rou.routeAsp;
                    }

                    Line train = new Line() { X1 = trainX1, X2 = trainX2, Y1 = trainY1, Y2 = trainY2 };
                    startLine.Stroke = new SolidColorBrush(Colors.Black);
                    endLine.Stroke = new SolidColorBrush(Colors.Black);
                    e_sLine.Stroke = new SolidColorBrush(Colors.Black);
                    train.Stroke = new SolidColorBrush(Colors.Black);
                    this.ContentPanelCanvas.Children.Add(train);
                    this.ContentPanelCanvas.Children.Add(startLine);
                    this.ContentPanelCanvas.Children.Add(endLine);
                    this.ContentPanelCanvas.Children.Add(e_sLine);
                    
                }
            }
        }
    }
}
