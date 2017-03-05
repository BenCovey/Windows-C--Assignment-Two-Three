using NamedayDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NamedayDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    //StorageFolder SF = ApplicationData.Current.LocalFolder;
    public  partial class MainPage : Page
    {
        Boolean Edit = false;
        Boolean New = false;
        public MainPage()
        {
            this.InitializeComponent();
            btnSave.IsEnabled = false;
            Title.IsEnabled = false;
            Content.IsEnabled = false;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
            Debug.WriteLine("App Bar Button");
        }

        private async void AddNote(object sender, RoutedEventArgs e)
        {
            string title = null;
            title = await InputTextDialogAsync("New Note Name");
            //Edit = true;
            //btnSave.IsEnabled = true;
            //btnAdd.IsEnabled = false;
            //Title.IsEnabled = true;
            //Content.IsEnabled = true;
            if (title != null)
            {
                Debug.WriteLine("Added Note");
                Title.Text = title;
                Content.Text = "";
                New = true;
                SelectNewNote(title);
                
            }
        }

        private static void SelectNewNote(string title)
        {
            MainPageData.AddNewNote(title, "");
        }

        private void EditNote(object sender, RoutedEventArgs e)
        {
            if (Edit)
            {
                Edit = false;
                btnSave.IsEnabled = false;
                btnAdd.IsEnabled = true;
                //Title.IsEnabled = false;
                Content.IsEnabled = false;
            }
            else
            {
                Edit = true;
                btnSave.IsEnabled = true;
                btnAdd.IsEnabled = false;
                //Title.IsEnabled = true;
                Content.IsEnabled = true;
            }
            Debug.WriteLine("Edit Note");
        }

        private void SaveNote(object sender, RoutedEventArgs e)
        {
            string text = Content.Text;
            MainPageData.Save(text);
            Debug.WriteLine("Saved Note");
        }

        private async void DeleteNote(object sender, RoutedEventArgs e)
        {
            var title = "Delete Note";
            var content = "Are you sure you would like to Delete the current note?";

            var Delete = new UICommand("Yes");
            var Nothing = new UICommand("No");

            var dialog = new MessageDialog(content, title);
            dialog.Options = MessageDialogOptions.None;
            dialog.Commands.Add(Delete);

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            if (Nothing != null)
            {
                dialog.Commands.Add(Nothing);
                dialog.CancelCommandIndex = (uint)dialog.Commands.Count - 1;
            }

            var command = await dialog.ShowAsync();

            if (command == Delete)
            {
                MainPageData.DeleteNote();
            }
            else if (command == Nothing)
            {
                // handle no command
            }
            else
            {
                // handle cancel command
            }
            
            Debug.WriteLine("Delete Note");
        }

        private async void ExitApp(object sender, RoutedEventArgs e)
        {
            var title = "Exit Program";
            var content = "Are you sure you would like to exit the program?";

            var yesCommand = new UICommand("Yes");
            var noCommand = new UICommand("No");
            
            var dialog = new MessageDialog(content, title);
            dialog.Options = MessageDialogOptions.None;
            dialog.Commands.Add(yesCommand);

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            if (noCommand != null)
            {
                dialog.Commands.Add(noCommand);
                dialog.CancelCommandIndex = (uint)dialog.Commands.Count - 1;
            }

            var command = await dialog.ShowAsync();

            if (command == yesCommand)
            {
                CoreApplication.Exit();
            }
            else if (command == noCommand)
            {
                // handle no command
            }
            else
            {
                // handle cancel command
            }
        }

        private async void AboutApp(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("USER: Ben Covey");
            await dialog.ShowAsync();
        }

        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }
    }
}
