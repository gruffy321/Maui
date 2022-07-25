using System.Linq;

namespace SKTClientHires2022;

public partial class MainPage : ContentPage
{
    //simple admin cover fr viewing xml data
    private const string admin = "123";
    private string memberDetails = string.Empty;
    private bool revealState = true;


    public MainPage()
    {
        InitializeComponent();
        btnHelpIcon.Source = "helpicon.png";
        SetUI();
    }

    private void SetUI()
    {
        entPassword.Text = string.Empty;
        entAdminKey.Text = string.Empty;
        entPassword.Focus();
    }

    private async void BtnMembersCheck_Clicked(object sender, EventArgs e)
    {    
        if(revealState)
        {
            string adminKey = entAdminKey.Text;

            if (string.IsNullOrWhiteSpace(adminKey))
            {
                entAdminKey.Text = string.Empty;

                await Navigation.PushModalAsync(new InfoYodal($"The Admin key was empty!\n Try inputting something!"));
                return;
            }
            else if (adminKey != admin)
            {
                entAdminKey.Text = string.Empty;

                await Navigation.PushModalAsync(new InfoYodal($"The Admin key you inserted: {adminKey} was incorrect. Try again!"));
                return;
            }
            else
            {
                var members = await App.Db.GetMembersAsync();

                switch (members?.Count)
                {
                    case 0:
                        StatusLabel.Text = "None Found, Are you sure you created any users?";
                        break;
                    case > 0:
                        StatusLabel.Text = $"{members.Count} Members Loaded!";
                        BooksListControl.ItemsSource = await App.Db.GetMembersAsync(); //unsure if i need this yet
                        btnMembersCheck.Text = "ClearData";
                        revealState = !revealState;
                        break;
                    default:
                        StatusLabel.Text = "Something went wrong";
                        break;
                }
            }
        }
        else
        {            
            StatusLabel.Text = "To View staff keys Again Re-Enter Admin Key";
            entAdminKey.Text = string.Empty;
            btnMembersCheck.Text = "Reload staff Keys";
            BooksListControl.ItemsSource = null;
            revealState = !revealState;
            return;
        }       
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        string pass = entPassword.Text;

        if (string.IsNullOrWhiteSpace(pass))
        {
            InputError($"Passcode: ?{pass} ? Shows nothing was entered: Entry DENIED!");
        }
        else
        {
            List<Member> members = await App.Db.GetMembersAsync();

            var matchingMembers = (from m in members where m.Pass == pass select m).ToList();
            if (matchingMembers.Count > 0) 
            {
                foreach (var m in matchingMembers) 
                {
                    m.DateStamp = DateTime.Now.ToShortDateString();
                    m.TimeStamp = DateTime.Now.ToShortTimeString();
                    memberDetails = new string($" {m.Usr} @ {m.TimeStamp} on {m.DateStamp}");
                    await App.Db.UpdateMemberAsync(m);
                    NextPage();
                }
            }
            else
            {
                InputError($"Passcode: {pass} was NOT a recognized code");
            }                   
        }
    }
    private void InputError(string v)
    {
         Navigation.PushModalAsync(new InfoYodal(v));
    }
    private void NextPage()
    {
        entPassword.Text = string.Empty;
        Navigation.PushAsync(new ClientMainPage(memberDetails));
    }


    private async void BtnAddNewMember_Clicked(object sender, EventArgs e)
    {
        if (revealState)
        {
            string adminKey = entAdminKey.Text;

            if (string.IsNullOrWhiteSpace(adminKey))
            {
                entAdminKey.Text = string.Empty;

                await Navigation.PushModalAsync(new InfoYodal($"The Admin key was empty!\n Try inputting something!"));
                return;
            }
            else if(adminKey != admin)
            {
                entAdminKey.Text = string.Empty;

                await Navigation.PushModalAsync(new InfoYodal($"The Admin key you inserted: {adminKey} was incorrect. Try again!"));
                return;
            }
            else
            {
                
                AdminPage();
                
            }
        }
    }
    private async void AdminPage()
    {
        SetUI();
        await Navigation.PushAsync(new AddStaff());
    }

    private async void BtnHelpIcon_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
}