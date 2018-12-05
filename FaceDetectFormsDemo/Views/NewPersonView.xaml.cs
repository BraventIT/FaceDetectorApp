using FaceDetectFormsDemo.Models;

namespace FaceDetectFormsDemo
{
    public partial class NewPersonView : BaseView
    {
        NewPersonViewModel ViewModel => BindingContext as NewPersonViewModel;

        public NewPersonView(PersonGroupResponse personGroup)
        {
            InitializeComponent();
            BindingContext = new NewPersonViewModel(personGroup);
            ViewModel.Navigation = Navigation;
        }
    }
}
