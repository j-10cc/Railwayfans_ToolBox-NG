using RailwayFans.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RailwayFans_ToolboxsNG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        //车型查询
        private void btnSearchType_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = DbContext.GetDbConnection())
            {
                StringBuilder msg = new StringBuilder();
                var dbEMU = conn.Table<EMU>();
                List<EMU> itemList = new List<EMU>();
                itemList = dbEMU.Where(a => a.Type == txtType.Text.ToUpper()).ToList();
                listType.ItemsSource = itemList;
                listType.SelectedItem = null;
            }
        }

        //车型列表
        private async void listType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listType.SelectedItem != null)
            {
                if (((EMU)listType.SelectedItem).Info != null)
                {
                    var dialog = new MessageDialog(((EMU)listType.SelectedItem).Info, ResourceLoader.GetForCurrentView("Resources").GetString("attachInf"));
                    dialog.Commands.Add(new UICommand(ResourceLoader.GetForCurrentView("Resources").GetString("aboutBtn"), cmd => { }, commandId: 0));
                    await dialog.ShowAsync();

                }
            }
        }

        //车号查询
        private void btnSearchNum_Click(object sender, RoutedEventArgs e)
        {
            textInfo.Text = "";
            List8.Visibility = Visibility.Collapsed; //8编组列表隐藏
            List16.Visibility = Visibility.Collapsed;//16编组列表隐藏
            using (var conn = DbContext.GetDbConnection())
            {
                int i = 1;
                int j = 1;
                string listID = "";
                StringBuilder msg = new StringBuilder();
                var dbEMU = conn.Table<EMU>();
                List<EMU> itemList = new List<EMU>();
                itemList = dbEMU.Where(a => a.ID == txtNum.Text).ToList();
                foreach (EMU emux in itemList)
                {

                    String Model = emux.Type;
                    String image = Model;
                    String Num = emux.ID;
                    String Agency = emux.Agency;
                    String Dep = emux.Dep;
                    String Factory = emux.Factory;
                    String Info = emux.Info;
                    listID = emux.listID;
                    if (Model == ResourceLoader.GetForCurrentView("Resources").GetString("sEMU"))
                    {
                        if (Num.StartsWith("02"))//判断标准动车组-四方系列
                            image = "CRH02";
                        else if (Num.StartsWith("05"))//判断标准动车组-长客系列
                            image = "CRH05";
                        Model = "CRH";
                    }
                    ModelImage.ImageSource = new BitmapImage(new System.Uri("ms-appx:///Assets/Models/" + image + ".jpg", System.UriKind.RelativeOrAbsolute));//获取动车组型号图片
                    textType.Text = Model + "-" + Num;
                    textAgency.Text = ResourceLoader.GetForCurrentView("Resources").GetString("AgencyLabel") + Agency;
                    textDep.Text = ResourceLoader.GetForCurrentView("Resources").GetString("DepLabel") + Dep;
                    textFactory.Text = ResourceLoader.GetForCurrentView("Resources").GetString("listFactoryMobile") + Factory;
                    if (Info != null)
                    {
                        textInfo.Text = Info;
                    }
                    i++;
                }
                List<listEMU> coList = new List<listEMU>();
                var dblistEMU = conn.Table<listEMU>();
                coList = dblistEMU.Where(a => a.listID == listID).ToList();
                foreach (listEMU listE in coList)
                {
                    if (listE.coP16 != null)
                    {
                        co16P1.Text = listE.coP1;
                        co16P2.Text = listE.coP2;
                        co16P3.Text = listE.coP3;
                        co16P4.Text = listE.coP4;
                        co16P5.Text = listE.coP5;
                        co16P6.Text = listE.coP6;
                        co16P7.Text = listE.coP7;
                        co16P8.Text = listE.coP8;
                        co16P9.Text = listE.coP9;
                        co16P10.Text = listE.coP10;
                        co16P11.Text = listE.coP11;
                        co16P12.Text = listE.coP12;
                        co16P13.Text = listE.coP13;
                        co16P14.Text = listE.coP14;
                        co16P15.Text = listE.coP15;
                        co16P16.Text = listE.coP16;
                        co16T1.Text = listE.coT1;
                        co16T2.Text = listE.coT2;
                        co16T3.Text = listE.coT3;
                        co16T4.Text = listE.coT4;
                        co16T5.Text = listE.coT5;
                        co16T6.Text = listE.coT6;
                        co16T7.Text = listE.coT7;
                        co16T8.Text = listE.coT8;
                        co16T9.Text = listE.coT9;
                        co16T10.Text = listE.coT10;
                        co16T11.Text = listE.coT11;
                        co16T12.Text = listE.coT12;
                        co16T13.Text = listE.coT13;
                        co16T14.Text = listE.coT14;
                        co16T15.Text = listE.coT15;
                        co16T16.Text = listE.coT16;
                        coaddInf.Text = listE.addInf;
                        List16.Visibility = Visibility;
                    }
                    else
                    {
                        coP1.Text = listE.coP1;
                        coP2.Text = listE.coP2;
                        coP3.Text = listE.coP3;
                        coP4.Text = listE.coP4;
                        coP5.Text = listE.coP5;
                        coP6.Text = listE.coP6;
                        coP7.Text = listE.coP7;
                        coP8.Text = listE.coP8;
                        coT1.Text = listE.coT1;
                        coT2.Text = listE.coT2;
                        coT3.Text = listE.coT3;
                        coT4.Text = listE.coT4;
                        coT5.Text = listE.coT5;
                        coT6.Text = listE.coT6;
                        coT7.Text = listE.coT7;
                        coT8.Text = listE.coT8;
                        coaddInf8.Text = listE.addInf;
                        List8.Visibility = Visibility;
                    }
                    j++;
                }
                Image.Visibility = Visibility;
                mm1435.Visibility = Visibility;//显示版权信息
            }
        }

        //配属路局列表
        private async void listAgency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listAgency.SelectedItem != null)
            {
                if (((EMU)listAgency.SelectedItem).Info != null)
                {
                    var dialog = new MessageDialog(((EMU)listAgency.SelectedItem).Info, ResourceLoader.GetForCurrentView("Resources").GetString("attachInf"));
                    dialog.Commands.Add(new UICommand(ResourceLoader.GetForCurrentView("Resources").GetString("aboutBtn"), cmd => { }, commandId: 0));
                    await dialog.ShowAsync();
                }
            }
        }

        //配属路局查询
        private void btnSearchAgency_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = DbContext.GetDbConnection())
            {
                StringBuilder msg = new StringBuilder();
                var dbEMU = conn.Table<EMU>();
                List<EMU> itemList = new List<EMU>();
                String agency = (comboAgency.SelectedItem as ComboBoxItem).Content.ToString();
                itemList = dbEMU.Where(a => a.Agency == agency).ToList();
                listAgency.ItemsSource = itemList;
                listAgency.SelectedItem = null;
            }
        }

        //获取当前路局所属动车组
        private void combo_Agency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var conn = DbContext.GetDbConnection())
            {
                StringBuilder msg = new StringBuilder();
                var dbDept = conn.Table<Dept>();
                List<Dept> itemList = new List<Dept>();
                String agency = (combo_Agency.SelectedItem as ComboBoxItem).Content.ToString();
                itemList = dbDept.Where(a => a.Agency == agency).ToList();
                comboDep.ItemsSource = itemList;
            }
            if (comboDep.Items.Count > 0)
            {
                comboDep.SelectedIndex = 0;
                listDep.SelectedIndex = -1;
            }
        }

        //配属动车组查询
        private async void listDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listDep.SelectedItem != null)
            {
                if (((EMU)listDep.SelectedItem).Info != null)
                {
                    var dialog = new MessageDialog(((EMU)listDep.SelectedItem).Info, ResourceLoader.GetForCurrentView("Resources").GetString("attachInf"));
                    dialog.Commands.Add(new UICommand(ResourceLoader.GetForCurrentView("Resources").GetString("aboutBtn"), cmd => { }, commandId: 0));
                    await dialog.ShowAsync();
                }
            }
        }
    }
}
