using System;

namespace WS_CRM.Helper
{
    public class SwaggerOrderByAttribute : Attribute
    {
        public Type ModelType { get; }

        public SwaggerOrderByAttribute(Type modelType)
        {
            ModelType = modelType;
        }
    }
}