
using log4net;
using PMSoft.Common;
using PMSoft.Dust.Lib;
using PMSoft.Dust.Lib.Enums;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSoft.TimingSchedulerServer.Job
{
    /// <summary>
    /// 15分钟定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics15Minute:IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics15Minute));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime endtime = DateTime.Now;
                DateTime begintime = endtime.AddMinutes(-15);
                string dbpath = ConfigurationManager.AppSettings["DBPath"];
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<TimeIntervalRunData> rundata = IntervalRunDataCalc.GetIntervalRunData(begintime, endtime, eValidTimeType.Miniute, dbpath, AQIpath).Result;
                foreach (TimeIntervalRunData rundatacalc in rundata)
                {
                    mMinutesAvg insertmodelPMTen = new mMinutesAvg();
                    insertmodelPMTen.AirQuality = rundatacalc.AQI;
                    insertmodelPMTen.AvgConcentration = rundatacalc.AveragePMTen;
                    insertmodelPMTen.DID = rundatacalc.DID;
                    insertmodelPMTen.EffectiveTimeInterval = rundatacalc.ValidTime;
                    insertmodelPMTen.GrainType = eGrainType.PMTen.ToString();
                    insertmodelPMTen.IAQI = rundatacalc.PMTenIAQI;
                    insertmodelPMTen.MaxValue = rundatacalc.MaxPMTen;
                    insertmodelPMTen.MinValue = rundatacalc.MinPMTen;
                    insertmodelPMTen.RegionalAvgConcentration = rundatacalc.AveragePMTenConcentrationOfLocation;
                    insertmodelPMTen.StartTime = begintime;
                    insertmodelPMTen.EndTime = endtime;
                    int ten = new dMinutesAvg().Add(insertmodelPMTen);

                    mMinutesAvg insertmodelPMTwoPointFive = new mMinutesAvg();
                    insertmodelPMTwoPointFive.AirQuality = rundatacalc.AQI;
                    insertmodelPMTwoPointFive.AvgConcentration = rundatacalc.AveragePMTwoPointFive;
                    insertmodelPMTwoPointFive.DID = rundatacalc.DID;
                    insertmodelPMTwoPointFive.EffectiveTimeInterval = rundatacalc.ValidTime;
                    insertmodelPMTwoPointFive.GrainType = eGrainType.PMTwoPointFive.ToString();
                    insertmodelPMTwoPointFive.IAQI = rundatacalc.PMTwoPointFiveIAQI;
                    insertmodelPMTwoPointFive.MaxValue = rundatacalc.MaxPMTwoPointFive;
                    insertmodelPMTwoPointFive.MinValue = rundatacalc.MinPMTwoPointFive;
                    insertmodelPMTwoPointFive.RegionalAvgConcentration = rundatacalc.AveragePMTwoPointFiveConcentrationOfLocation;
                    insertmodelPMTwoPointFive.StartTime = begintime;
                    insertmodelPMTwoPointFive.EndTime = endtime;
                    int TwoPointFive = new dMinutesAvg().Add(insertmodelPMTwoPointFive);

                    mMinutesAvg insertmodelTSP = new mMinutesAvg();
                    insertmodelTSP.AirQuality = rundatacalc.AQI;
                    insertmodelTSP.AvgConcentration = rundatacalc.AverageTSP;
                    insertmodelTSP.DID = rundatacalc.DID;
                    insertmodelTSP.EffectiveTimeInterval = rundatacalc.ValidTime;
                    insertmodelTSP.GrainType = eGrainType.TSP.ToString();
                    insertmodelTSP.IAQI = 0;
                    insertmodelTSP.MaxValue = rundatacalc.MaxTSP;
                    insertmodelTSP.MinValue = rundatacalc.MinTSP;
                    insertmodelTSP.RegionalAvgConcentration = rundatacalc.AverageTSPConcentrationOfLocation;
                    insertmodelTSP.StartTime = begintime;
                    insertmodelTSP.EndTime = endtime;
                    int TSP = new dMinutesAvg().Add(insertmodelTSP);
                }
            }
            catch (Exception ex)
            {

                logger.Error("15分组统计发生错误" + ex.Message);
            }
        }
    }
    /// <summary>
    /// 1小时定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics1Hour : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics1Hour));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime endtime = DateTime.Now;
                DateTime begintime = endtime.AddHours(-1);
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<StatisticsRunData> statistiscdata = IntervalRunDataCalc.GetStatisticsRunData(begintime, endtime, "MinutesAvg", AQIpath, false);
                dHourAvg dal = new dHourAvg();
                foreach (StatisticsRunData m in statistiscdata)
                {
                    mHourAvg model = new mHourAvg();
                    BaseLib.ToObject(m, model);
                    dal.Add(model);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("1小时统计发生错误" + ex.Message);
            }
        }
    }
    /// <summary>
    /// 8小时定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics8Hour : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics8Hour));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime endtime = DateTime.Now;
                DateTime begintime = endtime.AddHours(-8);
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<StatisticsRunData> statistiscdata = IntervalRunDataCalc.GetStatisticsRunData(begintime, endtime, "MinutesAvg", AQIpath, true);
                dEightHoursAvg dal = new dEightHoursAvg();
                foreach (StatisticsRunData m in statistiscdata)
                {
                    mEightHoursAvg model = new mEightHoursAvg();
                    BaseLib.ToObject(m, model);
                    dal.Add(model);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("8小时统计发生错误" + ex.Message);
            }
        }
    }
    /// <summary>
    /// 24小时定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics24Hour : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics24Hour));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime endtime = DateTime.Now;
                DateTime begintime = endtime.AddHours(-24);
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<StatisticsRunData> statistiscdata = IntervalRunDataCalc.GetStatisticsRunData(begintime, endtime, "MinutesAvg", AQIpath, false);
                dTwentyFourHoursAvg dal = new dTwentyFourHoursAvg();
                foreach (StatisticsRunData m in statistiscdata)
                {
                    mTwentyFourHoursAvg model = new mTwentyFourHoursAvg();
                    BaseLib.ToObject(m, model);
                    dal.Add(model);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("24小时统计发生错误" + ex.Message);
            }
        }
    }

    /// <summary>
    /// 月度定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics1Month : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics1Month));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                
                DateTime endtime = DateTime.Now;
                DateTime begintime = endtime.AddMonths(-1);
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<StatisticsRunData> statistiscdata = IntervalRunDataCalc.GetStatisticsRunData(begintime, endtime, "MinutesAvg", AQIpath, true);
                dMonthAvg dal = new dMonthAvg();
                foreach (StatisticsRunData m in statistiscdata)
                {
                    mMonthAvg model = new mMonthAvg();
                    BaseLib.ToObject(m, model);
                    dal.Add(model);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("月度统计发生错误" + ex.Message);
            }
        }
    }
    /// <summary>
    /// 季度定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics1Quarter : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics1Quarter));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime begintime = DateTime.Now;
                DateTime endtime = begintime.AddMonths(-3);
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<StatisticsRunData> statistiscdata = IntervalRunDataCalc.GetStatisticsRunData(begintime, endtime, "MinutesAvg", AQIpath, false);
                dQuarterAvg dal = new dQuarterAvg();
                foreach (StatisticsRunData m in statistiscdata)
                {
                    mQuarterAvg model = new mQuarterAvg();
                    BaseLib.ToObject(m, model);
                    dal.Add(model);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("季度统计发生错误" + ex.Message);
            }
        }
    }
    /// <summary>
    /// 年度定时统计任务
    /// </summary>
    public sealed class TimeIntervalStatistics1Year : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TimeIntervalStatistics1Year));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime begintime = DateTime.Now;
                DateTime endtime = begintime.AddYears(-1);
                string AQIpath = ConfigurationManager.AppSettings["AQIParamXMLPath"];
                List<StatisticsRunData> statistiscdata = IntervalRunDataCalc.GetStatisticsRunData(begintime, endtime, "MinutesAvg", AQIpath, false);
                dYearAvg dal = new dYearAvg();
                foreach (StatisticsRunData m in statistiscdata)
                {
                    mYearAvg model = new mYearAvg();
                    BaseLib.ToObject(m, model);
                    dal.Add(model);
                }
            }
            catch (Exception ex)
            {
                
                logger.Error("年度统计发生错误" + ex.Message);
            }
        }
    }



}
