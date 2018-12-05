using System.Windows.Input;
using Xamarin.Forms;

namespace FaceDetectFormsDemo
{
    public class CustomListView: ListView
    {

        public CustomListView(ListViewCachingStrategy cachingStratety) : base(cachingStratety) { }

        public CustomListView() : this(ListViewCachingStrategy.RecycleElement)
        {
            ItemTapped += CustomListView_ItemTapped;
        }



        void CustomListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemSelected = (sender as ListView).SelectedItem;
            Command?.Execute(itemSelected);
        }


        public static readonly BindableProperty CommandProperty = BindableProperty
            .Create(nameof(Command), typeof(ICommand), typeof(CustomListView));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomListView), null, BindingMode.TwoWay);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

    }
}
