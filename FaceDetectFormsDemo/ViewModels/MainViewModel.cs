using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FaceDetectFormsDemo.Models;
using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public class MainViewModel : BaseViewModel
    {

        #region Properties
        ObservableCollection<PersonGroupResponse> _groups;
        public ObservableCollection<PersonGroupResponse> Groups
        {
            get { return _groups; }
            set { SetProperty(ref _groups, value); }
        }


        public ICommand GroupSelectedCommand { get; private set; }

        #endregion

        #region Init and constructor
        public MainViewModel()
        {

            GroupSelectedCommand = new Command<PersonGroupResponse>(async (group) => await OnGroupSelected(group));

        }

        public override async Task InitAsync()
        {
            if (Groups != null) return;
            await ExecuteAsync(async () =>
            {

                var group = await FaceApiService.GetMyGroup();


                Groups = new ObservableCollection<PersonGroupResponse>
                {
                    group
                };

            });

        }
        #endregion

        #region Commands

        async Task OnGroupSelected(PersonGroupResponse personGroup)
        {
            await ExecuteAsync(async () =>
            {
                await Navigation.PushAsync(new GroupDetailView(personGroup));
            });
        }
        #endregion

    }
}
