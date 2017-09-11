using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using log4net;
using log4net.Config;

namespace Hey.Api.Rest.Exceptions
{
    public class LogExceptionHandler : IHeyExceptionHandler
    {
        private readonly ILog _log;

        public LogExceptionHandler()
        {
            _log = LogManager.GetLogger(GetType().Name);
        }

        public void Handle(Exception ex)
        {
            _log.Error(ex);
        }
    }
}