namespace Summary.Domain
{
    public class SampleDialogViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
    }
}
