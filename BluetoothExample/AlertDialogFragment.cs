using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace BluetoothExample
{
    public class AlertDialogFragment : DialogFragment
    {
        public Action Ok { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_alert, container);

            var textView = view.FindViewById<TextView>(Resource.Id.id_alert_message);
            textView.Text = "Need Permission";

            var btn = view.FindViewById<Button>(Resource.Id.id_ok_button);
            btn.Click += (sender, e) => {
                this.Dismiss();
                Ok?.Invoke();
                //Console.WriteLine("6");
            };

            return view;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}
