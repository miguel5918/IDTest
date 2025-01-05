using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormUI.Utility
{
    public  class UMessageDialog
    {
        //Error method to be called when is needed
        public async static void ShowError(string message, XamlRoot xamlRoot)
        {
            //show and alert on screem
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = xamlRoot
            };
            await dialog.ShowAsync();
        }

        public async static void ShowDialog(string message, XamlRoot xamlRoot)
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = xamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
