using Model.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace WebApp.Models
{
    public static class Utils
    {
     
        public static void LogError(string err)
        {

        }
        public static List<Error> ParseErrors(ICollection<ModelState> Values)
        {

            return new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() {
                message =  string.Join("; ", Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage))
                } } } };
        }

        public static string ParseError(ICollection<ModelState> Values)
        {
            return string.Join("; ", Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
        }

        //Parse Form Errors for Company Details
        public static List<FormErrorViewModel> ParseFormErrors(ModelStateDictionary modelState)
        {
            List<FormErrorViewModel> formErrorsList = new List<FormErrorViewModel>();
            foreach (var item in modelState)
            {
                string msg = string.Join("; ", item.Value.Errors.Select(e => e.ErrorMessage));
                if (msg != "")
                {
                    formErrorsList.Add(new FormErrorViewModel()
                    {
                        Field = item.Key,
                        Message = msg
                    });
                }
            }
            return formErrorsList;
        }
        public static List<FormErrorViewModel> ParseFormError(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            List<FormErrorViewModel> formErrorsList = new List<FormErrorViewModel>();
            foreach (var item in modelState)
            {
                string msg = string.Join("; ", item.Value.Errors.Select(e => e.ErrorMessage));
                if (msg != "")
                {
                    formErrorsList.Add(new FormErrorViewModel()
                    {
                        Field = item.Key,
                        Message = msg
                    });
                }
            }
            return formErrorsList;
        }


        public static IList<ColumnsView> GetModifiedRows(IEnumerable<KeyValuePair<string, ModelState>> models)
        {
            var columns = new List<ColumnsView>();
            short i = 0;
            int index = 0, length = 1;

            foreach (var key in models)
            {
                //if (i == 0)
                //{
                    index = key.Key.IndexOf('[') + 1;
                    length = key.Key.IndexOf(']') - index;
                //}

                if (key.Key.IndexOf('.') != -1)
                {
                    ColumnsView column = new ColumnsView
                    {
                        Row = short.Parse(key.Key.Substring(index, length)),
                        Cell = i,
                        Name = key.Key.Substring(key.Key.LastIndexOf('.') + 1),
                        Value = key.Value.Value.AttemptedValue
                    };

                    columns.Add(column);
                    i++;
                }
            }

            return columns;
        }

        public static ColumnsView[] GetColumnViews(IEnumerable<KeyValuePair<string, ModelState>> models) //   ModelStateDictionary ModelState
        {
            ColumnsView[] ColumnsViews = new ColumnsView[models.Count()];
            int i = 0;
            foreach (var model in models)
            {
                ColumnsViews[i++] = new ColumnsView() { Name = model.Key, Value = model.Value.Value.AttemptedValue, Cell = 0, Row = 1 };
            }

            return ColumnsViews;
        }

        public static IQueryable GetFilter(IQueryable query, ref string filter,ref string Sorting)
        {
            if (System.Web.HttpContext.Current != null)
            {
                var curRequest = System.Web.HttpContext.Current.Request.Params;
                var FilterLogic = curRequest["filter[logic]"]; // AND
                var SortField = curRequest["sort[0][field]"];
                if(SortField != null)
                {
                    string dir = curRequest["sort[0][dir]"];
                    if (dir == "asc")
                        Sorting = SortField + " ascending";
                    else
                        Sorting = SortField + " descending";
                }

                var x = 0;
                while (x < 20)
                {
                    var field = curRequest["filter[filters][" + x + "][field]"];
                    if (field == null)
                        break;

                    if (x > 0) filter += " " + FilterLogic + " ";
                    var val = curRequest["filter[filters][" + x + "][value]"] ?? string.Empty;
                    var strop = curRequest["filter[filters][" + x + "][operator]"];
                    if (strop != null) query = getFilter(field, strop, val, query, ref filter);

                    x++;
                }
            }

            return query;
        }

        private static IQueryable getFilter(string field, string theOperator, string val, IQueryable query, ref string filter)
        {
            field.Trim();
            string Type = "";
            if (query.Count() > 0)
                Type = ((System.Reflection.TypeInfo)(query.Take(1).ElementType)).GetDeclaredProperty(field).PropertyType.Name;
            switch (theOperator)
            {
                //equal ==
                case "eq":
                case "==":
                case "isequalto":
                case "equals":
                case "equalto":
                case "equal":
                    if (Type != "String")
                        filter += "(" + field + " = " + val + ")";
                    else
                        filter += "(" + field + " = \"" + val + "\"" + ")";
                    break;
                //not equal !=
                case "neq":
                case "!=":
                case "isnotequalto":
                case "notequals":
                case "notequalto":
                case "notequal":
                case "ne":
                    if (Type != "String")
                        filter += "(" + field + " != " + val + ")";
                    else
                        filter += "(" + field + " != \"" + val + "\"" + ")";
                    break;
                // Greater
                case "gt":
                case ">":
                case "isgreaterthan":
                case "greaterthan":
                case "greater":
                    filter += "(" + field + " > " + val + ")";
                    break;
                // Greater or equal
                case "gte":
                case ">=":
                case "isgreaterthanorequalto":
                case "greaterthanequal":
                case "ge":
                    filter += "(" + field + " >= " + val + ")";
                    break;
                // Less
                case "lt":
                case "<":
                case "islessthan":
                case "lessthan":
                case "less":
                    filter += "(" + field + " < " + val + ")";
                    break;
                // Less or equal
                case "lte":
                case "<=":
                case "islessthanorequalto":
                case "lessthanequal":
                case "le":
                    filter += "(" + field + " <= " + val + ")";
                    break;
                case "startswith":
                    {
                        query = query.Where("" + field + ".StartsWith(@0)", val);
                        break;
                    }
                case "endswith":
                    {
                        query = query.Where("" + field + ".EndsWith(@0)", val);
                        break;
                    }
                case "contains":
                    {
                        query = query.Where("" + field + ".Contains(@0)", val);
                        break;
                    }
                case "doesnotcontain":
                    {
                        query = query.Where("!" + field + ".Contains(@0)", val);
                        break;
                    }
                default:
                    {
                        query = query.Where("" + field + ".Contains(@0)", val);
                        break;
                    }
            }

            return query;
        }

    }
}