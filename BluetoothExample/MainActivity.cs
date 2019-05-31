using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;

namespace BluetoothExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);



            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
        const string _pkgName = "com.companyname.bluetoothexample";

        protected override void OnResume()
        {
            base.OnResume();

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Test();
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                //.SetAction("Action", OnClick).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnClick(View v)
        {
            Console.WriteLine("OnClick");
        }

        const int _ccode = 777;

        public void Test()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var ss = ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation);
                var per = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
                var permission = PackageManager.CheckPermission(Manifest.Permission.AccessFineLocation, _pkgName);
                if (ss)
                {
                    var view = this.FindViewById(Resource.Id.main_view);
                    Snackbar.Make(view, "Need permission", Snackbar.LengthLong).Show();
                    //.SetAction("Action", OnClick).Show();
                    //DialogFragment dialogFragment = new Android.App.DialogFragment();
                    Android.App.FragmentTransaction transcation = FragmentManager.BeginTransaction();
                }

                    ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation }, _ccode);

            }
            else
            {

            }
        }


    }
}

