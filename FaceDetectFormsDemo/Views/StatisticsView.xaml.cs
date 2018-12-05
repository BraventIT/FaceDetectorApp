using FaceDetectFormsDemo.Models;

namespace FaceDetectFormsDemo
{
    public partial class StatisticsView : BaseView
    {
        StatisticsViewModel ViewModel => BindingContext as StatisticsViewModel;

        public StatisticsView(PersonGroupResponse personGroup)
        {
            InitializeComponent();
            BindingContext = new StatisticsViewModel(personGroup);
            ViewModel.Navigation = Navigation;
        }

        void Handle_Unfocused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            ViewModel?.SelectedPersonCommand.Execute(null);
        }
    }


}
