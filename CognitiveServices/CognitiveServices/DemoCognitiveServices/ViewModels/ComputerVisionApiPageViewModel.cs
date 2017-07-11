using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DemoCognitiveServices.ViewModels
{
    public class ComputerVisionApiPageViewModel : ViewModelBase
    {
        
        public ICommand TakeCommand { get; set; }
        public ICommand UploadCommand { get; set; }

        private ImageSource _source;

        public ImageSource SourceImage
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged(); }
        }

        private AnalysisResult _result;

        public AnalysisResult Result
        {
            get { return _result; }
            set { _result = value; RaisePropertyChanged(); }
        }


        public ComputerVisionApiPageViewModel()
        {
            TakeCommand = new Command(Take);
            UploadCommand = new Command(Upload);
        }

        private async void Upload()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No upload", "Picking a photo is not supported.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            IsBusy = true;
            SourceImage = ImageSource.FromStream(() => file.GetStream());

            try
            {
                Result = await AnalyzePictureAsync(file.GetStream());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Take()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Name = "test.jpg"
            });

            if (file == null)
                return;

            IsBusy = true;

            SourceImage = ImageSource.FromStream(() => file.GetStream());
            Result = await AnalyzePictureAsync(file.GetStream());
            IsBusy = false;
        }

        private async Task<AnalysisResult> AnalyzePictureAsync(Stream inputFile)
        {
            // Use the connectivity plugin to detect
            // if a network connection is available
            // Remember a using Plugin.Connectivit; directive
            if (!CrossConnectivity.Current.IsConnected)
            {
                await App.Current.MainPage.DisplayAlert("Network error",
                    "Please check your network connection and retry.", "OK");
                return null;
            }

            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult,
                VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description,
                VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };

            AnalysisResult analysisResult =
                await App.ComputerServiceClient.AnalyzeImageAsync(inputFile,
                visualFeatures);

            return analysisResult;
        }
    }
}
