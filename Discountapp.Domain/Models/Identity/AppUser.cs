using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Identity
{
    using System.Collections.Generic;
    using Application;
    using AppConfig = Discountapp.Infrastructure.Constants.Config;
    public class AppUser : IdentityUser<long, AppUserLogin, AppUserRole, AppUserClaim>
    {
        //[JsonIgnore]
        //public virtual ICollection<Merchant> Merchants { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<Address> Addresses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Company> Companies { get; set; }
        [JsonIgnore]
        public virtual ICollection<RealEstate> RealEstates { get; set; }
        [JsonIgnore]
        public virtual ICollection<Promotion>  Promotions { get; set; }
        [JsonIgnore]
        public virtual  ICollection<Like> Likes { get; set; }

        public async Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUserManager userManager)
        {
            var userIdentity = await userManager.CreateIdentityAsync
            (
                this,
                DefaultAuthenticationTypes.ApplicationCookie
            );

            return userIdentity;
        }

        public async Task<string> CreateUniqueTempFolderAsync(string rootFolderPath, string prefix = "")
        {
            var task = new Task<string>(() =>
            {
                DateTime now = DateTime.Now;
                string folderName = $"{prefix}{nameof(Id)}_{Id}_{now:yyyyMMdd}_{Guid.NewGuid():N}";
                DirectoryInfo resultInfro = Directory.CreateDirectory(Path.Combine(rootFolderPath, folderName));

                return resultInfro.FullName;
            });

            task.Start();

            return await task;
        }

        public async Task<int> DeleteTempFoldersAsync(string rootFolderPath, string prefix = "")
        {
            var task = new Task<int>(() =>
            {
                var directoriesToDelete = Directory.GetDirectories(rootFolderPath)
                 .Where(folderPath =>
                    {
                        if(!string.IsNullOrEmpty(folderPath))
                        {
                            string folderName = new DirectoryInfo(folderPath).Name;
                            return folderName.StartsWith($"{prefix}{nameof(Id)}_{Id}_");
                        }
                        return false;
                    })
                 .ToList();


                directoriesToDelete.ForEach(folderPath =>
                    Directory.Delete(folderPath, true)
                );


                return directoriesToDelete.Count;
            });

            task.Start();

            return await task;
        }

        public async Task<string> DeleteThenCreateTempFolderAsync(string rootFolderPath)
        {
            await this.DeleteTempFoldersAsync(rootFolderPath);

            return await this.CreateUniqueTempFolderAsync(rootFolderPath);
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            /* Usage
             string source = "Hello World!";
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);

                Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");

                Console.WriteLine("Verifying the hash...");

                if (VerifyMd5Hash(md5Hash, source, hash))
                {
                    Console.WriteLine("The hashes are the same.");
                }
                else
                {
                    Console.WriteLine("The hashes are not same.");
                }
            }
             */

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }

}
