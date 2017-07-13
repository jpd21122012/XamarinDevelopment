using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace StorageDroid.Droid
{
	[Activity (Label = "StorageDroid.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        public Stream streamCopy;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

            button.Click += Button_Click;
		}
        private async Task SetImageToControlAsync(MediaFile file)
        {
            try
            {
                if (file == null)
                { return; }
                var stream = file.GetStream();
                streamCopy = new MemoryStream();
                stream.CopyTo(streamCopy);
                stream.Seek(0, SeekOrigin.Begin);
                file.Dispose();
                if (file != null || streamCopy.Length != 0)
                {

                    StorageDroid.StorageService storageSvc = new StorageService();
                    storageSvc.mess(stream.Length.ToString(), streamCopy.Length.ToString());
                }
            }
            catch (Exception)
            {

            }
            await performBlobOperation("slipknot_jpd@hotmail.com");
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            //StorageDroid.StorageService storageSvc = new StorageService();
            //await storageSvc.performBlobOperation("slipknot_jpd@hotmail.com");  
            MediaFile file = null;
            try {
                file = await TakePicture(true);
            } catch (Android.OS.OperationCanceledException) {

                }
            await SetImageToControlAsync(file);
        }
        public async Task performBlobOperation(string participante)
        {
            try
            {
                string blobSAS = "https://torneojpd.blob.core.windows.net/enigmajpd/" + participante + ".jpg?sv=2016-05-31&ss=bqtf&srt=sco&sp=rwdlacup&se=2017-05-23T10:17:48Z&sig=UpCI2DWo5%2BFDBH%2BfreqjXavvr%2Fj8elkOxZ1OMYIozkw%3d";

                //string blobSAS = "https://torneoxamarin.blob.core.windows.net/devs/" + participante + ".jpg?sv=2016-05-31&ss=b&srt=sco&sp=wlac&se=2017-07-01T15:00:00Z&st=2017-05-22T18:00:00Z&sip=0.0.0.0-255.255.255.255&spr=https,http&sig=UDaqkQEJrd8E%2FCPhLwWmrM3ZZobdTlTqQadVIy67pXc%3D";
                CloudBlockBlob blob = new CloudBlockBlob(new Uri(blobSAS));

                 await blob.UploadFromStreamAsync(streamCopy);
            }
            catch (Exception exc)
            {
                string msgError = exc.Message;
            }
        }
        public static async Task<MediaFile> TakePicture(bool useCam = true)
        {
            await CrossMedia.Current.Initialize();

            if (useCam) { if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) { return null; } }

            var file = useCam ? await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { Directory = "Championship", Name = "Reto6_Test.jpg" }) :await  CrossMedia.Current.PickPhotoAsync();

            return file;
        }
    }
}


