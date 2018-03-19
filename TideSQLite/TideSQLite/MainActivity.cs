using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using SQLite;
using System.IO;
using System.Collections.Generic;
using DAL;
using System.Linq;

namespace TideSQLite
{
    [Activity(Label = "TideSQLite", MainLauncher = true, Icon = "@drawable/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class MainActivity : Activity
    {
        private string dbPath;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tides.db");


            SetContentView(Resource.Layout.Main);

            if (bundle == null)
            {



                ExtractDatabase();
                var datePicker = FindViewById<DatePicker>(Resource.Id.tideDatePicker);
                long min = (DateTime.Parse("2018/01/01").Ticks - DateTime.Parse("1969/12/31").Ticks) / 10000;

                long max = (DateTime.Parse("2018/12/31").Ticks - DateTime.Parse("1969/12/31").Ticks) / 10000;
                datePicker.MinDate = min;
                datePicker.MaxDate = max;
            }

            var locationSpinner = FindViewById<Spinner>(Resource.Id.locationSpinner);
            var locations = GetLocations();
            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, locations);


            locationSpinner.Adapter = adapter;



            var showBtn = FindViewById<Button>(Resource.Id.showTidesBtn);
            showBtn.Click += ShowTideButtonClick;
        }

        private void ShowTideButtonClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TideListActivity));

            var datePicker = FindViewById<DatePicker>(Resource.Id.tideDatePicker);
            var locationSpinner = FindViewById<Spinner>(Resource.Id.locationSpinner);
            var location = (string)locationSpinner.SelectedItem;

            intent.PutExtra("DateTicks", datePicker.DateTime.Ticks);

            intent.PutExtra("Location", location);

            StartActivity(intent);
        }

        private void ExtractDatabase()
        {
            using (var instream = Assets.Open("tides.db"))
            {
                using (var outstream = File.Create(dbPath))
                {
                    instream.CopyTo(outstream);
                }
            }
        }

        private List<string> GetLocations()
        {
            var locations = new List<string>();

            using (var db = new SQLiteConnection(dbPath))




            {
                locations = db.Table<Tide>().ToList().Select(t => t.Location).Distinct().ToList();
            }

            return locations;
        }
    }
}

