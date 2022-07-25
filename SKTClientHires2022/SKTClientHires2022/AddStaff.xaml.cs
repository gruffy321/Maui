namespace SKTClientHires2022;

public partial class AddStaff : ContentPage
{
    private bool hasAdminAccess = true;
    private const string adminKey = "123";
	public AddStaff()
	{
		InitializeComponent();
        btnZRead.IsEnabled = false;
	}

    private async void BtnCreateNewStaff_Clicked(object sender, EventArgs e)
    {
        string admin = entAdminKey.Text;
        if(admin == adminKey)
        {
            //Id
            //Name
            //TimeOutStamp
            //DateOutStamp
            string user, pass, time, date;
            user = entStaffName.Text;
            pass = entStaffPasscode.Text;
            time = DateTime.Now.ToShortTimeString();
            date = DateTime.Now.ToShortDateString();
            await App.Db.SaveMemberAsync(new Member
            {
                //id auto incremnts
                Usr = user,
                Pass = pass,
                TimeStamp = time,
                DateStamp = date
            });
            await Navigation.PushModalAsync(new InfoYodal($"Member saved as: {user}, with passcode: {pass}\n Created on : {date} @ {time}"));
            await Navigation.PopModalAsync(true);
        }
        else
        {
            await Navigation.PushModalAsync(new InfoYodal("The admin key required to enter a new staff member\nwas not found or was incorrect"));

        }

    }

    private async void BtnViewExistingMembers_Clicked(object sender, EventArgs e)
    {
        if (hasAdminAccess)
        {
            string currentEntry = entAdminKey.Text;

            if (currentEntry != adminKey)
            {
                entAdminKey.Text = string.Empty;

                //pop up a modal warning? - UX
                return;
            }
            var members = await App.Db.GetMembersAsync();

            switch (members?.Count)
            {
                case 0:
                    btnViewMembers.Text = "None Found, Are you sure you created any users?";
                    break;
                case > 0:
                    //StatusLabel.Text = $"{members.Count} Members Loaded!";
                    //BooksListControl.ItemsSource = members;
                    memberView.ItemsSource = await App.Db.GetMembersAsync();
                    //btnViewMembers.Text = "ClearData";
                    hasAdminAccess = !hasAdminAccess;
                    break;
                default:
                    btnViewMembers.Text = "Something went wrong";
                    break;
            }
        }
        else
        {
            //StatusLabel.Text = "To View Again Re-Enter Admin Key";
            entAdminKey.Text = string.Empty;
            btnViewMembers.Text = "Review Staff Members [ADMIN KEY REQUIRED]";
            memberView.ItemsSource = null;
            hasAdminAccess = !hasAdminAccess;
            return;
        }


    }
    /// <summary>
    /// z read the days takings, fomrat them and havethem ready for the emailing page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnZRead_Clicked(object sender, EventArgs e)
    {
        string admin = entAdminKey.Text;
        if (admin == adminKey)
        {
            //set up new database table for next day

            App.Db.DropAndCreateNewPersonTableAsync();
            DateTime dNow = DateTime.Now;
            DateTime personTableDateCreated = dNow.AddDays(1);
            //personTableDateCreated.Date.AddDays(1d);
            Navigation.PushModalAsync(new InfoYodal($"new Person table created\n ready for Tomorrow's date: {personTableDateCreated.Date.ToShortDateString()}\nTime created: {personTableDateCreated.ToShortTimeString()} today"));
            btnZRead.Text = "Z READ COMPLETE- Table has been readied for tomorrows hires";
        }
        else
        {
            Navigation.PushModalAsync(new InfoYodal("The admin key required to destroy and recreate\nthe database was not correct or empty\nREMINDER: DO NOT USE THIS FUNCTION UNLESS...\nYOU HAVE EMAILED THE DAILY DATA BASE"));

        }
    }

    Member lastSelection;
    private void CView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lastSelection = e.CurrentSelection[0] as Member;
        entStaffName.Text = lastSelection.Usr;
        entStaffPasscode.Text = lastSelection.Pass;
    }
    private async void BtnDeleteMember_Clicked(object sender, EventArgs e)
    {
        if(lastSelection != null)
        {
            //lastSelection.Usr = entStaffName.Text;
            //lastSelection.Pass = ageNotOK.IsChecked;
            string lastUsr = lastSelection.Usr;
            //remove person from db, based on last selected in coleciton view
            await App.Db.DeleteMemberAsync(lastSelection);

            memberView.ItemsSource = await App.Db.GetMembersAsync();

            await Navigation.PushModalAsync(new InfoYodal($"{lastUsr} has been removed from the system"));

            entStaffName.Text = string.Empty;
            entStaffPasscode.Text = string.Empty;
        }
        
    }

    private void BtnOpenEmailerSystem_Clicked(object sender, EventArgs e)
    {
        btnZRead.IsEnabled = true;
        Navigation.PushAsync(new NavigationPage(new ExportDataPage()));
    }
}