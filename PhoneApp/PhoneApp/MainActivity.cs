using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp
{
    [Activity(Label = "Phone App", MainLauncher = true, Icon = "@drawable/Icon")]
    public class MainActivity : Activity
    {
        static readonly System.Collections.Generic.List<string> PhoneNumbers =
            new System.Collections.Generic.List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
                var TranslateButton= FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton= FindViewById<Button>(Resource.Id.CallButton);
            var CallHistoryButton=FindViewById<Button>(Resource.Id.CallHistoryButton);
            var ValidateActivityButton = FindViewById<Button>(Resource.Id.btnValidarAct);
            CallButton.Enabled = false;
            var TranslatedNumber = string.Empty;

            TranslateButton.Click += (object sender, System.EventArgs e) =>
              {
                  var Translator = new PhoneTranslator();
                  TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                  if (string.IsNullOrWhiteSpace(TranslatedNumber))
                  {
                      CallButton.Text = "Llamar";
                      CallButton.Enabled = false;
                  }
                  else
                  {
                      CallButton.Text = $"Llamar al {TranslatedNumber}";
                      CallButton.Enabled = true;
                  }
              };

            CallButton.Click+= (object sender, System.EventArgs e) =>
            {
                var CallDialog = new AlertDialog.Builder(this);
                CallDialog.SetMessage($"Llamar al numero { TranslatedNumber} ?");
                CallDialog.SetNeutralButton("Llamar", delegate
                 {
                     PhoneNumbers.Add(TranslatedNumber);
                     CallHistoryButton.Enabled = true;
                     var CallIntent =
                     new Android.Content.Intent(Android.Content.Intent.ActionCall);
                     CallIntent.SetData(
                         Android.Net.Uri.Parse($"tel:{ TranslatedNumber}"));
                     StartActivity(CallIntent);
                 });
                CallDialog.SetNegativeButton("Cancelar",delegate { });
                CallDialog.Show();
            };

            CallHistoryButton.Click += (sender, e) =>
              {
                  var Intent = new Android.Content.Intent(this,
                      typeof(CallHistoryActivity));
                  Intent.PutStringArrayListExtra("phone_numbers",PhoneNumbers);
                  StartActivity(Intent);
              };

            ValidateActivityButton.Click += (sender, e) =>
            {
                var Intent = new Android.Content.Intent(this,
                    typeof(ValidateActivity));
                StartActivity(Intent);
            };

        }

    }
}