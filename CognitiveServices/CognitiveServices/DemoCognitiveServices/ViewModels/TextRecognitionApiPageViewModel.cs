using DemoCognitiveServices.Settings;
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
    public class TextRecognitionApiPageViewModel : ViewModelBase
    {
        public ICommand TakeCommand { get; set; }
        public ICommand UploadCommand { get; set; }

        private ImageSource _source;

        public ImageSource SourceImage
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged(); }
        }

        private OcrResults _result;

        public OcrResults Result
        {
            get { return _result; }
            set { _result = value; RaisePropertyChanged(); }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged(); }
        }



        public TextRecognitionApiPageViewModel()
        {
            TakeCommand = new Command(Take);
            UploadCommand = new Command(Upload);
            this.visionClient = new VisionServiceClient(Keys.ComputerVisionKey);
        }

        private async void Upload(object obj)
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
                Text = BuildString(Result);
            }
            catch (Microsoft.ProjectOxford.Vision.ClientException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Error.Message, "OK");
                return;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
            Text = BuildString(Result);
            IsBusy = false;
        }

        private async Task<OcrResults> AnalyzePictureAsync(Stream inputFile)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await App.Current.MainPage.DisplayAlert("Network error", "Please check your network connection and retry.", "OK");
                return null;
            }

            OcrResults ocrResult = await App.ComputerServiceClient.RecognizeTextAsync(inputFile);
            return ocrResult;
        }

        private string BuildString(OcrResults ocrResult)
        {
            string result = "";
            foreach (var region in ocrResult.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        result = result + " " + word.Text;
                    }
                }
            }
            return result;
        }
    }
}
