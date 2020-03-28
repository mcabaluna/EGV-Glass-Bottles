using Merchandising.VM.Portal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Merchandising.Portal.Models
{
    public class MerchandisingApiWrapper
    {

        private static JsonSerializerSettings JsonSettings()
        {

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Delete<T>(string url)
        {
            string token = string.Empty;
            if (HttpContext.Current.Session["Token"] != null)
                token = (string)HttpContext.Current.Session["Token"];

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + "portal/api/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Add("Language", HttpContext.Current.Session["SelectedLanguage"]?.ToString() ?? "en");
            client.DefaultRequestHeaders.Referrer = HttpContext.Current.Request.UrlReferrer;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);

            client.Timeout = TimeSpan.FromHours(2);
            var result = client.SendAsync(request).Result;

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new HttpException(error.HttpStatus, msg);
                }
                throw new HttpException((int)result.StatusCode, result.ReasonPhrase);
            }
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static T Put<T>(string url, object parameter, string contentType = "application/json")
        {
            //string token = GetToken().access_token;
            //string schema = "Basic";

            //if (HttpContext.Current.Session["Token"] != null)
            //{
            //    token = (string)HttpContext.Current.Session["Token"];
            //    schema = "Bearer";
            //}

            string p = contentType == "application/json" ? JsonConvert.SerializeObject(parameter, JsonSettings()) : parameter.ToString();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + "portal/api/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(schema, token);
            //client.DefaultRequestHeaders.Add("Language", HttpContext.Current.Session["SelectedLanguage"]?.ToString() ?? "en");
            client.DefaultRequestHeaders.Referrer = HttpContext.Current.Request.UrlReferrer;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(p, Encoding.UTF8, contentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = client.SendAsync(request).Result;

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new HttpException(error.HttpStatus, msg);
                }
                throw new HttpException((int)result.StatusCode, result.ReasonPhrase);
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <param name="contentType"></param>
        /// <param name="isToken"></param>
        /// <returns></returns>
        public static T Post<T>(string url, object parameter, string contentType = "application/json", bool isToken = false)
        {
            //string token = GetToken().access_token;
            //string schema = "Basic";

            //if (HttpContext.Current.Session != null && HttpContext.Current.Session["Token"] != null)
            //{
            //    token = (string)HttpContext.Current.Session["Token"];
            //    schema = "Bearer";
            //}

            string p = contentType == "application/json" ? JsonConvert.SerializeObject(parameter, JsonSettings()) : parameter.ToString();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + (url == "token" ? string.Empty : "portal/api/"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(schema, token);
            //client.DefaultRequestHeaders.Add("Language", HttpContext.Current.Session["SelectedLanguage"]?.ToString() ?? "en");
            client.DefaultRequestHeaders.Referrer = HttpContext.Current.Request.UrlReferrer;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(p, Encoding.UTF8, contentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = client.SendAsync(request).Result;

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;


            if (!result.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    if (isToken)
                    {
                        var error = JsonConvert.DeserializeObject<Token>(resultStr, JsonSettings());
                        throw new HttpException((int)HttpStatusCode.Unauthorized, error.error_description);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                        string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                        throw new HttpException(error.HttpStatus, msg);

                    }
                }
                throw new HttpException((int)result.StatusCode, result.ReasonPhrase);
            }
            else
            {
                if (isToken)
                {
                    resultStr = Regex.Replace(resultStr, @"\\", "");
                    resultStr = Regex.Replace(resultStr, @"""{", "{");
                    resultStr = Regex.Replace(resultStr, @"}""", "}");
                }
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            }
        }


        public static T Upload<T>(string url, HttpPostedFile file, ref Stream stream, ref string fileName, bool isToken = false)
        {
            //string token = GetToken().access_token;
            //string schema = "Basic";

            //if (HttpContext.Current.Session["Token"] != null)
            //{
            //    token = (string)HttpContext.Current.Session["Token"];
            //    schema = "Bearer";
            //}

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + "portal/api/");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(schema, token);
            //client.DefaultRequestHeaders.Add("Language", HttpContext.Current.Session["SelectedLanguage"]?.ToString() ?? "en");
            client.DefaultRequestHeaders.Referrer = HttpContext.Current.Request.UrlReferrer;

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("file");
            form.Add(content, "file");

            content = new StreamContent(file.InputStream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = file.FileName
            };
            form.Add(content);
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = client.PostAsync(url, form).Result;
            var resultStr = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    if (isToken)
                    {
                        var error = JsonConvert.DeserializeObject<Token>(resultStr, JsonSettings());
                        throw new HttpException((int)HttpStatusCode.Unauthorized, error.error_description);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                        string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                        throw new HttpException(error.HttpStatus, msg);

                    }
                }
                throw new HttpException((int)result.StatusCode, result.ReasonPhrase);
            }
            else
            {
                if (isToken)
                {
                    resultStr = Regex.Replace(resultStr, @"\\", "");
                    resultStr = Regex.Replace(resultStr, @"""{", "{");
                    resultStr = Regex.Replace(resultStr, @"}""", "}");
                }
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            }
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(string url, object parameter = null)
        {
            //string token = GetToken().access_token;
            //string schema = "Basic";

            //if (HttpContext.Current.Session["Token"] != null)
            //{
            //    token = (string)HttpContext.Current.Session["Token"];
            //    schema = "Bearer";
            //}


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + "portal/api/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(schema, token);
            //client.DefaultRequestHeaders.Add("Language", HttpContext.Current.Session["SelectedLanguage"]?.ToString() ?? "en");
            client.DefaultRequestHeaders.Referrer = HttpContext.Current.Request.UrlReferrer;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            if (parameter != null)
            {
                string p = JsonConvert.SerializeObject(parameter, JsonSettings());
                request.Content = new StringContent(p, Encoding.UTF8, "application/json");
            }

            client.Timeout = TimeSpan.FromHours(2);
            var result = client.SendAsync(request).Result;

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            }
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new HttpException(error.HttpStatus, msg);
                }
                throw new HttpException((int)result.StatusCode, result.ReasonPhrase);
            }
        }

        //internal static string ComposeAuthToken()
        //{
        //    string authToken = string.Format("{0}:{1}", ConfigurationManager.AppSettings["AppSystemId"], ConfigurationManager.AppSettings["PublicKey"]);
        //    authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(authToken));
        //    return authToken;
        //}
        private static AccessTokenVM GetToken()
        {
            try
            {
                //Define Headers
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + "portal/api/token");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Prepare Request Body
                    var form = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", "manager"},
                    {"password", "1234"},
                };
                    //Request Token 
                    var result = client.PostAsync(ConfigurationManager.AppSettings["MerchandisingApiUrl"] + "portal/api/token", new FormUrlEncodedContent(form)).Result;

                    string resultStr = string.Empty;
                    using (HttpContent content = result.Content)
                        resultStr = content.ReadAsStringAsync().Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<AccessTokenVM>(resultStr, JsonSettings());
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(resultStr))
                        {
                            var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                            string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                            throw new HttpException(error.HttpStatus, msg);
                        }
                        throw new HttpException((int)result.StatusCode, result.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}