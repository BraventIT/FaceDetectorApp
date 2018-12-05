using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public partial class BaseView : ContentPage
    {
        public BaseView()
        {
            InitializeComponent();
            BindingContext = new BaseViewModel();
            NavigationPage.SetBackButtonTitle(this, string.Empty);
        }

        public View ContentChild
        {
            get { return ChildContentContainer.Content; }
            set { ChildContentContainer.Content = value; }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as BaseViewModel)?.InitAsync();
        }

        public static readonly BindableProperty PaddingBaseViewProperty =
            BindableProperty.Create(nameof(PaddingBaseView), typeof(Thickness), typeof(BaseView), new Thickness(15), BindingMode.TwoWay);

        public Thickness PaddingBaseView
        {
            get { return (Thickness)GetValue(PaddingBaseViewProperty); }
            set { SetValue(PaddingBaseViewProperty, value); }
        }

    }
}
