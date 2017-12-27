using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Domain;

namespace Db
{
    public sealed class MsgUtils
    {
        static List<MsgTbl> Messages;
        static string DefaultCulture = "en-GB";
        private static volatile MsgUtils instance;
       
        private MsgUtils()
        {
            
        }

        public MsgUtils Create(string connectionString)
        {
            if (Messages == null)
            {
                var db = new HrContext(connectionString);
                Messages = db.MsgTbl.ToList();
            }

            return Instance;
        }


        /// <summary>
        /// used to reload Messages table again from database to memory
        /// </summary>
        public void Refresh(string connectionString)
        {
            var db = new HrContext(connectionString);
            Messages = db.MsgTbl.ToList();
        }

        public static MsgUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MsgUtils();
                }

                return instance;
            }
        }

        /// <summary>
        /// used to translate message code to text according to current user language or culture
        /// Ex: Trls("ar-EG", "AlreadyExist")
        /// </summary>
        public string Trls(string culture, string msg)
        {
            if (Messages == null)
                return msg;
            var word = Messages.FirstOrDefault(m => m.Culture == culture && m.Name == msg);
            if (word == null || string.IsNullOrEmpty(word.Meaning))
            {
               // var lang = culture.Substring(0, 2) == "ar" ? "ar-EG" : "en-GB";
                word = Messages.FirstOrDefault(m => m.Culture == DefaultCulture && m.Name == msg);
                if (word == null || string.IsNullOrEmpty(word.Meaning))
                    return msg;
            }

            return word.Meaning;
        }

     
        /// <summary>
        /// Return string formatted in JavaScript array of objects that contains all messages 
        /// used in JS files
        /// </summary>
        public string GetJsMessages(string culture)
        {
            var messages = Messages
                .Where(m => m.Culture == culture && m.JavaScript)
                .Select(m => new { name = m.Name, msg = m.Meaning })
                .ToList();

            StringBuilder js = new StringBuilder("[");
            foreach (var message in messages)
            {
                if (js.Length > 1) js.Append(",");
                js.Append("{\"name\": \"" + message.name + "\", \"msg\": \"" + message.msg + "\"}");
            }

            if (js.Length == 1)
                js = new StringBuilder("empty");
            else
                js.Append("]");

            return js.ToString();
        }
    }
}
