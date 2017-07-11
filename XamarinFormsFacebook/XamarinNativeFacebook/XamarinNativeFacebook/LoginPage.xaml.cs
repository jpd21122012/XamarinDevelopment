namespace XamarinNativeFacebook
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();

            App.PostSuccessFacebookAction = async token =>
            {
                await Navigation.PushAsync(new DiplayTokenPage(token));
            };
        }
    }
}