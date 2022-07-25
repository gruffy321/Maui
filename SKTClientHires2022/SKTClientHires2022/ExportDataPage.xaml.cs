namespace SKTClientHires2022;

public partial class ExportDataPage : ContentPage
{
	//private string[] emails = new string[] { "gruffy.wright@gmail.com", "kieranlaureston@gmail.com" };
	private string[] addy = new string[] { "gruffy.wright@gmail.com", "kieranlaureston@googlemail.com" };
    List<string> emails;
	private string subject = string.Empty;
	private string body = string.Empty;
	private string csv = string.Empty;

    public ExportDataPage()
	{
		InitializeComponent();
		InitPageData();
	}


	private async void InitPageData()
	{
		string mails = string.Empty;
		for(int i = 0; i < addy.Length; i++)
        {
            mails += $"{addy[i]} " ;
        }
		entEmailRecipient.Text = mails;

        emails = new List<string>();
        emails = addy.ToList();
        string emailDateTime = DateTime.Now.ToShortDateString();
        subject = $"Takings as Comma Delimited Values Dated {emailDateTime}";

        entEmailSubject.Text = subject;
        body = await ConcatenateToFile();
        entEmailBody.Text = body;
    }

	private async Task<String> ConcatenateToFile()
    {
        List<Person> persons = new List<Person>();
		List<string> formattedRecords = new List<string>();
		//grab db
		persons = await App.Db.GetPersonsAsync();
		//asses each record and reformat
		foreach (Person p in persons)
        {
			string concat = $"{p.Id},{p.Name},{p.Signature},{p.Contact},{p.TimeOutStamp},{p.TimeOutHr},{p.TimeOutMin},{p.TimeDueIn},{p.Price},{p.AgeNotOK},{p.HireImageNumber},{p.HireImage},{p.IdImageLocalPath};";
			formattedRecords.Add(concat);
			csv += concat;
		}

        return csv;
    }

    private async void BtnSendEmail_Clicked(object sender, EventArgs e)
    {

		if(entEmailBody.Text != string.Empty || entEmailRecipient.Text != string.Empty || entEmailSubject.Text != string.Empty)
        {
			//EMAIL THE EMAIL !
			var email = new EmailMessage
			{
				Subject = subject,
				Body = body,
				BodyFormat = EmailBodyFormat.PlainText,
				To = new List<string>(emails)
            };

            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), App.dbPath);
            email.Attachments.Add(new EmailAttachment(path));

            await Email.Default.ComposeAsync(email);

			await Navigation.PopAsync();

        }
    }
}