using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApp
{
    public class SmsMisrService
    {
        public static async Task<string> SendSms(string url, string userName, string password, string senderId,
            string mobileNumbers, string message)
        {
            var fullUrl = BuildArgs(userName, password, senderId, mobileNumbers, message);

            fullUrl = url + "?" + fullUrl;

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(fullUrl);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = WebUtility.HtmlDecode(responseContent);
                var doc = new HtmlDocument();
                doc.LoadHtml(result);
                result = doc.DocumentNode.InnerText;
                return result;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        private static string BuildArgs(string userName, string password, string senderId, string mobileNumbers, string message)
        {
            var isUnicode = IsUnicode(message);

            message = isUnicode ? Encode(message) : message;

            var language = isUnicode ? "3" : "1";

            var fullUrl = "username=" + userName
                      + "&password=" + password
                      + "&language=" + language
                      + "&sender=" + senderId
                      + "&mobile=" + mobileNumbers
                      + "&message=" + message;

            return fullUrl;
        }

        public static bool IsUnicode(string text)
        {
            return text.Where((t, index) => char.ConvertToUtf32(text, index) > 255).Any();
        }

        private static string Encode(string message)
        {
            var encodedMessage = "";

            for (var index = 0; index < message.Length; index++)
                encodedMessage += char.ConvertToUtf32(message, index).ToString("X4");

            return encodedMessage;
        }

        public static string FormatResult(string result)
        {
            var firstLine = result.Split('\r', '\n').FirstOrDefault();
            var values = firstLine?.Split(',');
            var formatedResult = new StringBuilder();

            if (values != null && values.Length > 0)
            {
                switch (values[0])
                {
                    case "1901":
                        values[0] = "Success, Message Submitted Successfully";
                        break;
                    case "1902":
                        values[0] = "Invalid URL, This means that one of the parameters was not provided";
                        break;
                    case "1903":
                        values[0] = "Invalid value in username or password field";
                        break;
                    case "1904":
                        values[0] = "Invalid value in \"sender\" field ";
                        break;
                    case "1905":
                        values[0] = "Invalid value in \"mobile\" field ";
                        break;
                    case "1906":
                        values[0] = "Insufficient Credit";
                        break;
                    case "1907":
                        values[0] = "Server under updating";
                        break;
                }
            }

            if (values == null) return "";

            foreach (var value in values)
            {
                formatedResult.AppendLine(value);
            }

            return formatedResult.ToString();
        }
    }
}