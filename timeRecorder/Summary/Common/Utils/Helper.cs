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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Summary.Common.Utils
{
    public static class Helper
    {
        public static string RestContent ="";
        public static TimeSpan GlobalStartTimeSpan = new TimeSpan(6, 0, 0);
        public static TimeSpan GlobalEndTimeSpan = new TimeSpan(23, 59, 59);
        public static TimeSpan intervalRemindTimeSpan = new TimeSpan(0,2,0);
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
        public static bool displayInvisibleItems = false;
        public static Dictionary<string, int> NameIdDic = new Dictionary<string, int>();
        //public static RecordModel recordModel;
        public static int getMaxDepth(int currDepth, int findCategoryId)
        {
            List<Category> allSubCategories = new List<Category>();
            if (Helper.displayInvisibleItems == true)
            {
                allSubCategories = Helper.allcategories.Where(x => x.ParentCategoryId == findCategoryId).ToList();
            }
            else
            {
                allSubCategories = Helper.allcategories.Where(x => x.ParentCategoryId == findCategoryId && x.Visible).ToList();
            }

            if (allSubCategories.Count == 0) return currDepth;
            int a = currDepth;
            foreach (var category in allSubCategories)
            {
                currDepth = Math.Max(getMaxDepth(a+1, category.Id), currDepth);
            }
            return currDepth;
        }
        public static long GetAllSubTime(List<ToDoObj> plotData, string currcategory)
        {
            if (!Helper.NameIdDic.ContainsKey(currcategory)) return 0;
            var tempSubCategories = allcategories.Where(x => x.ParentCategoryId == Helper.NameIdDic[currcategory]).ToList();
            long res = 0;
            if (tempSubCategories.Count > 0)
            {
                foreach (var category in tempSubCategories)
                {
                    res+=GetAllSubTime(plotData, category.Name);
                }
            }
            else
            {
                return plotData.Where(x => x.CategoryId == Helper.NameIdDic[currcategory]).Sum(x => x.LastTime.Ticks);
            }
            res += plotData.Where(x => x.CategoryId == Helper.NameIdDic[currcategory]).Sum(x => x.LastTime.Ticks);
            return res;
        }
        public static async Task RBChanged(object obj, WpfPlot CategoryPlot, ISQLCommands SQLCommands,string Category, List<ToDoObj> allTasks)
        {
            int param = int.Parse(obj.ToString());
            var index = 0;
            CategoryPlot.Plot.Clear();
            var plt = CategoryPlot.Plot;
            Dictionary<string, int> typelevelDic = new Dictionary<string, int>();
            colorDic.Clear();
            allcategories = await SQLCommands.GetAllCategories();
            int findCategoryId = Helper.allcategories.FirstOrDefault(x => x.Name == Category, new Data.Category()).Id;
            NameIdDic.Clear();
            IdCategoryDic.Clear();
            IdCategoryDic.Add(0, "none");
            NameIdDic.Add("none", 0);
            colorDic.Add("none", "#F3F3F3");
            foreach (Category category in allcategories)
            {
                if (!category.Visible) { continue; }
                NameIdDic.Add(category.Name, category.Id);
                IdCategoryDic.Add(category.Id, category.Name);
                colorDic.Add(category.Name, category.Color);
                Category tempCate = category;
                int level = 0;
                if (tempCate.Id == findCategoryId)
                {
                    typelevelDic.Add(category.Name, level);
                }
                while (allcategories.FirstOrDefault(x => x.Id == tempCate.Id, new Data.Category() { Id= findCategoryId }).Id!=findCategoryId|| allcategories.FirstOrDefault(x => x.Id == tempCate.Id, new Data.Category() { Id = 0 }).Id != 0)
                {
                    level++;
                    tempCate = allcategories.FirstOrDefault(x => x.Id == tempCate.ParentCategoryId, new Data.Category() { ParentCategoryId= findCategoryId });
                    if (tempCate.Id==findCategoryId)
                        typelevelDic.Add(category.Name, level);
                }
            }
            List<ToDoObj> plotData = allTasks.Where(x => typelevelDic.ContainsKey(x.Category)).ToList();
            int maxDepth = Helper.getMaxDepth(1, findCategoryId);
            foreach (ToDoObj task in plotData)
            {
                typelevelDic.Add("task:" + task.Note, maxDepth);
                NameIdDic.Add("task:" + task.Note, 0);
                colorDic.Add("task:" + task.Note, colorDic[IdCategoryDic[task.CategoryId]]);
            }
            typelevelDic = typelevelDic.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            Dictionary<string, long> plotDataFinal = new Dictionary<string, long>();

            foreach (var item in typelevelDic)
            {
                if (item.Value>=param)
                {
                    //parentCate=invest tempCategories=[invest, Timerecorder, learn...]
                    var tempSubCategories = allcategories.Where(x => x.ParentCategoryId==NameIdDic[item.Key]||x.Id== NameIdDic[item.Key]).Select(x => new { x.Id, x.Name }).ToList();
                    long sumLastTime = 0;
                    if (item.Value==maxDepth)
                    {
                        sumLastTime = plotData.First(x => ("task:" + x.Note) == item.Key).LastTime.Ticks;
                    }
                    else
                    {
                        //sumLastTime = plotData.Where(x => tempSubCategories.Any(y => y.Id == x.CategoryId)).Sum(x => x.LastTime.Ticks) ;
                        sumLastTime = GetAllSubTime(plotData, item.Key);
                    }

                    plotDataFinal.Add(item.Key, sumLastTime);
                }
            }
            List<int> allTypes = typelevelDic.Where(x => x.Value==param).Select(x => NameIdDic[x.Key]).ToList();
            plotDataFinal = plotDataFinal.Where(x => allTypes.Contains(NameIdDic[x.Key])).ToDictionary(x => x.Key, x => x.Value);
            var items = plotDataFinal.Select(x => new ChartBar { Note = x.Key, Type = x.Key, Time = new TimeSpan(x.Value) }).OrderBy(x => x.Type).ThenByDescending(x => x.Time);
            Dictionary<string, ChartBar[]> TypeItemList = new Dictionary<string, ChartBar[]>();
            var allItemCount = 0;
            foreach (string type in plotDataFinal.Keys)
            {
                //if (type == "none") continue;
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
            Func<double, string> customFormatter = y => double.IsNaN(y) ? "0" : $"{TimeSpan.FromSeconds(y).ToString()}";
            plt.XAxis.TickLabelFormat(customFormatter);
            plt.YAxis.LabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            plt.XAxis.LabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");

            plt.YAxis.TickLabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            plt.XAxis.TickLabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            // adjust axis limits so there is no padding to the left of the bar graph
            plt.SetAxisLimits(xMin: 0);
            CategoryPlot.Configuration.Quality = ScottPlot.Control.QualityMode.High;
            CategoryPlot.Dispatcher.Invoke(new Action(delegate
            {
                CategoryPlot.Render(lowQuality: false);
                CategoryPlot.Refresh();
            }));
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
                List<MyTime> currentDateData = allTimeData.Where(x => x.createDate==currentDate&&x.endTime>GlobalStartTimeSpan).OrderBy(s => s.startTime).ToList<MyTime>();
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
                        TimeSpan tempStart = Helper.GlobalStartTimeSpan;
                        if (startTimeSpan > tempStart)
                        {
                            //if cannot find
                            GeneratedToDoTask findTask = SQLCommands.QueryTodo(Helper.RestContent);
                            int taskId = findTask==null ? 0 : findTask.Id;
                            string type = IdCategoryDic[findTask!=null?findTask.TypeId:0];
                            TimeViewObj startTimeObj = CreateNewTimeObj(tempStart, TimeObj.startTime, Helper.RestContent, currentDate, type, lastIndex,height, taskId: taskId);
                            await SQLCommands.AddObj(startTimeObj);
                            UpdateColor(startTimeObj, type);
                            currentDateTemplate.DailyObjs.Add(startTimeObj);
                        }
                        else if(startTimeSpan < tempStart)
                        {
                            //split first time obj into 2 time objs, delete the item and add the second time obj to the view
                            TimeViewObj firstSplitItem = CreateNewTimeObj(startTimeSpan, tempStart, TimeObj.note, currentDate, TimeObj.type, lastIndex, height, taskId: TimeObj.taskId);
                            await SQLCommands.AddObj(firstSplitItem);
                            UpdateColor(firstSplitItem, TimeObj.type);
                            TimeViewObj secondSplitItem = CreateNewTimeObj(tempStart, TimeObj.endTime, TimeObj.note, currentDate, TimeObj.type, lastIndex, height, taskId: TimeObj.taskId);
                            await SQLCommands.AddObj(secondSplitItem);
                            UpdateColor(secondSplitItem, TimeObj.type);
                            await SQLCommands.DeleteObj(TimeObj);
                            currentDateTemplate.DailyObjs.Add(secondSplitItem);
                            continue;
                        }
                        //if they are equal, don't do anything. Directly add the obj
                    }
                    if (TimeObj.type==null)
                    {
                        TimeObj.type = "";
                    }
                    if (startTimeSpan>=Helper.GlobalStartTimeSpan)
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
                    int taskId = findTask==null ? 0 : findTask.Id;
                    string type = IdCategoryDic[findTask!=null?findTask.TypeId:0];
                    TimeViewObj startTimeObj = CreateNewTimeObj(endTimeSpan, tempEndTime, RestContent, currentDate, type, lastIndex, height, taskId: taskId);
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
            TimeSpan allTimeSpan = GlobalEndTimeSpan-GlobalStartTimeSpan;
            if (viewType == "record")
            {
                allTimeSpan = Helper.getCurrentTime() - GlobalStartTimeSpan;
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
            foreach (var category in allcategories)
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
