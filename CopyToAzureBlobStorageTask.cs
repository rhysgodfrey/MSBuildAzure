using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;

namespace RhysG.MSBuild.Azure
{
    public class CopyToAzureBlobStorageTask : ITask
    {
        private IBuildEngine _buildEngine;
        private ITaskHost _taskHost;

        public IBuildEngine BuildEngine
        {
            get
            {
                return _buildEngine;
            }
            set
            {
                _buildEngine = value;
            }
        }

        [Required]
        public string ContainerName
        {
            get;
            set;
        }

        [Required]
        public string ConnectionString
        {
            get;
            set;
        }

        [Required]
        public string ContentType
        {
            get;
            set;
        }

        public string ContentEncoding
        {
            get;
            set;
        }

        [Required]
        public ITaskItem[] Files
        {
            get;
            set;
        }

        public bool Execute()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                // Provide the configSetter with the initial value
                configSetter(ConnectionString);
            });

            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("ConnectionSetting");

            CloudBlobClient client = account.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(ContainerName);
            container.CreateIfNotExist();
            container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Container });

            foreach (ITaskItem fileItem in Files)
            {
                FileInfo file = new FileInfo(fileItem.ItemSpec);

                CloudBlob blob = container.GetBlobReference(file.Name);

                try
                {
                    blob.FetchAttributes();
                }
                catch (StorageClientException) { }

                DateTime lastModified = DateTime.MinValue;

                if (!String.IsNullOrWhiteSpace(blob.Metadata["LastModified"]))
                {
                    long timeTicks = long.Parse(blob.Metadata["LastModified"]);
                    lastModified = new DateTime(timeTicks, DateTimeKind.Utc);
                }

                if (lastModified != file.LastWriteTimeUtc)
                {
                    blob.UploadFile(file.FullName);

                    blob.Properties.ContentType = ContentType;

                    if (!String.IsNullOrWhiteSpace(ContentEncoding))
                    {
                        blob.Properties.ContentEncoding = ContentEncoding;
                    }

                    blob.Metadata["LastModified"] = file.LastWriteTimeUtc.Ticks.ToString();
                    blob.SetMetadata();
                    blob.SetProperties();

                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs(String.Format("Updating: {0}", file.Name), String.Empty, "CopyToAzureBlobStorageTask", MessageImportance.Normal));
                }
            }

            return true;
        }

        public ITaskHost HostObject
        {
            get
            {
                return _taskHost;
            }
            set
            {
                _taskHost = value;
            }
        }
    }
}
