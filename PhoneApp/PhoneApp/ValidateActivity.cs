using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PhoneApp
{
    [Activity(Label = "Validar Actividad", Icon = "@drawable/Icon")]
    public class ValidateActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.validate);
            var valButton = FindViewById<Button>(Resource.Id.btnValidar);
            var Resultados = FindViewById<EditText>(Resource.Id.tbRes);
            
            valButton.Click += (sender, e) =>
            {
                Validate();
                Resultados.Text = r1 + "\n" + r2 + "\n" + r3;
            };
        }
        public string r1 = "";
        public string r2 = "";
        public string r3 = "";
        private async void Validate()
        {
            var email = FindViewById<EditText>(Resource.Id.tbCorreo);
            var pass = FindViewById<EditText>(Resource.Id.tbPass);
            var ServiceClient =
                new SALLab06.ServiceClient();
            string StudentEmail = email.Text;
            string Password = pass.Text;
            string myDevice =
                Android.Provider.Settings.Secure.GetString(
                    ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var Result =
                await ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);
            var Resultados = FindViewById<EditText>(Resource.Id.tbRes);
            r1 = Result.Status.ToString();
            r2 = Result.Fullname.ToString();
            r3 = Result.Token.ToString();
            System.Console.WriteLine(r1);
            System.Console.WriteLine(r2);
            System.Console.WriteLine(r3);
            Resultados.Text = r1 + "\n" + r2 + "\n" + r3;
        }
    }
}