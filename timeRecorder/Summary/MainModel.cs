using MaterialDesignThemes.Wpf;
using Summary.Common;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Threading.Tasks;
using Summary.Domain;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace Summary
{
    public class MainModel:ViewModelBase
    {
        private DateTime startTime;

		public DateTime StartTime
		{
			get { return startTime; }
			set { startTime = value; OnPropertyChanged();  }
		}
		private DateTime endTime;

		public DateTime EndTime
		{
			get { return endTime; }
			set { endTime = value; OnPropertyChanged(); }
		}
		private double leftPanelHeight;
		public double LeftPanelHeight
        {
			get { return leftPanelHeight; }
			set { leftPanelHeight = value;  OnPropertyChanged(); }
		}
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set { _IsDialogOpen = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GridSourceTemplate> allTimeViewObjs;

		public ObservableCollection<GridSourceTemplate> AllTimeViewObjs
		{
			get { return allTimeViewObjs; }
			set { allTimeViewObjs = value; OnPropertyChanged(); }
		}
		
		public MyCommand ClickOkButtonCommand { get; set; }
        public MyCommand SelectedTest { get; set; }
        public ISQLCommands SQLCommands { get; set; }
        public MainModel(ISQLCommands SqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
			EndTime = DateTime.Today;
            StartTime = DateTime.Today.AddDays(-7);
			SQLCommands = SqlCommands;
            SelectedCommand = new MyCommand(Selected);
        }

       

        private TimeViewObj selectedTimeObj;

		public TimeViewObj SelectedTimeObj
		{
			get { return selectedTimeObj; }
			set { selectedTimeObj = value;OnPropertyChanged(); }
		}

        private void Selected(object obj)
        {
            TimeViewObj myTimeView = (TimeViewObj)obj;
            SelectedTimeObj = myTimeView;
        }
        public MyCommand SelectedCommand { get; set; }

        private void  closeDialog()
		{
            IsDialogOpen=false;
			
        }
        private void openDialog()
        {
            IsDialogOpen=true;
           
        }
        private async void clickOkButton(object a) 
        {
            await Task.Run(() => { openDialog(); }).ContinueWith(delegate { showTimeView(); }).ContinueWith(delegate { closeDialog(); });
           
        }
        private  void showTimeView()
		{
             List<MyTime> allTimeData =  SQLCommands.GetAllTimeObjs(startTime, endTime);
             AllTimeViewObjs = BuildTimeViewObj(allTimeData);
        }
       
        private ObservableCollection<GridSourceTemplate> BuildTimeViewObj(List<MyTime> allTimeData)
        {
			DateTime currentDate = startTime;
            ObservableCollection<GridSourceTemplate> AllTimeViewObjs = new ObservableCollection<GridSourceTemplate>();

            while (currentDate<=endTime)
			{
				var currentDateTemplate = new GridSourceTemplate();
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
                 List<MyTime> currentDateData = allTimeData.Where(x => x.createDate==currentDate).OrderBy(s=>s.startTime).ToList<MyTime>();
                bool firstTimeObj = true;
				foreach(MyTime TimeObj in currentDateData)
				{
                    
                    TimeViewObj timeViewObj = new TimeViewObj();
					TimeSpan startTime = TimeSpan.Parse(TimeObj.startTime.ToString());
                    TimeSpan endTime = TimeSpan.Parse(TimeObj.endTime.ToString());
                    if (firstTimeObj)
                    {
                        firstTimeObj = false;
                        TimeSpan tempStart = new TimeSpan(6, 0, 0);
                        if (startTime > tempStart)
                        {
                            TimeViewObj startTimeObj = new TimeViewObj();
                            startTimeObj.CreatedDate = currentDate;
                            startTimeObj.LastTime = TimeObj.startTime - tempStart;
                            startTimeObj.Note = "nothing";
                            startTimeObj.Height = CalculateHeight(startTimeObj.LastTime);
                            startTimeObj.StartTime = tempStart;
                            startTimeObj.EndTime = TimeObj.startTime;
                            startTimeObj.Type = "none";
                            currentDateTemplate.DailyObjs.Add(startTimeObj);
                        }
                    }
                    if (startTime>new TimeSpan(6, 0, 0))
					{
						timeViewObj.CreatedDate = currentDate;
                        timeViewObj.LastTime = TimeObj.lastTime;
                        timeViewObj.Note = TimeObj.note;
						timeViewObj.Height = CalculateHeight(endTime - startTime);
                        timeViewObj.StartTime = TimeObj.startTime;
                        timeViewObj.EndTime = TimeObj.endTime;
                        timeViewObj.Type = TimeObj.type;
                        switch (TimeObj.type.Trim())
						{
							case "study": timeViewObj.Color = "#FFB6C1";
								break;
							case "waste": timeViewObj.Color = "#F08080";
                                break;
                            case "rest": timeViewObj.Color = "#98FB98";
                                break;
                            case "work": timeViewObj.Color = "#FFD700";
                                break;
                            case "none":
                                timeViewObj.Color = "#F3F3F3";
                                break;
                        }
                    }
					currentDateTemplate.DailyObjs.Add(timeViewObj);
				}
				currentDate = currentDate.AddDays(1);
				AllTimeViewObjs.Add(currentDateTemplate);
            }
			return AllTimeViewObjs;
        }

        private double CalculateHeight(TimeSpan lastTime)
        {
			TimeSpan allTimeSpan = new TimeSpan(18, 0, 0);
			return lastTime/allTimeSpan*(leftPanelHeight-60);
        }
		
    }
    public class GridSourceTemplate : ViewModelBase
    {
        private ObservableCollection<TimeViewObj> dailyObjs;

        public ObservableCollection<TimeViewObj> DailyObjs
        {
            get { return dailyObjs; }
            set { dailyObjs = value; OnPropertyChanged();  }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        private string week;
        public string Week
        {
            get { return week; }
            set { week = value; OnPropertyChanged(); }
        }
        private string color;
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }
        public GridSourceTemplate()
        {
            DailyObjs = new ObservableCollection<TimeViewObj>();
        }
       
    }

    public class GridViewList : ViewModelBase
    {
        private GridSourceTemplate _gridSourceTemplate;
        public GridSourceTemplate gridSourceTemplate
        {
            get { return _gridSourceTemplate; }
            set { _gridSourceTemplate = value; OnPropertyChanged(); }
        }
        public GridViewList()
        {
            gridSourceTemplate = new GridSourceTemplate();
        }

    }
}
