using Summary.Data;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Summary.Common.Utils
{
    public static class Helper
    {
        public static async Task<ObservableCollection<GridSourceTemplate>> BuildTimeViewObj(DateTime startTime,DateTime endTime,ISQLCommands SQLCommands,double height)
        {
            DateTime currentDate = startTime;
            ObservableCollection<GridSourceTemplate> AllTimeViewObjs = new ObservableCollection<GridSourceTemplate>();
            List<MyTime> allTimeData = await SQLCommands.GetAllTimeObjs(startTime, endTime);
            while (currentDate<=endTime)
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
                TimeSpan endTimeSpan = new TimeSpan(6, 0, 0);
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
                            TimeViewObj startTimeObj = CreateNewTimeObj(tempStart, TimeObj.startTime, "nothing", currentDate, TimeType.None, lastIndex,height);
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
                        timeViewObj.Height = CalculateHeight(endTimeSpan - startTimeSpan,height);
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
                TimeSpan tempEndTime = new TimeSpan(23, 59, 59);
                if (endTimeSpan < tempEndTime && currentDate<DateTime.Today)
                {
                    TimeViewObj startTimeObj = CreateNewTimeObj(endTimeSpan, tempEndTime, "nothing", currentDate, TimeType.None, lastIndex, height);
                    UpdateColor(startTimeObj, "none");
                    await SQLCommands.AddObj(startTimeObj);
                    currentDateTemplate.DailyObjs.Add(startTimeObj);
                }
                currentDate = currentDate.AddDays(1);
                AllTimeViewObjs.Add(currentDateTemplate);
            }
            return AllTimeViewObjs;
        }
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
        public static TimeViewObj CreateNewTimeObj(TimeSpan startTime, TimeSpan endTime, string note, DateTime createDate, TimeType type, int index,double height)
        {
            TimeViewObj TimeObj = new TimeViewObj();
            TimeObj.CreatedDate = createDate;
            TimeObj.LastTime = endTime - startTime;
            TimeObj.Note = note;
            TimeObj.Height = CalculateHeight(TimeObj.LastTime,height);
            TimeObj.StartTime = startTime;
            TimeObj.EndTime = endTime;
            TimeObj.Type = type.ToString().ToLower();
            TimeObj.Id = index;
            return TimeObj;
        }
        public static double CalculateHeight(TimeSpan lastTime,double height)
        {
            TimeSpan allTimeSpan = new TimeSpan(18, 0, 0);
            return lastTime/allTimeSpan*(height-100);
        }
    }
}
