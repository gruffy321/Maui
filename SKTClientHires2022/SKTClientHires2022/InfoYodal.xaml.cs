namespace SKTClientHires2022;

public partial class InfoYodal : ContentPage
{
	public InfoYodal()
	{
		InitializeComponent();
	}
    public InfoYodal(string message)
    {
        InitializeComponent();
        databaseLegend.IsVisible = false;
        modalCollection.IsVisible = false;
        imgClientImage.IsVisible = false;
        imgClientExpanded.IsVisible = false;
        lblMessage.Text = message;
    }
    public InfoYodal(string message, bool shouldPop)
    {
        InitializeComponent();
        databaseLegend.IsVisible = false;
        modalCollection.IsVisible = false;
        imgClientImage.IsVisible = false;
        imgClientExpanded.IsVisible = false;
        lblMessage.Text = message;
    }
    public InfoYodal(string imgSource, int scale)
    {
        InitializeComponent();
        
        lblMessage.IsVisible = false;   
        databaseLegend.IsVisible = false;
        modalCollection.IsVisible = false;
        imgClientImage.IsVisible = false;
        imgClientExpanded.IsVisible = true;
        imgClientExpanded.Source = imgSource;

    }
    public InfoYodal(CollectionView view)
    {
        InitializeComponent();
        imgClientExpanded.IsVisible = false;
        imgClientImage.IsVisible = false;
        databaseLegend.IsVisible = true;
        modalCollection.IsVisible = true;
        modalCollection.ItemsSource = view.ItemsSource;
    }

    public InfoYodal(List<Person> persons)
    {
        InitializeComponent();
        imgClientExpanded.IsVisible = false;
        imgClientImage.IsVisible = true;
        databaseLegend.IsVisible = true;
        modalCollection.IsVisible = true;
        lblMessage.Text = $"Listing All Records in Database\nInc: Name, Parental Name if under 18\nA Contact Number\n TheTime of Hire Out\nTime of Hire Due In - both as times\nThe Under Age box Indicator\n The Hired item price and The Hired Item itself";
        modalCollection.ItemsSource = persons;
    }


    private async void TapNavigateBack_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void ImgClientImage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new InfoYodal(lastSelection.IdImageLocalPath, 3));
    }

    Person lastSelection;
    private void ModalCView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lastSelection = e.CurrentSelection[0] as Person;
        //e.CurrentSelection[0].Text = lastSelection.Name;
        //signatureEntry.Text = lastSelection.Signature;
        //contactEntry.Text = lastSelection.Contact;
        //tpHireTime.Time = TimeSpan.Parse(lastSelection.TimeOutStamp);
        //lblHirePrice.Text = $"£ {lastSelection.Price}";
        //ageNotOK.IsChecked = lastSelection.AgeNotOK;
        //btnCamera.Source = lastSelection.IdImageLocalPath;
        //SetBtnScale(1.5f, lastSelection.HireImageNumber);
        lblMessage.Text = $"Selected Person is {lastSelection.Name}\n" +
            $"Under 18? {lastSelection.AgeNotOK}\n" +
            $"Required if under 18\n{lastSelection.Signature}\t" +
            $"{lastSelection.Contact}\n" +
            $"Numeric hire type identifier {lastSelection.HireImageNumber}\n" +
            $"Cost: {lastSelection.Price}\t  Was Paid Cash: {lastSelection.Cash}\n" +
            $"Hired @ {lastSelection.TimeOutStamp}\t Due back {lastSelection.TimeDueIn}\n" +
            $"Their Database ID is: {lastSelection.Id}";
        imgClientImage.IsVisible = true;
        imgClientImage.Source = lastSelection.IdImageLocalPath;
    }
}