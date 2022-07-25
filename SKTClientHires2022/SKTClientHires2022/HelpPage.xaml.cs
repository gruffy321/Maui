namespace SKTClientHires2022;

public partial class HelpPage : ContentPage
{
	public HelpPage()
	{
		InitializeComponent();
        //SetLabels();

        SetBtnScale(1.2f);
        
	}

    private void BtnLoginHelp_Clicked(object sender, EventArgs e)
    {
        imgLogin.Source = "loginhelp.png";
        //await Navigation.PushModalAsync(new InfoYodal("loginhelp.png", 1));
    }

    private void BtnInputHelp_Clicked(object sender, EventArgs e)
    {
        imgLogin.Source = "hireinputs.png";
        //await Navigation.PushModalAsync(new InfoYodal("hireinputs.png", 1));
    }

    private void BtnButtonsHelp_Clicked(object sender, EventArgs e)
    {
        imgLogin.Source = "hirebuttons.png";
        //await Navigation.PushModalAsync(new InfoYodal("hirebuttons.png", 1));
    }
    private void SetBtnScale(float scaleBy)
    {
        imgLogin.Scale = scaleBy;
    }

    private void BtnSwiperHelp_Clicked(object sender, EventArgs e)
    {
        imgLogin.Source = "swipehelp.png";
    }
}