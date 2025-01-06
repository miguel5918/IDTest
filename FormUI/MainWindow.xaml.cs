using FormUI.Model;
using FormUI.Services;
using FormUI.Utility;
using Microsoft.UI.Xaml;
using System;
using System.Linq;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FormUI
{

    public sealed partial class MainWindow : Window
    {

        private readonly ApiService _apiService;

       
        public MainWindow()
        {
            this.InitializeComponent();
            _apiService = new ApiService();

            LoadData();

             this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(300, 100, 800, 800));
        }

        public async void LoadData()
        {
            try
            {
                var LMessages = await _apiService.GetAllMessages();

                MyTableView.ItemsSource = LMessages.OrderByDescending(x => x.MessageId).ToList();
            }
            catch (Exception ex)
            {

                UMessageDialog.ShowError($"An error occurred: {ex.Message}", this.Content.XamlRoot);
            }
        
        }

       
        private async void ShowMessageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // get field values
                string recipient = PhoneNumberTextBox.Text;
                string message =   MessageTextBox.Text;

                bool error = false;

                //validate fields 
                if (string.IsNullOrWhiteSpace(recipient))
                {
                    errorPhone.Visibility = Visibility.Visible;
                    error = true;
                } 

                if (string.IsNullOrWhiteSpace(message))
                {
                    errorMessage.Visibility = Visibility.Visible;
                    error=true;
                }
                // if fields are filled
                if (error==false)
                {
                  var Numbers=  recipient.Split(',').ToList();
                    string Result = "";
                    string stext = "";
                    foreach (var number in Numbers) {
                        if (number!="")
                        {
                            CMessage cMessage = new() { Message = message, Recipient = number };

                            //Send object to api service to post data
                            Result = await _apiService.SendMessageAsync(cMessage);

                            if (Result != "NO" && !Result.Contains("error"))
                            {
                                stext += "Twilo has sent Message!, Code  = " + Result + ", To" + number + "\n";
                                PhoneNumberTextBox.Text = "";
                                MessageTextBox.Text = "";
                                LoadData();
                            }
                            else
                            {
                                stext += "Failed to send the message, to " + number;

                                break;
                            }
                        }                 
                       


                    }

                    //if (Result != "NO" && !Result.Contains("error"))
                    //{
                        UMessageDialog.ShowDialog(stext, this.Content.XamlRoot);
                        PhoneNumberTextBox.Text = "";
                        MessageTextBox.Text = "";
                        LoadData();
                    //}
                   


                }             




            }
            catch (Exception ex)
            {
                UMessageDialog.ShowError( $"An error occurred: {ex.Message}",this.Content.XamlRoot);
               
            }
        }

        //Error method to be called when is needed
       

        private void PhoneNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
            errorPhone.Visibility = Visibility.Collapsed;
            
        }
        private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            errorMessage.Visibility = Visibility.Collapsed;
          
        }

       
    }
}
