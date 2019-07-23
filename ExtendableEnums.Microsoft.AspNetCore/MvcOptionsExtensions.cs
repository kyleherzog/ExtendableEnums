using Microsoft.AspNetCore.Mvc;

namespace ExtendableEnums.Microsoft.AspNetCore
{
    public static class MvcOptionsExtensions
    {
        public static void UseExtendableEnumModelBinding(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new ExtendableEnumModelBinderProvider());
        }
    }
}
