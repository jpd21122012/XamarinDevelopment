using DemoCognitiveServices.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DemoCognitiveServices.ViewModels
{
    public class EmotionApiPageViewModel : ViewModelBase
    {
        
        public ICommand TakeCommand { get; set; }
        public ICommand UploadCommand { get; set; }

        private ImageSource _source;

        public ImageSource SourceImage
        {
            get { return _source; }
            set { _source = value; RaisePropertyChanged(); }
        }


        private FaceEmotionDetection _emotionDectection;

        public FaceEmotionDetection EmotionDetection
        {
            get { return _emotionDectection; }
            set { _emotionDectection = value; RaisePropertyChanged(); }
        }



        public EmotionApiPageViewModel()
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
                EmotionDetection = await DetectFaceAndEmotionsAsync(file);
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
            EmotionDetection = await DetectFaceAndEmotionsAsync(file);
            IsBusy = false;
        }

        private async Task<FaceEmotionDetection> DetectFaceAndEmotionsAsync(MediaFile inputFile)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await App.Current.MainPage.DisplayAlert("Network error", "Please check your network connection and retry.", "OK");
                return null;
            }

            try
            {
                Emotion[] emotionResult = await App.EmotionServiceClient.
                    RecognizeAsync(inputFile.GetStream());

                var faceEmotion = emotionResult[0]?.Scores.ToRankedList();

                var requiredFaceAttributes = new FaceAttributeType[] {
                FaceAttributeType.Age,
                FaceAttributeType.Gender,
                FaceAttributeType.Smile,
                FaceAttributeType.FacialHair,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Glasses
                };

                // Get a list of faces in a picture
                var faces = await App.FaceServiceClient.
                    DetectAsync(inputFile.GetStream(),
                                false, false, requiredFaceAttributes);

                // Assuming there is only one face, store its attributes
                var faceAttributes = faces[0]?.FaceAttributes;

                if (faceEmotion == null || faceAttributes == null) return null;

                FaceEmotionDetection faceEmotionDetection = new FaceEmotionDetection();
                faceEmotionDetection.Age = faceAttributes.Age;
                faceEmotionDetection.Emotion = faceEmotion.FirstOrDefault().Key;
                faceEmotionDetection.Glasses = faceAttributes.Glasses.ToString();
                faceEmotionDetection.Smile = faceAttributes.Smile;
                faceEmotionDetection.Gender = faceAttributes.Gender;
                faceEmotionDetection.Moustache = faceAttributes.FacialHair.Moustache;
                faceEmotionDetection.Beard = faceAttributes.FacialHair.Beard;

                return faceEmotionDetection;
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }
    }
}
