using static System.Net.Mime.MediaTypeNames;

namespace SKTClientHires2022
{
    public partial class ClientMainPage : ContentPage
    {
        private int imgSourceNumber;
        private string currentMember = string.Empty;
        private string imgSourceString = string.Empty;
        private string timeDueInAsString = string.Empty;
        private string timeDueOutAsString = string.Empty;
        private string idImageLocalPath = string.Empty;
        private string defaultCamImage = string.Empty;
        private string defaultCashOrCardImage = string.Empty;
        private int hireTypeSelectedPrice;
        private bool cameraBtnPressed = false;

        public ClientMainPage()
        {
            InitializeComponent();
            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
            Title = "SKT HIRES";
            SetUIToEmpty();
        }
        public ClientMainPage(string name)
        {
            InitializeComponent();
            currentMember = name;
            name = name.ToUpper();
            Title = $"Welcome to SKT Hires {name}";
            SetUIToEmpty();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            collectionView.ItemsSource = await App.Db.LinqAllRecordsAsync();


        }
        private async void BtnAddUser_Clicked(object sender, EventArgs e)
        {
            //Id
            //Name
            //Signature
            //Contact
            //TimeOutStamp
            //TimeDueIn
            //Price
            //AgeNotOK
            //HireImageNumber
            //HireImage  -> string

            if (nameEntry.Text == "" || imgSourceNumber <= 0)
            {
                await Navigation.PushModalAsync(new InfoYodal($"A name entry or hire item was not selected\nNO ENTRY WAS STORED\n ALL INPUT WILL BE WIPED FOR THIS ENTRY"));
                return;
            }
            if (ageNotOK.IsChecked)
            {
                if (signatureEntry.Text == "" || contactEntry.Text == "")
                {
                    await Navigation.PushModalAsync(new InfoYodal($"The under 18+ option was checked but...\nNO adult was verified & No Phone number inserted \nNO ENTRY WAS STORED\n ALL INPUT WILL BE WIPED FROM THIS ENTRY"));
                    return;
                }
            }
            if(!cameraBtnPressed)
            {
                await Navigation.PushModalAsync(new InfoYodal($"The Required Image ID was not taken...\n ALL INPUT WILL BE WIPED FROM THIS ENTRY"));
                return;
            }
            //set time for pasing through
            TimeSpan ts = new TimeSpan(tpHireTime.Time.Hours, tpHireTime.Time.Minutes, tpHireTime.Time.Seconds);
            timeDueOutAsString = ts.ToString(); //$"{ts.Hours}:{ts.Minutes}:{ts.Seconds}";
            ts = new TimeSpan(tpHireTime.Time.Hours + 1, tpHireTime.Time.Minutes, tpHireTime.Time.Seconds);//.Add(new TimeSpan(1, 0, 0));
            timeDueInAsString = ts.ToString();

            if (ageNotOK.IsChecked == false)
            {
                contactEntry.Text = "00000000000";
                signatureEntry.Text = "IS AN ADULT";
            }
            await App.Db.SavePersonAsync(new Person
            {
                //id auto incremnts
                Name = nameEntry.Text,
                Signature = signatureEntry.Text,
                Contact = contactEntry.Text,
                TimeOutStamp = timeDueOutAsString,
                TimeOutHr = ts.Hours,
                TimeOutMin = ts.Minutes,
                TimeDueIn = timeDueInAsString,
                Price = hireTypeSelectedPrice,
                Cash = cashOrCard,
                AgeNotOK = ageNotOK.IsChecked,
                HireImageNumber = imgSourceNumber,
                HireImage = imgSourceString,
                IdImageLocalPath = idImageLocalPath
            });
            SetUIToEmpty();
            //reset itemseoufrce to app db since update
            await ResetCurrentCollectionView();

        }

        private void SetUIToEmpty()
        {
            //button reset
            nameEntry.Text = "";
            signatureEntry.Text = "";
            contactEntry.Text = "";
            ageNotOK.IsChecked = false;
            ageNotOK.Scale = 2d;
            cashOrCard = false;
            lblHirePrice.Text = "£ 0";
            //changes
            signatureEntry.IsVisible = true;
            contactEntry.IsVisible = true;
            contactEntry.TextColor = new Color(1f, 0f, 1f, 0.5f);
            signatureEntry.TextColor = new Color(1f, 0f, 1f, 0.5f);
            signatureEntry.IsEnabled = false;
            contactEntry.IsEnabled = false;
            ageNotOK.IsChecked = false;

            defaultCamImage = "camicon.png";
            defaultCashOrCardImage = "cardicon.png";
            btnCamera.Source = defaultCamImage;
            btnCashOrCard.Source = defaultCashOrCardImage;
            cameraBtnPressed = false;
            SetTime();

        }

        Person lastSelection;
        private void CView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lastSelection = e.CurrentSelection[0] as Person;
            nameEntry.Text = lastSelection.Name;
            signatureEntry.Text = lastSelection.Signature;
            contactEntry.Text = lastSelection.Contact;
            tpHireTime.Time = TimeSpan.Parse(lastSelection.TimeOutStamp);
            lblHirePrice.Text = $"£ {lastSelection.Price}";
            ageNotOK.IsChecked = lastSelection.AgeNotOK;
            btnCamera.Source = lastSelection.IdImageLocalPath;
           
            
        }
        //unsafe method due totry parseing the int value for price: button has been disabled, but still viewable
        private async void BtnUpdateUser_Clicked(object sender, EventArgs e)
        {
            if (lastSelection != null)
            {
                lastSelection.Name = nameEntry.Text;
                lastSelection.Signature = signatureEntry.Text;
                lastSelection.Contact = contactEntry.Text;
                int price;
                if (Int32.TryParse(lblHirePrice.Text, out price))
                {
                    lastSelection.Price = price;
                }

                lastSelection.Cash = cashOrCard;
                lastSelection.AgeNotOK = ageNotOK.IsChecked;
                lastSelection.IdImageLocalPath = btnCamera.Source.ToString();
                ////update person from db, based on last selected in coleciton view

                //this might need clearing for posterity...
            }
            await App.Db.UpdatePersonAsync(lastSelection);

            await ResetCurrentCollectionView();
        }
        private async Task ResetCurrentCollectionView()
        {   
            collectionView.ItemsSource = await App.Db.GetPersonsAsync();
        }
        private async void BtnDeleteUser_Clicked(object sender, EventArgs e)
        {
            if (lastSelection != null)
            {
                lastSelection.Name = nameEntry.Text;
                lastSelection.AgeNotOK = ageNotOK.IsChecked;
                lastSelection.Cash = cashOrCard;
                await App.Db.DeletePersonAsync(lastSelection);
                await ResetCurrentCollectionView();
                SetUIToEmpty();
            }
        }
        private async void BtnSeeChildOnlyHires_Clicked(object sender, EventArgs e)
        {
            collectionView.ItemsSource = await App.Db.LinqAgeNotOKHiresAsync(true);
        }
        private async void BtnSeeAdultOnlyHires_Clicked(object sender, EventArgs e)
        {
            collectionView.ItemsSource = await App.Db.LinqAgeNotOKHiresAsync(false);
        }
        private async void BtnSeeCurrentHires_Clicked(object sender, EventArgs e)
        {
            SetTime();
            collectionView.ItemsSource = await App.Db.LinqSeeCurrentOrCompletedHires(true, tpHireTime.Time);
        }
        private async void BtnSeeCompletedHires_Clicked(object sender, EventArgs e)
        {
            SetTime();
            collectionView.ItemsSource = await App.Db.LinqSeeCurrentOrCompletedHires(false, tpHireTime.Time);
        }
        private async void BtnSeeAllHires_Clicked(object sender, EventArgs e)
        {
            List<Person> allPersons = await App.Db.GetPersonsAsync();
            await Navigation.PushModalAsync(new InfoYodal(allPersons));
        }
        private async void BtnReturnEntireDatabase_Clicked(object sender, EventArgs e)
        {
            collectionView.ItemsSource = await App.Db.LinqAllRecordsAsync();

            await Navigation.PushModalAsync(new InfoYodal(collectionView));
        }

        #region image interaction code

        private void SetTime()
        {
            //set time picker in view
            int h, m, s;
            h = DateTime.Now.Hour;
            m = DateTime.Now.Minute;
            s = DateTime.Now.Second;
            TimeSpan ts = new TimeSpan(h, m, s);
            tpHireTime.Time = ts;
        }

        #endregion

        private void TimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            TimePicker t = sender as TimePicker;

            TimeSpan ts = new TimeSpan(t.Time.Hours, t.Time.Minutes, t.Time.Seconds);
            timeDueOutAsString = ts.ToString();
            ts.Add(new TimeSpan(1, 0, 0));
            timeDueInAsString = ts.ToString();
        }
        private void AgeNotOK_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {       
                bool choice = e.Value;
                if (choice)
                {
                    ageNotOK.IsChecked = true;
                    contactEntry.TextColor = new Color(0f, 0f, 0f, 1f);
                    signatureEntry.TextColor = new Color(0f, 0f, 0f, 1f);
                    contactEntry.IsEnabled = true;
                    signatureEntry.IsEnabled = true;
                    signatureEntry.Focus();

                }
                else
                {
                    ageNotOK.IsChecked = false;
                    contactEntry.TextColor = new Color(1f, 0f, 1f, 0.5f);
                    signatureEntry.TextColor = new Color(1f, 0f, 1f, 0.5f);
                    contactEntry.IsEnabled = false;
                    signatureEntry.IsEnabled = false;
            }
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            //poss nto needed - cull at final
        }

        private void imgPaddleOrFloat_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnCamera_Clicked(object sender, EventArgs e)
        {
            string camImgPath = btnCamera.Source.ToString();
            if (camImgPath == $"File: {defaultCamImage}" )
            {
                TakeImage();
            }
            else
            {
                if(lastSelection != null)
                {
                    Navigation.PushModalAsync(new InfoYodal(lastSelection.IdImageLocalPath, 1));
                }
                else
                {
                    camImgPath = camImgPath.Remove(0, 6);
                    Navigation.PushModalAsync(new InfoYodal(camImgPath, 1));
                }
            }            
        }
        FileResult image;
        private async void TakeImage()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                image  = await MediaPicker.Default.CapturePhotoAsync();
                if(image != null)
                {
                    //save
                    string localPath = Path.Combine(FileSystem.CacheDirectory, image.FileName);
                    using Stream sourceStream = await image.OpenReadAsync();
                    using FileStream localStream = File.OpenWrite(localPath);
                    btnCamera.Source = localPath;
                    //set new image resource path ref to save resources elsewhere
                    idImageLocalPath = localPath;
                    await sourceStream.CopyToAsync(localStream);
                }
            }
            cameraBtnPressed = true;
            SetTime();
        }

        private void BtnClearFields_Clicked(object sender, EventArgs e)
        {
            SetUIToEmpty();
        }

        //starting up app with card icon as default and this bool set to false as the default state
        bool cashOrCard = false; 
        private void BtnCashOrCard_Clicked(object sender, EventArgs e)
        {           
            cashOrCard = !cashOrCard;
            if(cashOrCard) //true is case
            {
                defaultCashOrCardImage = "cashicon.png";               
            }
            else
            {
                defaultCashOrCardImage = "cardicon.png";
            }
            btnCashOrCard.Source = defaultCashOrCardImage;
        }

        private void ChkCashORCard_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            return;
        }
        private async void BtnRefreshHiresList_Clicked(object sender, EventArgs e)
        {
            //IsRefreshing = true;
            collectionView.ItemsSource = await App.Db.LinqAllRecordsAsync();
            //IsRefreshing = false;
        }

        private void SUPSwipeItem_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 1;
            imgSourceString = "SUP";
            hireTypeSelectedPrice = 15;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void SupLed1hr_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 1;
            imgSourceString = "SUP LED";
            hireTypeSelectedPrice = 20;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Sup2HrTour_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 1;
            imgSourceString = "SUP TOUR";
            hireTypeSelectedPrice = 35;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void SupHalfDayTour_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 1;
            imgSourceString = "SUP HDAY";
            hireTypeSelectedPrice = 50;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Sup1Hr_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 1;
            imgSourceString = "SUP";
            hireTypeSelectedPrice = 15;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Tandem_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 3;
            imgSourceString = "TANDEM";
            hireTypeSelectedPrice = 20;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Quattro_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 4;
            imgSourceString = "QUATTRO";
            hireTypeSelectedPrice = 30;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Solo_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 2;
            imgSourceString = "SOLO";
            hireTypeSelectedPrice = 10;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Croc_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 5;
            imgSourceString = "CROC";
            hireTypeSelectedPrice = 10;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Pedalo_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 5;
            imgSourceString = "PEDALO";
            hireTypeSelectedPrice = 20;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Boat30m_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 5;
            imgSourceString = "BOAT 30M";
            hireTypeSelectedPrice = 60;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Boat1Hr_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 5;
            imgSourceString = "BOAT 1HR";
            hireTypeSelectedPrice = 120;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void RowBoat_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 5;
            imgSourceString = "ROW BOAT";
            hireTypeSelectedPrice = 30;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Canoe_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 5;
            imgSourceString = "ROW BOAT";
            hireTypeSelectedPrice = 30;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Float_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 7;
            imgSourceString = "FLOAT";
            hireTypeSelectedPrice = 5;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }

        private void Paddle_Invoked(object sender, EventArgs e)
        {
            imgSourceNumber = 7;
            imgSourceString = "PADDLE";
            hireTypeSelectedPrice = 5;
            lblHirePrice.Text = $"£ {hireTypeSelectedPrice}";
        }
    }
}