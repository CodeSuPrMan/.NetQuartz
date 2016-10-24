using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSoft.TimingSchedulerServer
{
    public class QuartzServerFactory
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(QuartzServerFactory));

        /// <summary>
        /// Creates a new instance of an Quartz.NET server core.
        /// </summary>
        /// <returns></returns>
        public static PMSoftTimingSchedulerServer CreateServer()
        {
            string typeName = Configuration.ServerImplementationType;

            Type t = Type.GetType(typeName, true);

            logger.Debug("Creating new instance of server type '" + typeName + "'");
            PMSoftTimingSchedulerServer retValue = (PMSoftTimingSchedulerServer)Activator.CreateInstance(t);
            logger.Debug("Instance successfully created");
            return retValue;
        }
    }
}
