using Google.Cloud.Storage.V1;

namespace ProductivityTools.Journal.Images
{
    public class ImageManager
    {
        public void UploadImageToStorage(Stream source)
        {
            var gcsStorage = StorageClient.Create();
            gcsStorage.UploadObject("ptjrlclap", "Fdsafa", "image", source);
        }

    }
}