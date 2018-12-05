namespace FaceDetectFormsDemo
{
    public partial class MainPage : BaseView
    {
       MainViewModel ViewModel => BindingContext as MainViewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
            ViewModel.Navigation = Navigation;
        }

    }
}
