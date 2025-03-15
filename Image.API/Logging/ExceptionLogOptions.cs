using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Image.Logging
{
    public class ExceptionLogOptions : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.Filters.Add<ExceptionLogFilter>();
        }
    }
}