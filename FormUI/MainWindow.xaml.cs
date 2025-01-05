using FormUI.Model;
using FormUI.Services;
using FormUI.Utility;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using WinUI.TableView;


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
                    CMessage cMessage = new() { Message = message,Recipient=recipient };
                    //Send object to api service to post data
                    string Result = await _apiService.SendMessageAsync(cMessage);
                    if (Result != "NO" && !Result.Contains("error"))
                    {
                        UMessageDialog.ShowDialog( "Twilo has sent Message!, Code  = "+ Result, this.Content.XamlRoot);
                        PhoneNumberTextBox.Text="";
                        MessageTextBox.Text="";
                        LoadData();
                    }
                    else
                    {
                        UMessageDialog.ShowError("Failed to send the message. Please try again.", this.Content.XamlRoot);
                    }
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
