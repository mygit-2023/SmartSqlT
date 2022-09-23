﻿using SmartSQL.Annotations;
using SmartSQL.Framework;
using SmartSQL.Framework.SqliteModel;
using SmartSQL.Helper;
using SmartSQL.Views.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartSQL.UserControl.Tags
{
    /// <summary>
    /// TagObjects.xaml 的交互逻辑
    /// </summary>
    public partial class TagObjects : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region PropertyFiled
        public static readonly DependencyProperty ConnectionProperty = DependencyProperty.Register(
            "Connection", typeof(ConnectConfigs), typeof(TagObjects), new PropertyMetadata(default(ConnectConfigs)));
        /// <summary>
        /// 当前选中连接
        /// </summary>
        public ConnectConfigs SelectedConnection
        {
            get => (ConnectConfigs)GetValue(ConnectionProperty);
            set => SetValue(ConnectionProperty, value);
        }

        public static readonly DependencyProperty SelectedDataBaseProperty = DependencyProperty.Register(
            "SelectedDataBase", typeof(string), typeof(TagObjects), new PropertyMetadata(default(string)));
        /// <summary>
        /// 当前选中数据库
        /// </summary>
        public string SelectedDataBase
        {
            get => (string)GetValue(SelectedDataBaseProperty);
            set => SetValue(SelectedDataBaseProperty, value);
        }

        public static readonly DependencyProperty SelectedTagProperty = DependencyProperty.Register(
            "SelectedTag", typeof(TagInfo), typeof(TagObjects), new PropertyMetadata(default(TagInfo)));
        /// <summary>
        /// 当前选中标签
        /// </summary>
        public TagInfo SelectedTag
        {
            get => (TagInfo)GetValue(SelectedTagProperty);
            set => SetValue(SelectedTagProperty, value);
        }

        /// <summary>
        /// 标签对象数据列表
        /// </summary>
        public static readonly DependencyProperty TagObjectListProperty = DependencyProperty.Register(
            "TagObjectList", typeof(List<Framework.SqliteModel.TagObjects>), typeof(TagObjects), new PropertyMetadata(default(List<Framework.SqliteModel.TagObjects>)));
        public List<Framework.SqliteModel.TagObjects> TagObjectList
        {
            get => (List<Framework.SqliteModel.TagObjects>)GetValue(TagObjectListProperty);
            set
            {
                SetValue(TagObjectListProperty, value);
                OnPropertyChanged(nameof(TagObjectList));
            }
        } 
        #endregion

        public TagObjects()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// 初始化加载数据
        /// </summary>
        public void LoadPageData()
        {
            var conn = SelectedConnection;
            var selDatabase = SelectedDataBase;
            var selTag = SelectedTag;
            var sqLiteInstance = SQLiteHelper.GetInstance();
            var tagObjectList = sqLiteInstance.ToList<Framework.SqliteModel.TagObjects>(x =>
                x.ConnectId == conn.ID &&
                x.DatabaseName == selDatabase &&
                x.TagId == selTag.TagId);
            if (tagObjectList.Any())
            {
                MainNoDataText.Visibility = Visibility.Collapsed;
            }
            TagObjectList = tagObjectList;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        /// <summary>
        /// 设置标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetTag_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTag == null)
            {
                Oops.Oh("请选择标签.");
                return;
            }
            var parentWindow = (TagsView)Window.GetWindow(this);
            var ucAddObjects= new AddObjects();
            ucAddObjects.SelectedConnection = SelectedConnection;
            ucAddObjects.SelectedDataBase = SelectedDataBase;
            ucAddObjects.SelectedTag = SelectedTag;
            ucAddObjects.LoadPageData();
            parentWindow.MainContent = ucAddObjects;
        }
    }
}
