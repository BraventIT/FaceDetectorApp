using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FaceDetectFormsDemo.Models;
using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public class StatisticsViewModel : BaseViewModel
    {
        readonly PersonGroupResponse _personGroup;

        #region Properties
        bool _hasToShowPicker;
        public bool HasToShowPicker
        {
            get { return _hasToShowPicker; }
            set { SetProperty(ref _hasToShowPicker, value); }
        }

        ObservableCollection<Person> _persons;
        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set { SetProperty(ref _persons, value); }
        }

        ObservableCollection<Person> _similarPersons;
        public ObservableCollection<Person> SimilarPersons
        {
            get { return _similarPersons; }
            set 
            {
                SetProperty(ref _similarPersons, value);
            }
        }

        Person _personSelected;
        public Person PersonSelected
        {
            get { return _personSelected; }
            set
            {
                SetProperty(ref _personSelected, value);
            }
        }

        string _emotionText;
        public string EmotionText
        {
            get { return _emotionText; }
            set { SetProperty(ref _emotionText, value); }
        }

        string _ageText;
        public string AgeText
        {
            get { return _ageText; }
            set { SetProperty(ref _ageText, value); }
        }

        string _genderText;
        public string GenderText
        {
            get { return _genderText; }
            set { SetProperty(ref _genderText, value); }
        }

        public ICommand SelectedPersonCommand { get; private set; }
        #endregion


        public StatisticsViewModel(PersonGroupResponse personGroup)
        {
            _personGroup = personGroup;
            SelectedPersonCommand = new Command(async () => await OnSelectedPerson());
        }

        public override async Task InitAsync()
        {
            await ExecuteAsync(async () =>
            {
                HasToShowPicker = true;
                var persons = await FaceApiService.GetPersons();

                Persons = new ObservableCollection<Person>(persons);
                PersonSelected = persons?.FirstOrDefault();
            });
        }



        #region Commands
        async Task OnSelectedPerson()
        {
            await ExecuteAsync(async () =>
            {
                HasToShowPicker = false;
                await GetSimilarFaces();

                await LoadStatisticPercentages();

            });


        }

        async Task GetSimilarFaces()
        {
            var detectSelectedPersonRes = await FaceApiService.DetectFaces(PersonSelected.FaceImageUrl);
            var similarFaces = await FaceApiService.GetSimilarFaces(detectSelectedPersonRes.FirstOrDefault().FaceId);
            List<Person> similarPersons = new List<Person>();

            foreach(var similarFace in similarFaces)
            {
                var person = Persons.FirstOrDefault(p => p.LargeListFaceId == similarFace.PersistedFaceId);
                if (person != null)
                {
                    person.ConfidenceSimilarity = similarFace.Confidence.ToString();
                    similarPersons.Add(person);
                }
                    
            }

            SimilarPersons = new ObservableCollection<Person>(similarPersons);

        }
        #endregion

        #region Private voids
        async Task LoadStatisticPercentages()
        {
            var listPersonAttributes = new List<PersonAttributes>();

            foreach (var person in Persons)
            {
                if (string.IsNullOrWhiteSpace(person.FaceImageUrl))
                    continue;

                var detectResults = await FaceApiService.DetectFaces(person.FaceImageUrl);
                DetectFaceResponse detectFaceResponse = detectResults.FirstOrDefault();

                if (detectFaceResponse == null || detectFaceResponse.FaceAttributes == null)
                    continue;

                listPersonAttributes.Add(new PersonAttributes
                {
                    Emotion = GetEmotion(detectFaceResponse.FaceAttributes?.Emotion),
                    Age = detectFaceResponse.FaceAttributes.Age,
                    Gender = detectFaceResponse.FaceAttributes.Gender
                });

            }
            if (listPersonAttributes != null)
            {
                GetMaxEmotionOnGroup(listPersonAttributes);
                GetMediumAgeOnGroup(listPersonAttributes);
                GetPercentageFemaleOnGroup(listPersonAttributes);
            }

        }

        EmotionEnum GetEmotion(Emotion emotion)
        {
            EmotionEnum maxEmotionEnum = EmotionEnum.Anger;
            double maxEmotionValue = 0.0;
            if (emotion.Contempt > maxEmotionValue)
            {
                maxEmotionValue = emotion.Contempt;
                maxEmotionEnum = EmotionEnum.Contempt;
            }
            if (emotion.Disgust > maxEmotionValue)
            {
                maxEmotionValue = emotion.Disgust;
                maxEmotionEnum = EmotionEnum.Disgust;
            }
            if (emotion.Fear > maxEmotionValue)
            {
                maxEmotionValue = emotion.Fear;
                maxEmotionEnum = EmotionEnum.Fear;
            }

            if (emotion.Happiness > maxEmotionValue)
            {
                maxEmotionValue = emotion.Happiness;
                maxEmotionEnum = EmotionEnum.Happiness;
            }

            if (emotion.Neutral > maxEmotionValue)
            {
                maxEmotionValue = emotion.Neutral;
                maxEmotionEnum = EmotionEnum.Neutral;
            }

            if (emotion.Sadness > maxEmotionValue)
            {
                maxEmotionValue = emotion.Sadness;
                maxEmotionEnum = EmotionEnum.Sadness;
            }

            if (emotion.Surprise > maxEmotionValue)
            {
                maxEmotionValue = emotion.Surprise;
                maxEmotionEnum = EmotionEnum.Surprise;
            }
            return maxEmotionEnum;
        }

        void GetMaxEmotionOnGroup(List<PersonAttributes> personAttributes)
        {


            var neutralPeople = personAttributes?.Count(p => p.Emotion == EmotionEnum.Neutral);
            var happyPeople = personAttributes?.Count(p => p.Emotion == EmotionEnum.Happiness);
            var sadPeople = personAttributes?.Count(p => p.Emotion == EmotionEnum.Sadness);
            var surprisedPeople = personAttributes?.Count(p => p.Emotion == EmotionEnum.Surprise);

            EmotionEnum firstEmotion = EmotionEnum.Neutral;
            int? counterEmotion = neutralPeople ?? 0;

            if(happyPeople > counterEmotion)
            {
                firstEmotion = EmotionEnum.Happiness;
                counterEmotion = happyPeople;
            }
            if (sadPeople > counterEmotion)
            {
                firstEmotion = EmotionEnum.Sadness;
                counterEmotion = sadPeople;
            }
                
            if(surprisedPeople > counterEmotion)
            {
                firstEmotion = EmotionEnum.Surprise;
                counterEmotion = surprisedPeople;
            }
            var emotionText = string.Empty;
            switch (firstEmotion)
            {
                case EmotionEnum.Anger:
                    emotionText = "enfado";
                    break;
                case EmotionEnum.Contempt:
                    emotionText = "desprecio";
                    break;
                case EmotionEnum.Disgust:
                    emotionText = "asco";
                    break;
                case EmotionEnum.Fear:
                    emotionText = "temor";
                    break;
                case EmotionEnum.Happiness:
                    emotionText = "felicidad";
                    break;
                case EmotionEnum.Neutral:
                    emotionText = "neutral";
                    break;
                case EmotionEnum.Sadness:
                    emotionText = "tristeza";
                    break;
                case EmotionEnum.Surprise:
                    emotionText = "sorpresa";
                    break;

            }

            EmotionText = string.IsNullOrWhiteSpace(emotionText)
                                ? string.Empty
                                : $"Emoción más repetida: { emotionText}";

        }

        void GetMediumAgeOnGroup(List<PersonAttributes> personAttributes)
        {
            var mediumAge = personAttributes.Average(p => p.Age);

            AgeText = mediumAge <= 0.0 ? string.Empty : $"Edad media: {Math.Round(mediumAge)} años";
        }

        void GetPercentageFemaleOnGroup(List<PersonAttributes> personAttributes)
        {
            int femaleNumber = personAttributes.Count(p => p.Gender.Equals("female"));
            if(femaleNumber == 0)
            {
                GenderText = $"Porcentaje de mujeres: 0%";
            }
            double femalePercentage = (double)femaleNumber / (double)personAttributes.Count;

            GenderText = $"Porcentaje de mujeres: {femalePercentage * 100}%";
        }
        #endregion
    }

    public class PersonAttributes
    {
        public EmotionEnum Emotion { get; set; }
        public double Age { get; set; }
        public string Gender { get; set; }
    }

    public enum EmotionEnum
    {
        Anger,
        Contempt,
        Disgust,
        Fear,
        Happiness,
        Neutral,
        Sadness,
        Surprise
    }
}
