using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discountapp.Domain.Models
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    public abstract class BaseEntity : IIdentifiable
    {
        public virtual long Id { get; set; }
        public virtual DirectoryInfo GenerateFolder()
        {
            var userFolderPath = Path.Combine(Config.UploadFolderFullPath, $"{Guid.NewGuid():N}");
            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            return new DirectoryInfo(userFolderPath);     
        } 
    }
}
