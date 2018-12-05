using FaceDetectFormsDemo.Models;

namespace FaceDetectFormsDemo
{
    public partial class GroupDetailView : BaseView
    {
        GroupDetailViewModel ViewModel => BindingContext as GroupDetailViewModel;

        public GroupDetailView(PersonGroupResponse personGroup)
        {
            InitializeComponent();
            BindingContext = new GroupDetailViewModel(personGroup);
            ViewModel.Navigation = Navigation;
        }
    }
}
