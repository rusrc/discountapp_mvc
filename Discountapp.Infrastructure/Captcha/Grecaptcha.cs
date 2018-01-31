using System.Net;
using Discountapp.Infrastructure.Constants;
using Newtonsoft.Json;

namespace Discountapp.Infrastructure.Captcha
{
    using CONSTS = Config;
    public class Grecaptcha
    {
        #region Properties
        private string _secret;
        public string Secret
        {
            get
            {
                return string.IsNullOrEmpty(_secret) ? CONSTS.SecretKey : _secret;
            }
            set { _secret = value; }
        }

        private string _url;

        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                {
                    return CONSTS.GrecaptchaUrl;
                }
                return _url;
            }
            set { _url = value; }
        }

        public bool IsError { get; private set; }

        public string ErrorMsg { get; private set; }
        /// <summary>
        /// Объект с ответом
        /// </summary>
        public GrecaptchaResponse GResponse { get; set; }
        #endregion

        public Grecaptcha() { }

        /// <summary>
        /// Задайте ключ через конструктор
        /// </summary>
        /// <param name="secretKey">Секретный ключ полученный от гугла</param>
        public Grecaptcha(string secretKey)
        {
            this.Secret = secretKey;
        }

        /// <summary>
        /// Задайте ключ и url через конструктор
        /// </summary>
        /// <param name="secretKey">Секретный ключ полученный от гугла</param>
        /// <param name="url">Url запроса на проверку</param>
        public Grecaptcha(string secretKey, string url)
        {
            this.Secret = secretKey;
            this.Url = url;
        }

        /// <summary>
        /// Совершает запрос на проверку по ссылке https://www.google.com/recaptcha/api/siteverify
        /// </summary>
        /// <param name="grecaptchaResponse">Хеш-код гуглкапчи полученный от клиента</param>
        /// <returns>Grecaptcha</returns>
        public Grecaptcha Result(string grecaptchaResponse)
        {
            GrecaptchaResponse jsonResult = null;
            using (var client = new WebClient())
            {
                jsonResult = JsonConvert.DeserializeObject<GrecaptchaResponse>
                    (
                        value: client.DownloadString(string.Format(this.Url, this.Secret, grecaptchaResponse))
                    );
            }

            if (!jsonResult.Success && jsonResult.ErrorCodes.Count > 0)
            {
                this.ErrorMsg = this.GetError(jsonResult.ErrorCodes[0].ToLower());
                this.IsError = true;
            }
            else
            {
                this.GResponse = jsonResult;
                this.IsError = false;
            }

            return this;
        }

        #region Helper
        private string GetError(string errorName)
        {
            var msg = string.Empty;

            switch (errorName)
            {
                case ("missing-input-secret"):
                    msg = "";//ResxCabinet.gRecaptchaMissingInputSecret;
                    break;
                case ("invalid-input-secret"):
                    msg = ""; //ResxCabinet.gRecaptchaInvalidInputSecret;
                    break;
                case ("missing-input-response"):
                    msg = ""; //ResxCabinet.gRecaptchaMissingInputResponse;
                    break;
                case ("invalid-input-response"):
                    msg = ""; //ResxCabinet.gRecaptchaInvalidInputResponse;
                    break;
                default:
                    msg = ""; //ResxCabinet.gRecaptchaDefault;
                    break;
            }

            return msg;

        }
        #endregion
    }
}