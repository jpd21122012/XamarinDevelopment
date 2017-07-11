using DemoCognitiveServices.Models;
using DemoCognitiveServices.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoCognitiveServices.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return navigationDrawerList; } }
        public MasterPage()
        {
            InitializeComponent();
            BindingContext = new MasterPageViewModel();
        }
    }
}
