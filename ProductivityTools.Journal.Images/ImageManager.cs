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

        private const string bucketPrefix= "ptjournal";
        private const string projectName = "PTJournalDev";

        public void UploadImageToStorage(Stream source, string userEmail, string fileName, string imageType)
        {
            string bucketName = string.Format($"{bucketPrefix}_{userEmail.Replace("@", "-").Replace(".","-")}");
            if(CheckIfBucketExists(bucketName)==false)
            {
                CreateRegionalBucket(projectName, bucketName, "europe-central2");
            }
            var x = this.StorageClient.UploadObject(bucketName, fileName, imageType, source);
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