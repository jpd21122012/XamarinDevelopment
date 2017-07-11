﻿using DemoCognitiveServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoCognitiveServices.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            Master = masterPage;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ComputerVisionApiPage)));
            masterPage.ListView.ItemSelected += OnItemSelected;
            MasterBehavior = MasterBehavior.Popover;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.Target));
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
