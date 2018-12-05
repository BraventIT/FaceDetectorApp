using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FaceDetectFormsDemo.Models;
using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public class GroupDetailViewModel : BaseViewModel
    {
        readonly PersonGroupResponse _personGroup;


        #region Properties

        ObservableCollection<Person> _persons;
        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set { SetProperty(ref _persons, value); }
        }

        string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ICommand AddPersonCommand { get; private set; }

        public ICommand DeletePersonCommand { get; private set; }

        public ICommand IdentifyPersonsOnPhotoCommand { get; private set; }

        public ICommand StatisticsCommand { get; private set; }

        #endregion

        #region Init and Constructor
        public GroupDetailViewModel(PersonGroupResponse personGroup)
        {
            _personGroup = personGroup;
            AddPersonCommand = new Command(async () => await OnAddPerson());
            DeletePersonCommand = new Command<Person>(async (person) => await OnDeletePerson(person));
            IdentifyPersonsOnPhotoCommand = new Command(async () => await OnIdentifyPersonsOnPhoto());
            StatisticsCommand = new Command(async () => await OnStatistics());
            Title = _personGroup.Name;

        }

        public override async Task InitAsync()
        {
            await ExecuteAsync(async () =>
            {
                var persons = await FaceApiService.GetPersons();
                Persons = new ObservableCollection<Person>(persons);

            });
        }
        #endregion

        #region Commands
        async Task OnAddPerson()
        {
            await ExecuteAsync(async () =>
            {
                await Navigation.PushAsync(new NewPersonView(_personGroup));
            });
        }

        async Task OnDeletePerson(Person person)
        {
            await ExecuteAsync(async () =>
            {
                await FaceApiService.DeletePersonFromGroup(_personGroup.PersonGroupId, person.PersonId);
                Persons.Remove(person);
            });
        }

        async Task OnIdentifyPersonsOnPhoto()
        {
            await ExecuteAsync(async () =>
            {
                await Navigation.PushAsync(new IdentifyFacesView(_personGroup));
            });

        }


        async Task OnStatistics()
        {
            await ExecuteAsync(async () =>
            {
                await Navigation.PushAsync(new StatisticsView(_personGroup));
            });
        }
        #endregion
    }
}
