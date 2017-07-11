using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemoCognitiveServices.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        protected Page _currentPage;

        public ViewModelBase()
        {

        }

        public ViewModelBase(Page page)
        {
            _currentPage = page;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }



        public const bool IsDebug = true;


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NavigateTo(Page pageView)
        {
            this._currentPage.Navigation.PushAsync(pageView);
        }

       

        public void NavigateToPageInit(Page pageView)
        {
            Application.Current.MainPage = new NavigationPage(pageView)
            {
                BarTextColor = Color.White,
                BarBackgroundColor = (Color)App.Current.Resources["ColorAqua"],// Color.White,
            };
        }

        public void NavigateToPageCurrent(Page pageView)
        {
            Application.Current.MainPage = pageView;
        }

        public void NavigateGoBack()
        {
            this._currentPage.Navigation.PopAsync();
        }


    }
}
