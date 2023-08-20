using ScottPlot;
using Summary.Data;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Summary.Common.Utils
{
    public static class Helper
    {
        public const string RestContent = "休息";
        public static TimeSpan GlobalStartTimeSpan = new TimeSpan(6, 0, 0);
        public static TimeSpan GlobalEndTimeSpan = new TimeSpan(23, 59, 59);
        public static TimeSpan TickTime;
        public static string WorkContent;
        public static bool WorkMode;
        public static bool MiniWindowShow = false;
        public static RecordModel recordModel;
        public static TimeType ConvertTimeType(string type)
        {
            return (TimeType)Enum.Parse(typeof(TimeType), type);
        }
        public static string GetAppSetting(string key)
        {
            var cfg = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            string value = cfg.AppSettings.Settings[key].Value;
            return value;
        }
        public static void SetAppSetting(string key,string value)
        {
            var cfg = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            cfg.AppSettings.Settings[key].Value = value;
            cfg.Save();
        }
        public static async Task<ObservableCollection<GridSourceTemplate>> BuildTimeViewObj(DateTime startTime,DateTime endTime,ISQLCommands SQLCommands,double height,string viewType = "summary")
        {
            DateTime currentDate = startTime;
            ObservableCollection<GridSourceTemplate> AllTimeViewObjs = new ObservableCollection<GridSourceTemplate>();
            List<MyTime> allTimeData = await SQLCommands.GetAllTimeObjs(startTime, endTime);
            while (allTimeData!=null && currentDate <= endTime)
            {
                int lastIndex = 1;
                var currentDateTemplate = new GridSourceTemplate(currentDate);

                currentDateTemplate.Title = currentDate.ToShortDateString();
                currentDateTemplate.Week = currentDate.DayOfWeek.ToString();

                if (currentDateTemplate.Week.Equals("Saturday")||currentDateTemplate.Week.Equals("Sunday"))
                {
                    currentDateTemplate.Color = "#32CD32";
                }
                else
                {
                    currentDateTemplate.Color = "#008080";
                }
                TimeSpan endTimeSpan = GlobalStartTimeSpan;
                List<MyTime> currentDateData = allTimeData.Where(x => x.createDate==currentDate&&x.startTime>=endTimeSpan).OrderBy(s => s.startTime).ToList<MyTime>();
                bool firstTimeObj = true;
                if (currentDateData.Count>0)
                {
                    lastIndex = currentDateData.Max(x => x.currentIndex)+1;
                }
                foreach (MyTime TimeObj in currentDateData)
                {
                    TimeViewObj timeViewObj = new TimeViewObj();
                    TimeSpan startTimeSpan = TimeSpan.Parse(TimeObj.startTime.ToString());
                    endTimeSpan = TimeSpan.Parse(TimeObj.endTime.ToString());
                    if (firstTimeObj)
                    {
                        firstTimeObj = false;
                        //Add first time object
                        TimeSpan tempStart = new TimeSpan(6, 0, 0);
                        if (startTimeSpan > tempStart)
                        {
                            TimeViewObj startTimeObj = CreateNewTimeObj(tempStart, TimeObj.startTime, "nothing", currentDate, TimeType.none, lastIndex,height);
                            lastIndex++;
                            await SQLCommands.AddObj(startTimeObj);
                            UpdateColor(startTimeObj, "none");
                            currentDateTemplate.DailyObjs.Add(startTimeObj);
                        }
                    }
                    if (startTimeSpan>=new TimeSpan(6, 0, 0))
                    {
                        timeViewObj.CreatedDate = currentDate;
                        timeViewObj.LastTime = TimeObj.endTime-TimeObj.startTime;
                        timeViewObj.Note = TimeObj.note;
                        timeViewObj.Height = CalculateHeight(endTimeSpan - startTimeSpan,height, viewType);
                        timeViewObj.StartTime = TimeObj.startTime;
                        timeViewObj.EndTime = TimeObj.endTime;
                        timeViewObj.Type = TimeObj.type.Trim()=="" ? "none" : TimeObj.type.Trim();
                        timeViewObj.Id = TimeObj.currentIndex;
                        UpdateColor(timeViewObj, TimeObj.type.Trim().ToLower());
                    }
                    currentDateTemplate.DailyObjs.Add(timeViewObj);
                }
                //Add last obj
                //Add first time object
                TimeSpan tempEndTime = GlobalEndTimeSpan;
                if (endTimeSpan < tempEndTime && currentDate<DateTime.Today)
                {
                    TimeViewObj startTimeObj = CreateNewTimeObj(endTimeSpan, tempEndTime, "nothing", currentDate, TimeType.none, lastIndex, height);
                    UpdateColor(startTimeObj, "none");
                    await SQLCommands.AddObj(startTimeObj);
                    currentDateTemplate.DailyObjs.Add(startTimeObj);
                }
                currentDate = currentDate.AddDays(1);
                AllTimeViewObjs.Add(currentDateTemplate);
            }
            return AllTimeViewObjs;
        }
        public static Dictionary<TimeType, string> colorDic = new Dictionary<TimeType, string>();
        public static void UpdateColor(TimeViewObj timeViewObj, string type)
        {
            switch (type.ToLower())
            {
                case "study":
                    timeViewObj.Color = "#FFB6C1";
                    break;
                case "waste":
                    timeViewObj.Color = "#F08080";
                    break;
                case "rest":
                    timeViewObj.Color = "#98FB98";
                    break;
                case "work":
                    timeViewObj.Color = "#FFD700";
                    break;
                case "none":
                    timeViewObj.Color = "#F3F3F3";
                    break;
                case "play":
                    timeViewObj.Color = "#ADD8E6";
                    break;
            }
        }
        public static TimeSpan getCurrentTime(){
            DateTime now = DateTime.Now;
            return new TimeSpan(now.Hour, now.Minute, now.Second);
        }
        public static TimeViewObj CreateNewTimeObj(TimeSpan startTime, TimeSpan endTime, string note, DateTime createDate, TimeType type, int index,double height,string viewType = "summary")
        {
            TimeViewObj TimeObj = new TimeViewObj();
            TimeObj.CreatedDate = createDate;
            TimeObj.LastTime = endTime - startTime;
            TimeObj.Note = note;
            TimeObj.Height = CalculateHeight(TimeObj.LastTime,height,viewType);
            TimeObj.StartTime = startTime;
            TimeObj.EndTime = endTime;
            TimeObj.Type = type.ToString().ToLower();
            TimeObj.Id = index;
            return TimeObj;
        }
        public static double CalculateHeight(TimeSpan lastTime, double height, string viewType = "summary")
        {
            TimeSpan allTimeSpan = new TimeSpan(18, 0, 0);
            if (viewType == "record")
            {
                allTimeSpan = Helper.getCurrentTime() - new TimeSpan(6, 0, 0);
                return lastTime/allTimeSpan*(height-90);
            }
            return lastTime/allTimeSpan*(height-100);
        }
        public static void refreshPlot(IEnumerable<TimeViewObj> AllObj, WpfPlot plot)
        {
            var items = AllObj.GroupBy(x => new { x.Note, x.Type }).Select(x => new ChartBar { Note = x.Key.Note, Type = x.Key.Type, Time = new TimeSpan(x.Sum(x => x.LastTime.Ticks)) }).OrderBy(x => x.Type).ThenByDescending(x => x.Time);
            var studyItems = items.Where(x => x.Type == "study").ToArray();
            var wasteItems = items.Where(x => x.Type == "waste").ToArray();
            var workItems = items.Where(x => x.Type == "work").ToArray();
            var restItems = items.Where(x => x.Type == "rest").ToArray();
            var playItems = items.Where(x => x.Type == "play").ToArray();
            var index = 0;
            plot.Plot.Clear();
            var plt = plot.Plot;
            var allItemCount = studyItems.Length + wasteItems.Length + workItems.Length + restItems.Length + playItems.Length;
            string[] TimeLabels = new string[allItemCount];
            string[] YLabels = new string[allItemCount];
            double[] position = new double[allItemCount];
            addChartData(studyItems, TimeType.study, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(workItems, TimeType.work, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(wasteItems, TimeType.waste, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(restItems, TimeType.rest, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(playItems, TimeType.play, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);

            plt.YTicks(position, YLabels);
            plt.Legend(location: Alignment.UpperRight);
            Func<double, string> customFormatter = y => $"{TimeSpan.FromSeconds(y).ToString()}";
            plt.XAxis.TickLabelFormat(customFormatter);
            // adjust axis limits so there is no padding to the left of the bar graph
            plt.SetAxisLimits(xMin: 0);
            plot.Refresh();
        }
        private static void addChartData(ChartBar[] Items, TimeType type, ref double[] position, ref string[] YLabels, ref string[] TimeLabels, ref Plot plt, ref int index)
        {
            if (Items.Count() > 0)
            {
                double[] itemPostion = new double[Items.Count()];
                double[] itemValues = new double[Items.Count()];
                initColor();
                for (int i = 0; i < Items.Count(); i++)
                {
                    itemPostion[i] = index + i + 1;
                    position[i + index] = index + i + 1;
                    YLabels[i + index] = Items[i].Note;
                    itemValues[i] = Items[i].Time.TotalSeconds;
                    TimeLabels.Append(Items[i].Time.ToString());
                }
                var bar = plt.AddBar(itemValues, itemPostion, System.Drawing.ColorTranslator.FromHtml(colorDic[type]));
                bar.Orientation = ScottPlot.Orientation.Horizontal;
                bar.ShowValuesAboveBars = true;
                Func<double, string> customFormatter = y => $"{TimeSpan.FromSeconds(y).ToString()}";
                bar.ValueFormatter = customFormatter;
                index = index + Items.Count();
            }
        }
        public static void initColor(){
            if (colorDic.Count == 0)
            {
                colorDic.Add(TimeType.none, "#F3F3F3");
                colorDic.Add(TimeType.study, "#FFB6C1");
                colorDic.Add(TimeType.waste, "#F08080");
                colorDic.Add(TimeType.rest, "#98FB98");
                colorDic.Add(TimeType.work, "#FFD700");
                colorDic.Add(TimeType.play, "#ADD8E6");
            }
        }
    }
}
