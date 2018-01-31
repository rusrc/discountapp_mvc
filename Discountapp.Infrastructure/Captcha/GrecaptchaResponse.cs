using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discountapp.Infrastructure.Captcha
{
    public class GrecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }



    }
}