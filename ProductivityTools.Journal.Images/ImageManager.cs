using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace ProductivityTools.Journal.Images
{
    public class ImageManager
    {
        StorageClient StorageClient { get; set; }

        public ImageManager()
        {
            this.StorageClient = StorageClient.Create();
        }

        private const string bucketPrefix = "ptjournal";
        private const string projectName = "PTJournalDev";

        //public string UploadImageToStorage(Stream source, string userEmail, string fileName, string imageType)
        //{
        //    string bucketName = string.Format($"{bucketPrefix}_{userEmail.Replace("@", "-").Replace(".", "-")}");
        //    if (CheckIfBucketExists(bucketName) == false)
        //    {
        //        CreateRegionalBucket(projectName, bucketName, "europe-central2");
        //    }
        //    int randomNumber = new Random().Next(100);
        //    string fName = Path.GetFileNameWithoutExtension(fileName);
        //    string fExt = Path.GetExtension(fileName);
        //    var filenameRandomized = String.Concat(fName, "-", randomNumber.ToString().PadLeft(3, '0'), fExt);
        //    while (filenameRandomized.Contains(" "))
        //    {
        //        filenameRandomized = filenameRandomized.Replace(" ", "");
        //    }
        //    var x = this.StorageClient.UploadObject(bucketName, filenameRandomized, imageType, source);
        //    return string.Format($"https://storage.cloud.google.com/{bucketName}/{filenameRandomized}");
        //}

        public string UploadImageToStorage(Stream source, string userId, string fileName, string imageType)
        {
            string bucketName = "ptjournal-b53b0.appspot.com";
            //if (CheckIfBucketExists(bucketName) == false)
            //{
            //    CreateRegionalBucket(projectName, bucketName, "europe-central2");
            //}
            int randomNumber = new Random().Next(100);
            string fName = Path.GetFileNameWithoutExtension(fileName);
            string fExt = Path.GetExtension(fileName);
            var filenameRandomized = String.Concat("user/",userId,"/",fName, "-", randomNumber.ToString().PadLeft(3, '0'), fExt);
            while (filenameRandomized.Contains(" "))
            {
                filenameRandomized = filenameRandomized.Replace(" ", "");
            }
            var x = this.StorageClient.UploadObject(bucketName, filenameRandomized, imageType, source);
            return string.Format($"https://storage.cloud.google.com/{bucketName}/{filenameRandomized}");
        }

        private bool CheckIfBucketExists(string bucketName)
        {
            try
            {
                var bucket = this.StorageClient.GetBucket(bucketName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public Bucket CreateRegionalBucket(string projectId, string bucketName, string location)
        {
            try
            {
                var x = CreateRegionalBucketInternal(projectId, bucketName, location);
                return x;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public Bucket CreateRegionalBucketInternal(string projectId, string bucketName, string location, string storageClass = "REGIONAL")
        {
            var storage = StorageClient.Create();

            Bucket bucket = new Bucket
            {
                Location = location,
                Name = bucketName,
                StorageClass = storageClass
            };
            var newlyCreatedBucket = storage.CreateBucket(projectId, bucket);
            Console.WriteLine($"Created {bucketName}.");
            return newlyCreatedBucket;
        }
    }
}