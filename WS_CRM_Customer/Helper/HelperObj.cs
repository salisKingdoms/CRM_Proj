using System.Reflection;

namespace WS_CRM_Customer.Helper
{
    public class HelperObj
    {
        public static void Copy<TParent, TChild>(TParent parent, TChild child) where TParent : class where TChild : class
        {
            PropertyInfo[] properties = parent.GetType().GetProperties();
            PropertyInfo[] properties2 = child.GetType().GetProperties();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                PropertyInfo[] array2 = properties2;
                foreach (PropertyInfo propertyInfo2 in array2)
                {
                    if (propertyInfo.Name == propertyInfo2.Name && propertyInfo.PropertyType == propertyInfo2.PropertyType)
                    {
                        propertyInfo2.SetValue(child, propertyInfo.GetValue(parent));
                        break;
                    }
                }
            }
        }

        public static Tresult convert<TParent, Tresult>(TParent parent) where TParent : class where Tresult : new()
        {
            PropertyInfo[] properties = parent.GetType().GetProperties();
            Tresult val = new Tresult();
            PropertyInfo[] properties2 = val.GetType().GetProperties();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                PropertyInfo[] array2 = properties2;
                foreach (PropertyInfo propertyInfo2 in array2)
                {
                    if (propertyInfo.Name == propertyInfo2.Name && propertyInfo.PropertyType == propertyInfo2.PropertyType)
                    {
                        propertyInfo2.SetValue(val, propertyInfo.GetValue(parent));
                        break;
                    }
                }
            }

            return val;
        }
    }
}

