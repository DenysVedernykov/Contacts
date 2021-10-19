using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainListView : ContentPage
    {
        public MainListView()
        {
            InitializeComponent();

            var animation = new Animation(v => ButtonFloating.Scale = v, 1, 1.1);
            animation.Commit(this, "SimpleAnimation", 60, 800, Easing.Linear, (v, c) => ButtonFloating.Scale = 1, () => true);
        }
    }
}