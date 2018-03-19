using SQLite;

namespace DAL
{
    public class Tide
    {
        public Tide() { }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public long TimeTicks { get; set; }

        [MaxLength(4)]
        public string HighLow { get; set; }

        public string Location { get; set; }

        public float FeetHeight { get; set; }

        public int CentimeterHeight { get; set; }
    }
}
