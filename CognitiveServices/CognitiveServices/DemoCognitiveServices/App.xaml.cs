using DemoCognitiveServices.Settings;
using DemoCognitiveServices.Views;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DemoCognitiveServices
{
    public partial class App : Application
    {
        public static readonly VisionServiceClient ComputerServiceClient = new VisionServiceClient(Keys.ComputerVisionKey);
        public static readonly IFaceServiceClient FaceServiceClient =  new FaceServiceClient(Keys.FaceApiKey);
        public static readonly EmotionServiceClient EmotionServiceClient = new EmotionServiceClient(Keys.EmotionApiKey);
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
