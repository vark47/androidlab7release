using System;

using Android.App;
using Android.Views;
using Android.Widget;
using DAL;

namespace TideSQLite
{
    public class TideAdapter : BaseAdapter<Tide>
    {
        private Tide[] tideArray;
        private Activity context;

        public TideAdapter(Activity context, Tide[] items)
        {
            tideArray = items;
            this.context = context;
        }

        public override Tide this[int position]
        {
            get
            {
                return tideArray[position];
            }
        }

        public override int Count
        {
            get
            {
                return tideArray.Length;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.TideCell, null);

            var tide = tideArray[position];
            var date = new DateTime(tide.TimeTicks);

            view.FindViewById<TextView>(Resource.Id.dateTimeLbl).Text = date.ToString("MM/dd HH:MM");
            view.FindViewById<TextView>(Resource.Id.dayLbl).Text = date.ToString("ddd");
            view.FindViewById<TextView>(Resource.Id.heightLbl).Text = tide.FeetHeight.ToString() + " feet";
            view.FindViewById<TextView>(Resource.Id.hiloLbl).Text = tide.HighLow;

            return view;
        }
    }
}