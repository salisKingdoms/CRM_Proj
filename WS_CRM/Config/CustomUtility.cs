
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using WS_CRM.Helper;
//using System.Web.Helpers;

namespace WS_CRM.Config
{
    public class CustomUtility
    {
        enum State
        {
            Initial,
            Quote,
            Data,
            NestedQuote
        }

        public static (string, Dictionary<string, object>) GetWhere<T>(T obj, bool isExtendWhere, GlobalFilter pagingParam)
        {
            string result = string.Empty;

            bool isFirstWhere = !isExtendWhere;
            Dictionary<string, object> param = new Dictionary<string, object>();
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                var display_name = p.GetCustomAttributes(typeof(ColumnAttribute), false).Cast<ColumnAttribute>().Single().Name;
                if (display_name == pagingParam.filter_column)
                {
                    PropertyFilter propertyFilter = GetPropertyFilter(p, pagingParam.filter);

                    if (propertyFilter.filtration)
                    {
                        if (isFirstWhere)
                            isFirstWhere = false;
                        else
                            result += " and ";

                        result += propertyFilter.nameWithTable + propertyFilter.@operator;

                        param.Add(p.Name, pagingParam.filter);
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(result) && !isExtendWhere)
                result = " where " + result;

            return (result + " ", param);
        }

        public static PropertyFilter GetPropertyFilter(PropertyInfo p, object value)
        {
            string nameWithTable = p.GetCustomAttributes(typeof(ColumnAttribute), false).Cast<ColumnAttribute>().Single().Name;
            object[] getCustomAttr = p.GetCustomAttributes(typeof(CustomAttribute), false);
            string TypeName = getCustomAttr.Any() ? getCustomAttr.Cast<CustomAttribute>().Single().Name : string.Empty;

            PropertyFilter propertyFilter = new PropertyFilter()
            {
                @operator = " = ",
                filtration = false,
                nameWithTable = ""
            };

            switch (p.PropertyType.Name)
            {
                case nameof(String):
                    propertyFilter.nameWithTable = " lower(" + nameWithTable + ") ";
                    if (TypeName == "Like")
                    {
                        propertyFilter.@operator = " like '%' || lower(@" + p.Name + "::varchar) || '%'";
                    }

                    propertyFilter.filtration = value != null;
                    break;

                case nameof(Int64):
                    propertyFilter.nameWithTable = nameWithTable;
                    if (TypeName == "Equal")
                    {
                        propertyFilter.@operator = TypeName == "Equal" ? @" = @" + p.Name + "::bigint" : string.Empty;
                    }

                    propertyFilter.filtration = value != null;
                    break;

                case nameof(Boolean):
                    propertyFilter.nameWithTable = nameWithTable;
                    if (TypeName == "Equal")
                    {
                        propertyFilter.@operator = TypeName == "Equal" ? @" = @" + p.Name + "::boolean" : string.Empty;
                    }

                    propertyFilter.filtration = value != null;
                    break;

                case nameof(DateTime):
                    propertyFilter.nameWithTable = nameWithTable;
                    if (TypeName == "Equal")
                    {
                        propertyFilter.@operator = TypeName == "Equal" ? @" = @" + p.Name + "::date" : string.Empty;
                    }

                    propertyFilter.filtration = value != null;
                    break;
            }

            return propertyFilter;
        }

        public static bool GetAttributeColumnOrder(object value, int position, out string propName)
        {
            dynamic properties = value.GetType().GetProperties();
            bool result = false;
            string propertiesName = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();
                if (attributes == null)
                {
                    result = false;
                }
                else
                {
                    if (Convert.ToInt32(attributes.GetType().GetProperties().FirstOrDefault()?.GetValue(attributes)) == position)
                    {
                        propertiesName = property.Name;
                        result = true;
                        break;
                    }
                }
            }
            propName = propertiesName;
            return result;
        }

        public static string[] ReadCsv(StreamReader reader)
        {
            var line = reader.ReadLine();
            var retval = new List<string>();

            if (line == null)
                return null;

            var state = State.Initial;
            var text = new StringBuilder();

            foreach (var ch in line)
                switch (state)
                {
                    case State.Initial:
                        if (ch == '"')
                            state = State.Quote;
                        else if (ch == ',')
                            retval.Add(string.Empty);
                        else
                        {
                            text.Append(ch);
                            state = State.Data;
                        }

                        break;

                    case State.Data:
                        if (ch == ',')
                        {
                            retval.Add(text.ToString());
                            text.Length = 0;
                            state = State.Initial;
                        }
                        else
                            text.Append(ch);

                        break;

                    case State.Quote:
                        if (ch == '"')
                            state = State.NestedQuote;
                        else
                            text.Append(ch);

                        break;

                    case State.NestedQuote:
                        if (ch == '"')
                        {
                            text.Append('"');
                            state = State.Quote;
                            break;
                        }

                        state = State.Data;
                        goto case State.Data;
                }

            retval.Add(text.ToString());

            return retval.ToArray();
        }

        public static bool isValidWarrantyCode(string warranty_code)
        {
            if (!string.IsNullOrEmpty(warranty_code))
            {
                string[] split = warranty_code.Split("-");

                return split.Length < 3 ? false : true;
            }
            else
            {
                return false;
            }
        }

        public static int GenerateWarrantyMonthPeriod(string warranty_code)
        {
            int result = 0;

            string[] split = warranty_code.Split("-");

            foreach (var item in split)
            {
                string word = item.Substring(item.Length - 3);
                int calculate = CalculateWarrantyMonth(word);

                if (calculate > result)
                {
                    result = calculate;
                }
            }
            return result;
        }

        public static int[] DecodeWarrantyMonthPeriod(string warranty_code)
        {
            int[] result_data = new int[3]; //urutan = Unit, Sparepart, Service.
            int counter = 0;
            string[] split = warranty_code.Split("-");

            foreach (var item in split)
            {
                string word = item.Substring(item.Length == 6 ? item.Length - 3 : item.Length - 2);
                result_data[counter] = CalculateWarrantyMonth(word);
                counter++;
            }

            return result_data;
        }

        public static int CalculateWarrantyMonth(string word)
        {
            int result = 0;

            switch (word)
            {
                case "000":
                    result = 0;
                    break;
                case "LFT":
                    result = 999;
                    break;
                default:
                    if (word.ToUpper().Contains("Y"))
                    {
                        int period = Convert.ToInt32(word.Substring(0, 2));
                        result += period * 12;
                    }
                    else if (word.ToUpper().Contains("M"))
                    {
                        int period = 0;
                        if (word.Length == 3)
                        {
                            period = Convert.ToInt32(word.Substring(0, 2));
                        }
                        else
                        {
                            period = Convert.ToInt32(word.Substring(0, 1));
                        }
                        result += period;
                    }
                    else if (word.ToUpper().Contains("S"))
                    {
                        result += 0; //harus dipastikan apakah ada yang S
                    }
                    break;
            }

            return result;
        }

        public static string GenerateWarranty(string warranty_code)
        {
            string result = string.Empty;
            List<string> warranty_split = new List<string>();

            string[] split = warranty_code.Split("-");

            foreach (var item in split)
            {
                warranty_split.Add(GenerateWarrantyString(item));
            }

            result = string.Join(System.Environment.NewLine, warranty_split);

            return result;
        }

        public static string GenerateWarrantyString(string split)
        {
            List<string> strings = new List<string>();

            string company = string.Empty;
            string pattern1 = split.Substring(0, 2);
            string pattern2 = split.Substring(2, 1);
            string pattern3 = split.Substring(3, split.Length - 3);

            strings.Add("Garansi");
            switch (pattern1)
            {
                case "WU":
                    strings.Add("Unit");
                    break;
                case "WP":
                    strings.Add("Spare Part");
                    break;
                case "SV":
                    strings.Add("Service");
                    break;
                default:
                    break;
            }

            switch (pattern2)
            {
                case "S":
                    company = "ACE";
                    break;
                case "K":
                    company = "KLS";
                    break;
                default:
                    break;
            }

            switch (pattern3)
            {
                case "000":
                    strings.Add("Tidak ada");
                    break;
                case "06M":
                    strings.Add("6 Bulan");
                    break;
                case "12M":
                    strings.Add("12 Bulan");
                    break;
                case "06Y":
                    strings.Add("6 Tahun");
                    break;
                case "LFT":
                    strings.Add("Seumur hidup");
                    break;
                default:
                    break;
            }

            return String.Join(" ", strings);
        }

        //public static bool SendEmail(Email email)
        //{
        //    bool result = false;
        //    try
        //    {
        //        using (MailMessage mm = new MailMessage(email.From, email.To))
        //        {
        //            mm.Subject = email.Subject;
        //            mm.Body = GetEmailBody(email.Template, email.Body, email.Subject);
        //            mm.IsBodyHtml = true;
        //            if (!string.IsNullOrEmpty(email.CC) && email.CC.ToLower() != "string")
        //            {
        //                if (email.CC.Contains(","))
        //                {
        //                    var multipleCC = email.CC.Split(',');
        //                    foreach (var _cc in multipleCC)
        //                    {
        //                        mm.CC.Add(new MailAddress(_cc));
        //                    }
        //                }
        //                else
        //                {
        //                    mm.CC.Add(new MailAddress(email.CC));
        //                }

        //            }

        //            if (email.Attachment != null && !string.IsNullOrEmpty(email.Attachment_name))
        //            {
        //                if (Path.GetExtension(email.Attachment_name) == ".zip")
        //                {
        //                    Attachment attachment = new Attachment(email.Attachment, email.Attachment_name, MediaTypeNames.Application.Zip);
        //                    mm.Attachments.Add(attachment);
        //                }
        //                else
        //                {
        //                    Attachment attachment = new Attachment(email.Attachment, Path.GetFileName(email.Attachment_name));
        //                    mm.Attachments.Add(attachment);
        //                }
        //            }

        //            using (SmtpClient smtp = new SmtpClient(email.MailHost, email.MailPort))
        //            {
        //                //smtp.Credentials = new System.Net.NetworkCredential(email.MailUsername, email.MailPassword);
        //                smtp.Send(mm);
        //            }
        //        }

        //        result = true;
        //    }
        //    catch (SmtpException ex)
        //    {
        //        string message = ex.Message;
        //        result = false;
        //    }


        //    return result;
        //}

        //private static string GetEmailBody(string template, string body, string subject)
        //{
        //    string result = string.Empty;

        //    switch (template)
        //    {
        //        //case ActivityConst.TEMPLATE_EMAIL:
        //        //    result = GetEmailHtml(ActivityConst.TEMPLATE_EMAIL, body, subject);
        //        //    break;
        //    }

        //    return result;
        //}

        private static string GetEmailHtml(string template, string bodyContent, string subject)
        {
            string EmailBody = string.Empty;
            EmailBody = "<!doctype html>\r\n<html lang=\"en-US\">\r\n\r\n<head>\r\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n    <title>Template 1</title>\r\n    <meta name=\"description\" content=\"New Registration Email Template.\">\r\n    <style type=\"text/css\">\r\n        a:hover {\r\n            text-decoration: underline !important;\r\n        }\r\n\r\n        table th {\r\n            font-family: 'Open Sans', sans-serif;\r\n            font-size: 16px;\r\n            font-weight: normal;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<!-- //NOSONAR --><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #fff;\" leftmargin=\"0\">\r\n\r\n    <!-- //NOSONAR --><table aria-describedby=\"table1\" cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#fff\" style=\"font-family: 'Open Sans', sans-serif;\">\r\n        <tr>\r\n            <th scope=\"col\"></th></tr><tr><div id=\"bodyContent\"></div></tr><tr><td style=\"text-align:left;\"><p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.kawanlama.com</strong></p></td></tr></table></body></html>";
            //string FilePath = Path.Combine(Environment.CurrentDirectory, "EmailTemplate", template);
            //StreamReader str = new StreamReader(FilePath);
            //EmailBody = str.ReadToEnd();
            if (!string.IsNullOrEmpty(EmailBody) && EmailBody.Contains("<div id=\"bodyContent\">"))
            {
                string subjectHtml = "<h1 style=\"color:#1e1e2d; text-align:center;font-weight:500; margin:0;font-size:32px;font-family:\"Rubik\",sans-serif;\" id=\"subject\">" + subject + "</h1>";
                var newBody = EmailBody.Split("<div id=\"bodyContent\">");
                string newbody = newBody[0].ToString() + "<div id=\"bodyContent\">" + subjectHtml + bodyContent + newBody[1].ToString();
                EmailBody = newbody;
            }
            //str.Close();

            return EmailBody;
        }

        /*public static byte[] CompressImageMagickImage(IFormFile file, int targetSizeKB)
        {
            byte[] result = null;

            using (var image = new MagickImage(file.OpenReadStream()))
            {
                int quality = 75;
                var currentSizeKB = 0;

                MagickGeometry geometry = new MagickGeometry()
                {
                    IgnoreAspectRatio = false,
                    FillArea = false,
                    Width = image.Width,
                    Height = image.Height,
                };

                do
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    //image.FilterType = FilterType.Point;
                    image.Resize(geometry);
                    image.Quality = quality;
                    image.Write(ms);

                    currentSizeKB = (int)ms.Length / 1024;
                    quality -= 5;

                    result = ms.ToArray();
                    ms.Dispose();
                } while (currentSizeKB > targetSizeKB && quality >= 5);

                return result;
            }
        }*/

        //public static byte[] CompressSixLaborsImage(IFormFile file, int targetSizeKB)
        //{
        //    byte[] result = null;

        //    using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
        //    {
        //        int quality = 75;
        //        var currentSizeKB = 0;

        //        do
        //        {
        //            MemoryStream ms = new MemoryStream();
        //            ms.Position = 0;

        //            var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
        //            {
        //                Quality = quality
        //            };

        //            image.Save(ms, encoder);

        //            currentSizeKB = (int)ms.Length / 1024;
        //            quality -= 5;

        //            result = ms.ToArray();
        //            ms.Dispose();
        //        } while (currentSizeKB > targetSizeKB && quality >= 5);

        //        return result;
        //    }
        //}

        public static string CombinePathUrl(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }

        public static DateTime GetCalculateLastExpired(List<DateTime> date)
        {
            List<DateTime> tes = new List<DateTime>();
            DateTime returnDate = new DateTime();

            foreach (var dateItem in date)
            {
                if (dateItem != null)
                {
                    var dateNow = DateTime.UtcNow;
                    if (dateItem > dateNow)
                    {
                        if (tes.Count > 0)
                        {
                            if (dateItem > tes[0])
                            {
                                returnDate = dateItem;
                            }
                            else
                            {
                                returnDate = tes[0];
                            }
                        }
                        else
                        {
                            tes.Add(dateItem);
                            returnDate = tes[0];
                        }
                    }
                }
            }
            return returnDate;
        }
    }

    #region Custom Class
    public class ParamDatabasePagingParam
    {
        public int? limit { get; set; }
        public int? offset { get; set; }
        public string? order_column { get; set; }
        public string? order_method { get; set; }
        public string? filter { get; set; }
        public string? filter_column { get; set; }
    }

    public class TotalResult
    {
        public int total_record { get; set; }
    }

    public class PropertyFilter
    {
        public string @operator { get; set; }
        public bool filtration { get; set; }
        public string nameWithTable { get; set; }
    }

    public class CustomAttribute : Attribute
    {
        public string Name { get; }
        public CustomAttribute(string name)
        {
            Name = name;
        }
    }

    public class Email
    {
        //Email Config
        public string MailHost { get; set; }
        public int MailPort { get; set; }
        public string MailUsername { get; set; }
        public string MailPassword { get; set; }

        //Email Payload
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string Template { get; set; }
        public string UrlVerification { get; set; }
        public string CC { get; set; }
        public object AdditionalData { get; set; }
        public MemoryStream Attachment { get; set; }
        public string Attachment_name { get; set; }
    }
    public class CodeResult
    {
        public string generate_warranty_no { get; set; }
        public string generate_do_no { get; set; }
    }
    #endregion
}
