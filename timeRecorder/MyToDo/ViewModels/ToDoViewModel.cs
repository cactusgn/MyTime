using MyToDo.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel:BindableBase
    {
        public ToDoViewModel()
        {
            toDoDtos = new ObservableCollection<ToDoDto>();
            createToDo();
            AddCommand= new DelegateCommand(Add);
        }
        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private void Add()
        {
           IsRightDrawerOpen = true;
        }

        public DelegateCommand  AddCommand { get; private set; }
        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged();  }
        }
        void createToDo()
        {
            for(int i = 0; i<20; i++)
            {
                toDoDtos.Add(new ToDoDto() { Title="ToDoTitle "+i, Content="Content "+i});
            }
        }

    }
}
