using System.Collections;
using SQLite;

namespace SKTClientHires2022;

public class Member
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Usr { get; set; }
    public string Pass { get; set; }
    public string TimeStamp { get; set; }
    public string DateStamp { get; set; }

}