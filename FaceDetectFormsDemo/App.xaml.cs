using System;
using FaceDetectFormsDemo.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FaceDetectFormsDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            MainPage = new NavigationPage(new MainPage());
            (Current.MainPage as NavigationPage).BarBackgroundColor = Color.FromHex("#CED345");
            (Current.MainPage as NavigationPage).BarTextColor = Color.White;

            DependencyService.Register<IHttpProvider, HttpProvider>();
            DependencyService.Register<IFaceApiService, FaceApiService>();
            DependencyService.Register<IBlobStorageService, BlobStorageService>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
