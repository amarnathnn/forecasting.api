using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PS.CTS.Common.Utility
{
    public class FileStorageHandler
    {
        public void upload_ToBlob(string fileToUpload, string azure_ContainerName)
        { 
            string file_extension,
            filename_withExtension,
            storageAccount_connectionString;
            Stream file;

            //Copy the storage account connection string from Azure portal     
            storageAccount_connectionString = "DefaultEndpointsProtocol=https;AccountName=pramaticsolutionsstorage;AccountKey=pPgpqGxd2uSMnbVIlicHOqUIA7rSkhGERwgmBbxT3mB93fIHZbFJKBlYDJ4q70Kv41FIqjeXD//hVey303HmwQ==;EndpointSuffix=core.windows.net";

            // << reading the file as filestream from local machine >>    
            file = new FileStream(fileToUpload, FileMode.Open);

            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(storageAccount_connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(azure_ContainerName);

            //checking the container exists or not  
            if (container.CreateIfNotExistsAsync().Result)
            {

                container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess =
                  BlobContainerPublicAccessType.Blob
                });

            }

            //reading file name & file extention    
            file_extension = Path.GetExtension(fileToUpload);
            filename_withExtension = Path.GetFileName(fileToUpload);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename_withExtension);
            cloudBlockBlob.Properties.ContentType = file_extension;

            cloudBlockBlob.UploadFromStreamAsync(file); // << Uploading the file to the blob >>  
        }

        public void download_FromBlob(string filetoDownload, string azure_ContainerName)
        {
            Console.WriteLine("Inside downloadfromBlob()");

            string storageAccount_connectionString = "DefaultEndpointsProtocol=https;AccountName=pramaticsolutionsstorage;AccountKey=pPgpqGxd2uSMnbVIlicHOqUIA7rSkhGERwgmBbxT3mB93fIHZbFJKBlYDJ4q70Kv41FIqjeXD//hVey303HmwQ==;EndpointSuffix=core.windows.net";

            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(storageAccount_connectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(azure_ContainerName);
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filetoDownload);

            // provide the file download location below            
            Stream file = File.OpenWrite(@"C:\CaliberHackathon2020_PS\BlobDownload\" + filetoDownload);


            //cloudBlockBlob.DownloadToStream(file);

            Console.WriteLine("Download completed!");

        }
    }
}
