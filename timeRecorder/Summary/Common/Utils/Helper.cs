using Microsoft.Data.SqlClient;
using ScottPlot;
using ScottPlot.Renderable;
using Summary.Data;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
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
        public static ObservableCollection<string> TestCategory = new ObservableCollection<string>();
        public static List<Category> mainCategories = new List<Category>();
        public static Dictionary<string, int> categoryDic = new Dictionary<string, int>();
        public static Dictionary< int, string> IdCategoryDic = new Dictionary<int, string>();
        public static Dictionary<string, int> SummaryCategoryDic = new Dictionary<string, int>();
        public static List<Category> allcategories = new List<Category>();
        //public static RecordModel recordModel;

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
                            TimeViewObj startTimeObj = CreateNewTimeObj(tempStart, TimeObj.startTime, Helper.RestContent, currentDate, "none", lastIndex,height, taskId:TimeObj.taskId);
                            lastIndex++;
                            await SQLCommands.AddObj(startTimeObj);
                            UpdateColor(startTimeObj, "none");
                            currentDateTemplate.DailyObjs.Add(startTimeObj);
                        }
                    }
                    if (TimeObj.type==null)
                    {
                        TimeObj.type = "";
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
                    GeneratedToDoTask findTask = SQLCommands.QueryTodo(Helper.RestContent);
                    int taskId = findTask == null ? 0 : findTask.Id;
                    string type = "none";
                    foreach (var item in categoryDic)
                    {
                        if (item.Value == taskId)
                        {
                            type = item.Key;
                        }
                    }
                    TimeViewObj startTimeObj = CreateNewTimeObj(endTimeSpan, tempEndTime, RestContent, currentDate, type, lastIndex, height);
                    UpdateColor(startTimeObj, type);
                    await SQLCommands.AddObj(startTimeObj);
                    currentDateTemplate.DailyObjs.Add(startTimeObj);
                }
                currentDate = currentDate.AddDays(1);
                AllTimeViewObjs.Add(currentDateTemplate);
            }
            return AllTimeViewObjs;
        }
        public static Dictionary<string, string> colorDic = new Dictionary<string, string>();
        public static void UpdateColor(TimeViewObj timeViewObj, string type)
        {
            if (colorDic.ContainsKey(type))
            {
                timeViewObj.Color = colorDic[type.ToLower()];
            }
            else
            {
                timeViewObj.Color = colorDic["none"];
            }
        }
        public static TimeSpan getCurrentTime(){
            DateTime now = DateTime.Now;
            return new TimeSpan(now.Hour, now.Minute, now.Second);
        }
        
        public static TimeViewObj CreateNewTimeObj(TimeSpan startTime, TimeSpan endTime, string note, DateTime createDate, string type, int index,double height,string viewType = "summary",int taskId=0)
        {
            TimeViewObj TimeObj = new TimeViewObj();
            TimeObj.CreatedDate = createDate;
            TimeObj.LastTime = endTime - startTime;
            TimeObj.Note = note;
            TimeObj.Height = CalculateHeight(TimeObj.LastTime,height,viewType);
            TimeObj.StartTime = startTime;
            TimeObj.EndTime = endTime;
            TimeObj.Type = Helper.mainCategories.Any(x=>x.Name==type.ToLower())?type.ToLower():"none";
            TimeObj.Id = index;
            TimeObj.TaskId = taskId;
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
        public static bool CheckSysFontExisting(string fontName = "微软雅黑")
        {
            Font font;

            try
            {
                font = new Font(fontName, 10);
                if (font.Name != fontName)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public static void refreshPlot(IEnumerable<TimeViewObj> AllObj, WpfPlot plot)
        {
            var index = 0;
            plot.Plot.Clear();
            var plt = plot.Plot;
            var items = AllObj.GroupBy(x => new { x.Note, x.Type }).Select(x => new ChartBar { Note = x.Key.Note, Type = x.Key.Type, Time = new TimeSpan(x.Sum(x => x.LastTime.Ticks)) }).OrderBy(x => x.Type).ThenByDescending(x => x.Time);
            Dictionary<string,ChartBar[]> TypeItemList = new Dictionary<string, ChartBar[]>();
            var allItemCount = 0;
            foreach (string type in TestCategory)
            {
                if (type == "none") continue;
                var timeItems = items.Where(x => x.Type == type).ToArray();
                TypeItemList.Add(type, timeItems);
                allItemCount += timeItems.Length;
            }
            string[] TimeLabels = new string[allItemCount];
            string[] YLabels = new string[allItemCount];
            double[] position = new double[allItemCount];
            foreach (var TypeItems in TypeItemList)
            {
                if (TypeItems.Key == "none") continue;
                addChartData(TypeItems.Value, TypeItems.Key, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            }
            plt.YTicks(position, YLabels);
            plt.Legend(location: Alignment.UpperRight);
            Func<double, string> customFormatter = y => $"{TimeSpan.FromSeconds(y).ToString()}";
            plt.XAxis.TickLabelFormat(customFormatter);
            plt.YAxis.LabelStyle(fontSize: 14, fontName: CheckSysFontExisting()?"微软雅黑":"Microsoft YaHei");
            plt.XAxis.LabelStyle(fontSize: 14, fontName: CheckSysFontExisting()?"微软雅黑":"Microsoft YaHei");

            plt.YAxis.TickLabelStyle(fontSize: 14, fontName: CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            plt.XAxis.TickLabelStyle(fontSize: 14, fontName: CheckSysFontExisting()?"微软雅黑":"Microsoft YaHei");
            // adjust axis limits so there is no padding to the left of the bar graph
            plt.SetAxisLimits(xMin: 0);
            plot.Configuration.Quality = ScottPlot.Control.QualityMode.High;
            plot.Render(lowQuality: false);
            plot.Refresh();
        }
        private static void addChartData(ChartBar[] Items, string type, ref double[] position, ref string[] YLabels, ref string[] TimeLabels, ref Plot plt, ref int index)
        {
            if (Items.Count() > 0)
            {
                double[] itemPostion = new double[Items.Count()];
                double[] itemValues = new double[Items.Count()];
                //initColor();
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
        public static void initColor(ISQLCommands SqlCommands)
        {
            allcategories = SqlCommands.GetAllCategories().Result.ToList();
            mainCategories = allcategories.Where(x => x.ParentCategoryId==0).ToList();
            TestCategory.Clear();
            TestCategory.Add("none");
            colorDic.Clear();
            colorDic.Add("none", "#F3F3F3");
            IdCategoryDic.Clear();
            IdCategoryDic.Add(0, "none");
            categoryDic.Clear();
            categoryDic.Add("none", 0);
            foreach (var category in mainCategories)
            {
                //categoryDic为了后续快速获取这几个主要任务的id
                categoryDic.Add(category.Name, category.Id);
                colorDic.Add(category.Name, category.Color);
                IdCategoryDic.Add(category.Id, category.Name);
                TestCategory.Add(category.Name);
            }
        }
    }
}
