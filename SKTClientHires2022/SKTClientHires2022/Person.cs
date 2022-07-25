using SQLite;

namespace SKTClientHires2022
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Contact { get; set; }
        public string TimeOutStamp { get; set; }
        public int TimeOutHr { get; set; }
        public int TimeOutMin { get; set; }
        public string TimeDueIn { get; set; }
        public int Price { get; set; }
        public bool Cash { get; set; }
        public bool AgeNotOK { get; set; } 
        public int HireImageNumber { get; set; }
        public string HireImage { get; set; }

        public string IdImageLocalPath { get; set; }


    }
}
