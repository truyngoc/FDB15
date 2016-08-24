using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Data;
using Excel;
using System.Collections;
using System.Security.Cryptography;

namespace FDB.Common
{
    public static class Helpers
    {

        /// <param name="destObject">Destination object, must already be created</param>
        public static void CopyObject<T>(object sourceObject, ref T destObject)
        {
            //	If either the source, or destination is null, return
            if (sourceObject == null || destObject == null)
                return;

            //	Get the type of each object
            Type sourceType = sourceObject.GetType();
            Type targetType = destObject.GetType();

            //	Loop through the source properties
            foreach (PropertyInfo p in sourceType.GetProperties())
            {
                //	Get the matching property in the destination object
                PropertyInfo targetObj = targetType.GetProperty(p.Name);
                //	If there is none, skip
                if (targetObj == null)
                    continue;

                //	Set the value in the destination
                targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
            }
        }

        public static string ConvertToUnSign(string text)
        {
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            //text = text.Replace(" ", "-"); //Comment lại để không đưa khoảng trắng thành ký tự -

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static void GetValueForm<T>(FormCollection _form, int _LastNumber, ref T t)
        {
            if (t == null)
                t = Activator.CreateInstance<T>();
            List<PropertyInfo> lstPer = t.GetType().GetProperties().ToList();
            foreach (PropertyInfo p in lstPer)
            {
                string _formKey = p.Name + "_" + _LastNumber.ToString();
                string _Key = _form.AllKeys.Where(o => o.Length > 3).ToList().Find(k => k.Substring(3, k.Length - 3).ToUpper() == _formKey.ToUpper());
                if (_Key != null)
                {
                    Type _typeP = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    object safeValue = null;
                    if (_form[_Key] == null)
                        safeValue = null;
                    else if (_form[_Key] == "")
                    {
                        safeValue = null;
                    }
                    else
                    {
                        if (_Key.Substring(0, 3) == "chk")
                            safeValue = Convert.ChangeType(_form[_Key] == "on" ? true : false, _typeP);
                        else
                            safeValue = Convert.ChangeType(_form[_Key], _typeP);
                    }

                    p.SetValue(t, safeValue, null);
                }

            }
        }

        public static void GetValueForm<T>(FormCollection _form, ref T t, string propertyname)
        {
            if (t == null)
                t = Activator.CreateInstance<T>();
            List<PropertyInfo> lstPer = t.GetType().GetProperties().ToList();
            string _Key = _form.AllKeys.ToList().Find(o => o.Length > 3 && ((o.Substring(3, o.Length - 3).ToUpper() == propertyname.ToUpper()) || (o.ToUpper() == propertyname.ToUpper())));

            if (_Key != null)
            {
                PropertyInfo p = t.GetType().GetProperty(propertyname);
                Type _typeP = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                object safeValue = null;
                if (_form[_Key] == null)
                    safeValue = null;
                else if (_form[_Key] == "")
                {
                    safeValue = null;
                }
                else
                {
                    if (_Key.Substring(0, 3) == "chk")
                        safeValue = Convert.ChangeType(_form[_Key] == "on" ? true : false, _typeP);
                    else
                        safeValue = Convert.ChangeType(_form[_Key], _typeP);
                }

                p.SetValue(t, safeValue, null);
            }


        }


        public static void GetForeignKeyName<T>()
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {

                foreach (ForeignKeyAttribute foreignKey in property.GetCustomAttributes(typeof(ForeignKeyAttribute), false))
                {

                }
            }
        }

        public static List<String> GetForeignKeyName<T>(String propertyName)
        {
            List<String> lstForeignKeyName = new List<string>();
            var property = typeof(T).GetProperty(propertyName);
            T t = Activator.CreateInstance<T>();
            foreach (ForeignKeyAttribute foreignKey in property.GetCustomAttributes(typeof(ForeignKeyAttribute), false))
            {
                lstForeignKeyName.Add(foreignKey.Name);
                property.SetValue(t, 1);
            }

            return lstForeignKeyName;
        }

        // làm việc với  File:
        public static FileResult DownLoadFile(string FullPathFileName, string FileName, string ContentType)
        {
            string fileName = FileName;
            string contentType = ContentType;
            var _fileResult = new FilePathResult(FullPathFileName, contentType);
            _fileResult.FileDownloadName = fileName;
            return _fileResult;
        }

        // làm việc với Excel  File:
        public static DataTable ConvertExcelToDataTable(string FilePath)
        {
            DataTable res = null;
            try
            {
                IExcelDataReader iExcelDataReader = null;
                FileInfo fileInfo = new FileInfo(FilePath);
                string file = fileInfo.Name;
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                if (file.Split('.')[1].Equals("xls"))
                {
                    iExcelDataReader = ExcelReaderFactory.CreateBinaryReader(fs);
                }
                else if (file.Split('.')[1].Equals("xlsx"))
                {
                    iExcelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                }
                fs.Dispose();
                iExcelDataReader.IsFirstRowAsColumnNames = true;

                DataSet dsData = new DataSet();
                dsData = iExcelDataReader.AsDataSet();
                iExcelDataReader.Close();
                if (dsData != null && dsData.Tables.Count > 0)
                {
                    res = dsData.Tables[0];
                }
                else
                {
                    // "No data";
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }

            return res;
        }

        public static string MapPropertyNameToText(string FieldText, bool ProperTyNameToText = true, Dictionary<string, string> _mapPropertyNameToText = null)
        {
            if (_mapPropertyNameToText != null)
            {
                string ret = string.Empty;
                if (ProperTyNameToText)
                {
                    foreach (string s in _mapPropertyNameToText.Keys)
                    {
                        if (_mapPropertyNameToText[s] == FieldText)
                            ret = s;
                    }
                }
                else
                {
                    if (_mapPropertyNameToText.ContainsKey(FieldText))
                    {
                        ret = _mapPropertyNameToText[FieldText];
                    }
                    else
                    {
                        ret = FieldText;
                    }
                }
                return ret;
            }
            else
                return string.Empty;


        }

        public static void RenameDataColumn(ref DataTable dt, Func<string, bool, Dictionary<string, string>, string> MapPropertyNameToText, Dictionary<string, string> _mapPropertyNameToText)
        {
            foreach (DataColumn col in dt.Columns)
            {
                col.ColumnName = MapPropertyNameToText.Invoke(col.ColumnName, false, _mapPropertyNameToText);
            }
        }

        public static void RenameDataColumn(ref DataTable dt, Func<string, bool, string> MapPropertyNameToText)
        {
            foreach (DataColumn col in dt.Columns)
            {
                col.ColumnName = MapPropertyNameToText.Invoke(col.ColumnName, false);
            }
        }

        public static DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);
            return dTable;
        }

        public static List<T> MapDataTableToList<T>(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            List<T> list = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(MapDataRowToObject<T>(dr));
            }

            return list;
        }
        public static T MapDataRowToObject<T>(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            T instance = Activator.CreateInstance<T>();

            PropertyInfo[] properties = instance.GetType().GetProperties();

            if ((properties.Length > 0))
            {

                foreach (PropertyInfo propertyObject in properties)
                {
                    bool valueSet = false;


                    foreach (object attributeObject in propertyObject.GetCustomAttributes(false))
                    {
                        Type _typeP = Nullable.GetUnderlyingType(propertyObject.PropertyType) ?? propertyObject.PropertyType;
                        if (object.ReferenceEquals(attributeObject.GetType(), typeof(MapperColumn)))
                        {
                            MapperColumn columnAttributeObject = (MapperColumn)attributeObject;



                            if ((columnAttributeObject.ColumnName != string.Empty))
                            {

                                if (dr.Table.Columns.Contains(columnAttributeObject.ColumnName) && (!object.ReferenceEquals(dr[columnAttributeObject.ColumnName], DBNull.Value)))
                                {
                                    propertyObject.SetValue(instance, Convert.ChangeType(dr[columnAttributeObject.ColumnName], _typeP), null);

                                    valueSet = true;

                                }

                            }
                        }
                    }

                    if (!valueSet)
                    {
                        if (dr.Table.Columns.Contains(propertyObject.Name) && !object.ReferenceEquals(dr[propertyObject.Name], DBNull.Value))
                        {
                            Type _typeP = Nullable.GetUnderlyingType(propertyObject.PropertyType) ?? propertyObject.PropertyType;
                            propertyObject.SetValue(instance, Convert.ChangeType(dr[propertyObject.Name], _typeP), null);

                        }
                    }

                }
            }

            return instance;
        }

        [AttributeUsage(AttributeTargets.Property)]
        private class MapperColumn : Attribute
        {


            private string mColumnName;
            public MapperColumn(string columnName)
            {
                mColumnName = columnName;
            }

            public string ColumnName
            {
                get { return mColumnName; }
                set { mColumnName = value; }
            }
        }


    }

    #region "Notify"
    public enum NotifyType
    {
        Success,
        Info,
        Warning,
        Error
    }

    public class Notify
    {
        public const string TempDataKey = "TempDataNotifys";

        public string Title { get; set; }
        public string Message { get; set; }

        public NotifyType NotifyType { get; set; }
    }

    public class FDBController : Controller
    {
        public void Success(string message, string title = "")
        {
            AddNotify(NotifyType.Success, title, message);
        }

        public void Information(string message, string title = "")
        {
            AddNotify(NotifyType.Info, title, message);
        }

        public void Warning(string message, string title = "")
        {
            AddNotify(NotifyType.Warning, title, message);
        }

        public void Error(string message, string title = "")
        {
            AddNotify(NotifyType.Error, title, message);
        }


        // Inline alert
        public void Inline_Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Inline_Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Inline_Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Inline_Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }


        private void AddNotify(NotifyType notifyType, string title, string message)
        {
            var notifys = TempData.ContainsKey(Notify.TempDataKey)
                ? (List<Notify>)TempData[Notify.TempDataKey]
                : new List<Notify>();

            notifys.Add(new Notify
            {
                Title = title,
                Message = message,
                NotifyType = notifyType
            });

            TempData[Notify.TempDataKey] = notifys;
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
    }
    #endregion


    #region "Alert inline form"
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }

    public class BaseController : Controller
    {
        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

    }
    #endregion


    #region "Custom razor view engine"
    public class CustomViewLocationRazorViewEngine : RazorViewEngine
    {
        public CustomViewLocationRazorViewEngine()
        {
            AreaViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            AreaMasterLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            AreaPartialViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };

            ViewLocationFormats = new[] { 
                "~/Views/{1}/{0}.cshtml", "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.vbhtml",
                "~/Views/AccountManagement/{1}/{0}.cshtml","~/Views/AccountManagement/{1}/{0}.vbhtml",   // them moi location AccountManagement
                "~/Views/DanhMuc/{1}/{0}.cshtml","~/Views/DanhMuc/{1}/{0}.vbhtml",   // them moi location danh muc
                "~/Views/KhaiThac/{1}/{0}.cshtml","~/Views/KhaiThac/{1}/{0}.vbhtml",   // them moi location khai thac
                "~/Views/NuoiTrong/{1}/{0}.cshtml","~/Views/NuoiTrong/{1}/{0}.vbhtml",   // them moi location nuoi trong
                "~/Views/REPORTS/{1}/{0}.cshtml","~/Views/REPORTS/{1}/{0}.vbhtml"   // them moi location reports
            };

            MasterLocationFormats = new[] { 
                "~/Views/{1}/{0}.cshtml", "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.vbhtml",
                "~/Views/AccountManagement/{1}/{0}.cshtml","~/Views/AccountManagement/{1}/{0}.vbhtml",   // them moi location AccountManagement
                "~/Views/DanhMuc/{1}/{0}.cshtml","~/Views/DanhMuc/{1}/{0}.vbhtml",   // them moi location danh muc
                "~/Views/KhaiThac/{1}/{0}.cshtml","~/Views/KhaiThac/{1}/{0}.vbhtml",   // them moi location khai thac
                "~/Views/NuoiTrong/{1}/{0}.cshtml","~/Views/NuoiTrong/{1}/{0}.vbhtml",   // them moi location nuoi trong
                "~/Views/REPORTS/{1}/{0}.cshtml","~/Views/REPORTS/{1}/{0}.vbhtml"   // them moi location reports
            };

            PartialViewLocationFormats = new[] { 
                "~/Views/{1}/{0}.cshtml", "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.vbhtml",
                "~/Views/AccountManagement/{1}/{0}.cshtml","~/Views/AccountManagement/{1}/{0}.vbhtml",   // them moi location AccountManagement
                "~/Views/DanhMuc/{1}/{0}.cshtml","~/Views/DanhMuc/{1}/{0}.vbhtml",   // them moi location danh muc
                "~/Views/KhaiThac/{1}/{0}.cshtml","~/Views/KhaiThac/{1}/{0}.vbhtml",   // them moi location khai thac
                "~/Views/NuoiTrong/{1}/{0}.cshtml","~/Views/NuoiTrong/{1}/{0}.vbhtml",   // them moi location nuoi trong
                "~/Views/REPORTS/{1}/{0}.cshtml","~/Views/REPORTS/{1}/{0}.vbhtml"   // them moi location reports
            };
        }
    }
    #endregion

    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
        }
    }
    public class NullableDateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return value == null
                ? null
                : value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
        }
    }

    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var cultureCookie = controllerContext.HttpContext.Request.Cookies["language"];

            var culture = "en-US";

            if (cultureCookie != null)
                culture = cultureCookie.Value;

            decimal value;

            var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProvider == null)
                return null;

            if (String.IsNullOrEmpty(valueProvider.AttemptedValue))
                return null;

            if (Decimal.TryParse(valueProvider.AttemptedValue, NumberStyles.Currency, new CultureInfo(culture), out value))
            {
                return value;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid decimal");

            return null;
        }
    }
}
