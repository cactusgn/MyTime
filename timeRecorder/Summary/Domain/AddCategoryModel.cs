using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Domain
{
    public class AddCategoryModel:ViewModelBase
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        private string category;
        
        public string Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(); }
        }
        private string noCaption;

        public string NoCaption
        {
            get { return noCaption; }
            set { noCaption = value; OnPropertyChanged(); }
        }
        private string yesCaption;

        public string YesCaption
        {
            get { return yesCaption; }
            set { yesCaption = value; OnPropertyChanged(); }
        }
        private string selectedColor;

        public string SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; OnPropertyChanged(); }
        }
        private int bonus;

        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; OnPropertyChanged(); }
        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        private int parentId;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ParentCategorySV> parentCategoryList;

        public ObservableCollection<ParentCategorySV> ParentCategoryList
        {
            get { return parentCategoryList; }
            set { parentCategoryList = value; OnPropertyChanged(); }
        }

        private bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; OnPropertyChanged(); }
        }
        private string showInvalidCateMessage= "Collapsed";

        public string ShowInvalidCateMessage
        {
            get { return showInvalidCateMessage; }
            set { showInvalidCateMessage = value; OnPropertyChanged(); }
        }
        public bool ParentEnabled
        {
            get
            {
                return ParentId!=0;
            }
        }
        public string ParentVisible
        {
            get
            {
                return ParentId!=0 ? "Visible" : "Collapsed";
            }
        }
        public string AutoCreateTaskVisible
        {
            get
            {
                return ParentId==0 ? "Visible": "Collapsed";
            }
        }
        private bool autoCreateTask;

        public bool AutoCreateTask
        {
            get { return autoCreateTask; }
            set { autoCreateTask = value; OnPropertyChanged(); }
        }

        public AddCategoryModel(){
        }
    }
    public class ParentCategorySV
    {
        public int ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
