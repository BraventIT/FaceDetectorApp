using FaceDetectFormsDemo.Models;

namespace FaceDetectFormsDemo
{
    public partial class IdentifyFacesView : BaseView
    {
        IdentifyFacesViewModel ViewModel => BindingContext as IdentifyFacesViewModel;

        public IdentifyFacesView(PersonGroupResponse personGroup)
        {
            InitializeComponent();
            BindingContext = new IdentifyFacesViewModel(personGroup);
            ViewModel.Navigation = Navigation;
        }
    }
}
