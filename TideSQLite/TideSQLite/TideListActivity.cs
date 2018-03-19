using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using SQLite;
using DAL;
using System.IO;

namespace TideSQLite
{
    [Activity(Label = "TideList", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class TideListActivity : ListActivity
    {
        private string dbPath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tides.db");

            SetTideList();
        }

        private void SetTideList()
        {
            var selectedTicks = Intent.GetLongExtra("DateTicks", DateTime.Now.Ticks);
            var weekAhead = selectedTicks + TimeSpan.FromDays(7).Ticks;
            var location = Intent.GetStringExtra("Location");
            Tide[] tides;

            using(var db = new SQLiteConnection(dbPath))
            {
                tides = db.Table<Tide>().Select(t => t).Where(t => t.Location == location && t.TimeTicks >= selectedTicks && t.TimeTicks < weekAhead).ToArray();
            }

            ListAdapter = new TideAdapter(this, tides);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            SetTideList();
        }
    }
}