using System;
using System.Collections.Generic;
using System.Linq;
using Android;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace BluetoothExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        View _mainView;
        TextView _textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);


            _mainView = this.FindViewById(Resource.Id.main_view);

            _textView = _mainView.FindViewById<TextView>(Resource.Id.id_textView);
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        private BluetoothManager _bluetoothManager;
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothLeScanner _bluetoothLeScanner;

        protected override void OnStart()
        {
            base.OnStart();

            _bluetoothManager = (BluetoothManager)GetSystemService(BluetoothService);
            _bluetoothAdapter = _bluetoothManager.Adapter;

            _bluetoothLeScanner = _bluetoothAdapter?.BluetoothLeScanner;
        }

        const string _pkgName = "com.cxzhe.bluetoothexample";

        protected override void OnResume()
        {
            base.OnResume();

            if (_bluetoothAdapter == null || !_bluetoothAdapter.IsEnabled)
                OpenBluetooth();

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
            //View view = (View)sender;
            ScanDevice();
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                //.SetAction("Action", OnClick).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == _locaPermissionRequestCode)
            {
                if (!grantResults.Any(r => r == Permission.Denied))
                {
                    Scan();
                }
                else
                {
                    Snackbar.Make(_mainView, "Need permission", Snackbar.LengthLong).Show();
                    Android.App.FragmentTransaction transcation = FragmentManager.BeginTransaction();
                }
            }
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //public void OnClick(View v)
        //{
        //    Console.WriteLine("OnClick");
        //}

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == _openBluetoothRequesetCode)
            {
                _bluetoothAdapter = _bluetoothManager.Adapter;
                _bluetoothLeScanner = _bluetoothAdapter?.BluetoothLeScanner;
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        const int _openBluetoothRequesetCode = 1010;
        const int _locaPermissionRequestCode = 777;

        public void ScanDevice()
        {
            if (CheckPermission())
            {
                Scan();
            }
            else
            {
                //var view = this.FindViewById(Resource.Id.main_view);
                //Snackbar.Make(view, "Need permission", Snackbar.LengthLong).Show();
                //Android.App.FragmentTransaction transcation = FragmentManager.BeginTransaction();
            }

            //_bluetoothAdapter.ScanMode = Android.Bluetooth.ScanMode.Connectable;
            //var b = _bluetoothAdapter.StartLeScan(this);
            //_bluetoothAdapter.StartDiscovery();

        }

        private readonly Handler _handler = new Handler();

        private void Scan()
        {
            if (_bluetoothManager.Adapter.State != State.Disconnecting)
            {
                Console.WriteLine("StartScan");
                _textView.Text = string.Empty;
                _bluetoothLeScanner.StartScan(Scanner.Instance);
                _handler.PostDelayed(() =>
                {
                    Console.WriteLine("StopScan");
                    _bluetoothLeScanner.StopScan(Scanner.Instance);

                }, 10000);
            }
        }

        private void OpenBluetooth()
        {
            var intent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(intent, _openBluetoothRequesetCode);
        }

        private bool CheckPermission()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var again = ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation);
                var per = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);
                //var permission = PackageManager.CheckPermission(Manifest.Permission.AccessFineLocation, _pkgName);
                if (per == Permission.Denied)
                {
                    if (again)
                    {
                        var alert = new AlertDialogFragment();

                        alert.Ok += () => {
                            ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation }, _locaPermissionRequestCode);
                        };
                        alert.Show(FragmentManager, "Alert");

                    }
                    else
                        ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation }, _locaPermissionRequestCode);

                    return false;
                }
            }
            else
            {

            }

            return true;
        }
    }

    internal class Scanner : ScanCallback
    {
        public static Scanner Instance;

        static Scanner()
        {
            Instance = new Scanner();
        
        }

        //TextView _textView;

        //public Scanner(TextView textView)
        //{
        //    _textView = textView;
        //}

        public override void OnBatchScanResults(IList<ScanResult> results)
        {
            //Console.WriteLine("");
            //base.OnBatchScanResults(results);
        }

        public override void OnScanFailed([GeneratedEnum] ScanFailure errorCode)
        {
            //Console.WriteLine("");
            //base.OnScanFailed(errorCode);
        }

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
        {
            var device = result.Device;
            if (!string.IsNullOrEmpty(device.Name))
            {
                //_textView.Text += result.Device.Address + result.Device.Name;
                Console.WriteLine(result.Device.Address + result.Device.Name);
            }
            //base.OnScanResult(callbackType, result);
        }
    }
}

