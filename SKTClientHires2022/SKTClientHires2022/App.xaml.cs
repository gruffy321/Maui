using System.IO;
namespace SKTClientHires2022;

public partial class App : Application
{
    public static Database db;
    public static string dbPath;

    public static Database Db
    {
        get
        {
            if (db == null)
            {
                //DateTime dNow = DateTime.Now;
                //DateTime dt = dNow.AddDays(1);
                //dt.Date.AddDays(1d);
                //string emailDateTime = dt.Date.ToShortDateString(); 
                //emailDateTime = emailDateTime.Replace('/', ' ');
                //suing this : {emailDateTime} as the insert to creata  new db each end of day - unfortunately it also completerty reomves all admin too
                //as it creates fresh table - so leaving this as is here aND WILL REPORT ON NEW TABLE CREATION AT A SEND STATUS PAGE
                dbPath = $"sktpersondb.db3";
                db = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbPath));
                var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbPath);

                //check if it actually exists on device
                if (File.Exists(path))
                {
                    string viewableFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string destinationPath = Path.Combine(viewableFolderPath, "DestinationDB.db3");
                    if (File.Exists(destinationPath))
                        return db;

                    File.Copy(path, destinationPath);
                }


            }

            return db;
        }
    }
    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage());
	}
}
