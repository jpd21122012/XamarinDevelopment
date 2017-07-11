using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json.Linq;
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
    public class FaceApiPageViewModel : ViewModelBase
    {
        public ICommand TakeCommand { get; set; }
        public ICommand UploadCommand { get; set; }

        private ImageSource _source;

        public ImageSource SourceImage
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged(); }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged(); }
        }


        public FaceApiPageViewModel()
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
                AnalysisInDomainResult analysisResult = await AnalyzePictureAsync(file.GetStream());
                Text = (ParseCelebrityName(analysisResult.Result)) == "" ? "not matching" : ParseCelebrityName(analysisResult.Result);
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
            AnalysisInDomainResult analysisResult = await AnalyzePictureAsync(file.GetStream());
            Text = ParseCelebrityName(analysisResult.Result);
            IsBusy = false;
        }

        private async Task<Model> GetDomainModel()
        {
            ModelResult modelResult = await App.ComputerServiceClient.ListModelsAsync();
            // At this writing, only celebrity recognition
            // is available. It is the first model in the list
            return modelResult.Models.First();
        }

        private string ParseCelebrityName(object analysisResult)
        {
            JObject parsedJSONresult = JObject.Parse(analysisResult.ToString());

            var celebrities = from celebrity in parsedJSONresult["celebrities"]
                              select (string)celebrity["name"];

            return celebrities.FirstOrDefault();
        }

        private async Task<AnalysisInDomainResult> AnalyzePictureAsync(Stream inputFile)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await App.Current.MainPage.DisplayAlert("Network error", "Please check your network connection and retry.", "OK");
                return null;
            }

            AnalysisInDomainResult analysisResult =
                await App.ComputerServiceClient.AnalyzeImageInDomainAsync(inputFile, await GetDomainModel());

            return analysisResult;
        }
    }
}
