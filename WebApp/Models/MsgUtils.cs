using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebApp.Extensions;


//public static class MsgUtil
//{
//    private static List<Model.Domain.MsgTbl> Messages = null;

//    public static string DefaultCulture = "en-GB";
//    private static void Initialize()
//    {
//        if (Messages == null)
//        {
//            var db = new Db.HrContext(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString);
//            Messages = db.MsgTbl.ToList();
//        }
//    }
//    public static string Trls(string msg)
//    {
//        Initialize();

//        string culture = System.Web.HttpContext.Current.User.Identity.GetCulture();
//        var word = Messages.FirstOrDefault(m => m.Culture == culture && m.Name == msg);
//        if (word == null || string.IsNullOrEmpty(word.Meaning))
//        {
//            word = Messages.FirstOrDefault(m => m.Culture == DefaultCulture && m.Name == msg);
//            if (word == null || string.IsNullOrEmpty(word.Meaning))
//                return msg;
//        }

//        return word.Meaning;
//    }
//    public static string TrlsMany(string msg)
//    {
//        var start = msg.IndexOf("'");
//        StringBuilder result = new StringBuilder();
//        int end, index = 0;

//        while (start >= 0)
//        {
//            end = msg.Substring(index + start + 1).IndexOf("'");
//            if (end > 0) result.Append(msg.Substring(index, start) + Trls(msg.Substring(index + start + 1, end)));
//            index += start + end + 2;
//            start = msg.Substring(index).IndexOf("'");
//        }

//        result.Append(msg.Substring(index));

//        return result.ToString();
//    }
//    public static List<Model.ViewModel.Error> TrlsMany(List<Model.ViewModel.Error> errors)
//    {
//        return errors.Select(e => new Model.ViewModel.Error
//        {
//            id = e.id,
//            page = e.page,
//            row = e.row,
//            errors = e.errors.Select(a => new Model.ViewModel.ErrorMessage { field = a.field, message = TrlsMany(a.message) }).ToList()
//        }).ToList();
//    }
//    public static string GetJsMessages()
//    {
//        Initialize();

//        var messages = Messages
//            .Where(m => m.Culture == System.Web.HttpContext.Current.User.Identity.GetCulture() && m.JavaScript)
//            .Select(m => new { name = m.Name, msg = m.Meaning})
//            .ToList();

//        StringBuilder js = new StringBuilder("[");
//        foreach (var message in messages)
//        {
//            if (js.Length > 1) js.Append(",");
//            js.Append("{\"name\": \"" + message.name + "\", \"msg\": \"" + message.msg + "\"}");
//        }

//        if (js.Length == 1)
//            js = new StringBuilder("empty");
//        else
//            js.Append("]");

//        return js.ToString();
//    }
//    public static void Refresh()
//    {
//        Messages = null;
//    }
//}

//public sealed class MsgUtils
//{
//    static List<Model.Domain.MsgTbl> Messages;
//    static string DefaultCulture = "en-GB";
//    private static volatile MsgUtils instance;
//    private static object syncRoot = new Object();

//    private MsgUtils() {
//        if (Messages == null)
//        {
//            var db = new Db.HrContext(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString);
//            Messages = db.MsgTbl.ToList();
//        }
//    }

//    /// <summary>
//    /// used to reload Messages table again from database to memory
//    /// </summary>
//    public void Refresh()
//    {
//        instance = null;
//    }

//    public static MsgUtils Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                lock (syncRoot)
//                {
//                    if (instance == null)
//                        instance = new MsgUtils();
//                }
//            }

//            return instance;
//        }
//    }

//    /// <summary>
//    /// used to translate message code to text according to current user language or culture
//    /// Ex: Trls("AlreadyExist")
//    /// </summary>
//    public string Trls(string msg)
//    {
//        string culture = System.Web.HttpContext.Current.User.Identity.GetCulture();
//        var word = Messages.FirstOrDefault(m => m.Culture == culture && m.Name == msg);
//        if (word == null || string.IsNullOrEmpty(word.Meaning))
//        {
//            word = Messages.FirstOrDefault(m => m.Culture == DefaultCulture && m.Name == msg);
//            if (word == null || string.IsNullOrEmpty(word.Meaning))
//                return msg;
//        }

//        return word.Meaning;
//    }

//    /// <summary>
//    /// used to translate many message codes at once code passed between single quotations
//    /// 'Errors': 'AlreadyExist', 'Required'.
//    /// </summary>
//    public string TrlsMany(string msg)
//    {
//        var start = msg.IndexOf("'");
//        StringBuilder result = new StringBuilder();
//        int end, index = 0;

//        while (start >= 0)
//        {
//            end = msg.Substring(index + start + 1).IndexOf("'");
//            if (end > 0) result.Append(msg.Substring(index, start) + Trls(msg.Substring(index + start + 1, end)));
//            index += start + end + 2;
//            start = msg.Substring(index).IndexOf("'");
//        }

//        result.Append(msg.Substring(index));

//        return result.ToString();
//    }

//    /// <summary>
//    /// Translate list of Error objects
//    /// </summary>
//    public List<Model.ViewModel.Error> TrlsMany(List<Model.ViewModel.Error> errors)
//    {
//        return errors.Select(e => new Model.ViewModel.Error
//        {
//            id = e.id,
//            page = e.page,
//            row = e.row,
//            errors = e.errors.Select(a => new Model.ViewModel.ErrorMessage { field = a.field, message = TrlsMany(a.message) }).ToList()
//        }).ToList();
//    }

//    /// <summary>
//    /// Return string formatted in JavaScript array of objects that contains all messages 
//    /// used in JS files
//    /// </summary>
//    public string GetJsMessages()
//    {
//        var messages = Messages
//            .Where(m => m.Culture == System.Web.HttpContext.Current.User.Identity.GetCulture() && m.JavaScript)
//            .Select(m => new { name = m.Name, msg = m.Meaning })
//            .ToList();

//        StringBuilder js = new StringBuilder("[");
//        foreach (var message in messages)
//        {
//            if (js.Length > 1) js.Append(",");
//            js.Append("{\"name\": \"" + message.name + "\", \"msg\": \"" + message.msg + "\"}");
//        }

//        if (js.Length == 1)
//            js = new StringBuilder("empty");
//        else
//            js.Append("]");

//        return js.ToString();
//    }
//}

public sealed class MsgUtils
{
    private static volatile MsgUtils instance;
    private static object syncRoot = new Object();

    private MsgUtils()
    {
        Db.MsgUtils.Instance.Create(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString);
    }

    /// <summary>
    /// used to reload Messages table again from database to memory
    /// </summary>
    public void Refresh()
    {
        Db.MsgUtils.Instance.Refresh(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString);
    }

    public static MsgUtils Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new MsgUtils();
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// used to translate message code to text according to current user language or culture
    /// Ex: Trls("AlreadyExist")
    /// </summary>
    public string Trls(string msg,string Language = null)
    {
        var message = Db.MsgUtils.Instance.Trls(Language == null ? HttpContext.Current.User.Identity.GetLanguage():Language, msg);
        if (string.IsNullOrEmpty(message))
            message = Db.MsgUtils.Instance.Trls("en-GB", msg);
        return Db.MsgUtils.Instance.Trls(Language == null ? HttpContext.Current.User.Identity.GetLanguage() : Language, msg);
    }
  
    /// <summary>
    /// used to translate many message codes at once code passed between single quotations
    /// 'Errors': 'AlreadyExist', 'Required'.
    /// </summary>
    public string TrlsMany(string msg)
    {
        var start = msg.IndexOf("'");
        StringBuilder result = new StringBuilder();
        int end, index = 0;

        while (start >= 0)
        {
            end = msg.Substring(index + start + 1).IndexOf("'");
            if (end > 0) result.Append(msg.Substring(index, start) + Trls(msg.Substring(index + start + 1, end)));
            index += start + end + 2;
            start = msg.Substring(index).IndexOf("'");
        }

        result.Append(msg.Substring(index));

        return result.ToString();
    }

    /// <summary>
    /// Translate list of Error objects
    /// </summary>
    public List<Model.ViewModel.Error> TrlsMany(List<Model.ViewModel.Error> errors)
    {
        return errors.Select(e => new Model.ViewModel.Error
        {
            id = e.id,
            page = e.page,
            row = e.row,
            errors = e.errors.Select(a => new Model.ViewModel.ErrorMessage { field = a.field, message = TrlsMany(a.message) }).ToList()
        }).ToList();
    }

    /// <summary>
    /// Return string formatted in JavaScript array of objects that contains all messages 
    /// used in JS files
    /// </summary>
    public string GetJsMessages()
    {
        var message = Db.MsgUtils.Instance.GetJsMessages(System.Web.HttpContext.Current.User.Identity.GetLanguage());
        if (message == "empty" )
            Db.MsgUtils.Instance.GetJsMessages("en-GB");

        return message;
    }
}