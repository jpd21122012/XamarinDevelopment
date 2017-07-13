using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.IO;
using System.IO;
using System.Text;
using System.Diagnostics;
using Android.Graphics;
using Java.IO;

namespace StorageDroid
{
    public class StorageService
    {
        public StorageService()
        {
        }
        public Stream stream;
        public  async Task performBlobOperation(string participante)
        {
            try
            {
                string blobSAS = "https://torneojpd.blob.core.windows.net/enigmajpd/" + participante + ".jpg?sv=2016-05-31&ss=bqtf&srt=sco&sp=rwdlacup&se=2017-05-23T10:17:48Z&sig=UpCI2DWo5%2BFDBH%2BfreqjXavvr%2Fj8elkOxZ1OMYIozkw%3d";

                //string blobSAS = "https://torneoxamarin.blob.core.windows.net/devs/" + participante + ".jpg?sv=2016-05-31&ss=b&srt=sco&sp=wlac&se=2017-07-01T15:00:00Z&st=2017-05-22T18:00:00Z&sip=0.0.0.0-255.255.255.255&spr=https,http&sig=UDaqkQEJrd8E%2FCPhLwWmrM3ZZobdTlTqQadVIy67pXc%3D";
                CloudBlockBlob blob = new CloudBlockBlob(new Uri(blobSAS));

                //string blobContent = "Jorge Perales Diaz - Mexico";
                //MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                //msWrite.Position = 0;
                //using (msWrite)
                //{
                //   await blob.UploadFromStreamAsync(msWrite);
                //}
                   await blob.UploadFromStreamAsync(stream);
                Debug.WriteLine("Se subio la img");
            }
            catch (Exception exc)
            {
                string msgError = exc.Message;
                Debug.WriteLine(msgError);
            }
        }
        public void mess(string a, string b )
        {
            Debug.WriteLine(a+" "+b);
        }

        public static async Task performBlobOperation()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=torneojpd;AccountKey=jKE2APiqZgJH29+yqE30QOtfThQAZ3p9JzlSgOxluxzaJmoM4a3EFXqKdSfxVPaxnOI0gc0JRJpuQJYWheVDIg==;EndpointSuffix=core.windows.nets");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            // Create the "myblob" blob with the text "Hello, world!"
            await blockBlob.UploadTextAsync("Hello, world!");
        }

    }
}

