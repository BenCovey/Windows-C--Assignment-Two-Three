using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace NamedayDemo
{
    public class MainPageData : INotifyPropertyChanged
    {
        static Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        // Defaults to "Hello" if not set
        int id = 0;
        public string Greeting { get; set; } = "No Note Selected";
        public string Content { get; set; } = "No Note Selected";
        // List of NamedayModel classes
        public static ObservableCollection<NotesModel> NoteModel
        {
            get; set;
        }

        public static ObservableCollection<NotesModel> FilterNotes
        {
            get; set;
        }

        public static NotesModel _selectedNote;

        public  event PropertyChangedEventHandler PropertyChanged;

        public  NotesModel SelectedNote
        {
            get
            {
                return _selectedNote;
            }
            set
            {
                _selectedNote = value;
                if (value == null)
                {
                    Greeting = "No Note Selected";
                   
                } else
                {
                    Greeting = _selectedNote.Title;
                    Content = _selectedNote.Content;
                }
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Greeting"));
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Content"));
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("FilterNotes"));
                //CheckCommand.FireCanExecuteChanged();
            }
        }

        public async static void fillList()
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            foreach (Windows.Storage.StorageFile file in files)
            {
                string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                string title = file.Name;
                title = title.Replace(".txt", "");
                try
                {
                    int id = NoteModel.Count;
                    id++;
                    NoteModel.Add(new NotesModel(id, title, text));
                    FilterNotes.Add(new NotesModel(id, title, text));
                }
                catch (Exception err) {
                    string error = err.Message;
                }
            }

        }

        public async static void Save(string text)
        {

            _selectedNote.Content = text;
            string title = _selectedNote.Title;
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            var titleformat = title + ".txt";
            
            Windows.Storage.StorageFile sampleFile =
                        await storageFolder.CreateFileAsync(titleformat,
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);

            await storageFolder.GetFileAsync(titleformat);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, text);
        }

        public async static void SaveNew(string title, string content)
        {
            String formattitle = title + ".txt";
            Windows.Storage.StorageFile sampleFile =
                        await storageFolder.CreateFileAsync(formattitle,
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);


            await storageFolder.GetFileAsync(formattitle);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, content);

        }

        public static async void  AddNewNote(string title, string content)
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            Boolean samename = false;
            foreach (Windows.Storage.StorageFile file in files)
            {
                string filetitle = file.Name;
                filetitle = filetitle.Replace(".txt", "");
                if(filetitle.Equals(title))
                {
                    samename = true;
                    var OkCommand = new UICommand("Ok");
                    var dialog = new MessageDialog("Name already used. Please pick another", "Invalid Title");
                    dialog.Options = MessageDialogOptions.None;
                    dialog.Commands.Add(OkCommand);
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 0;
                    
                }
            }
            if (samename != true)
            {
                int Id = Convert.ToInt32(NoteModel.LongCount()) + 1;
                NoteModel.Add(new NotesModel(Id, title, content));
                FilterNotes.Add(new NotesModel(Id, title, content));
                //title = title + ".txt";
                SaveNew(title, content);
            }

            //_selectedNote = NoteModel[Id];
        }

        public async static void DeleteNote()
        {

            int Id = _selectedNote.ID;
            string title = _selectedNote.Title;
            title = title + ".txt";
            StorageFile sFile = await storageFolder.GetFileAsync(title);
            await sFile.DeleteAsync();
            
            NoteModel.RemoveAt(Id);
            FilterNotes.RemoveAt(Id);
            //refillList();


        }

        public static void ShowAll()
        {
            FilterNotes.Clear();
            foreach (NotesModel note in NoteModel)
            {
                FilterNotes.Add(note);
            }
        }

        public static void PerformFiltering(string Filter)
        {
            if (Filter.Trim() == null || Filter.Equals(""))
            {
                FilterNotes.Clear();
                foreach (NotesModel note in NoteModel)
                {
                    FilterNotes.Add(note);
                }

            }
            else
            {
                var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

                var result =
                    NoteModel.Where(d => d.Title.ToLowerInvariant()
                    .Contains(lowerCaseFilter))
                    .ToList();

                var toRemove = FilterNotes.Except(result).ToList();

                foreach (var x in toRemove)
                    FilterNotes.Remove(x);

                var resultCount = result.Count;
                for (int i = 0; i < resultCount; i++)
                {
                    var resultItem = result[i];
                    if (i + 1 > FilterNotes.Count || !FilterNotes[i].Equals(resultItem))
                        FilterNotes.Insert(i, resultItem);
                }
            }
        }

        //    FilterNotes.Clear();
        //    try
        //    {
        //        foreach (NotesModel note in NoteModel)
        //        {
        //            for (int i = 0; i < results.Count; i++)
        //            {
        //                int id = results[i];
        //                if (note.ID != id)
        //                {
        //                    FilterNotes.Add(NoteModel[id]);
        //                }//end if
        //            }//end for
        //        }//end foreach
        //    }
        //    catch (Exception) { }
        //}//end filtering

        public CheckCommand CheckCommand { get; }

        public MainPageData()
        {

            CheckCommand v = new CheckCommand(this);
            NoteModel = new ObservableCollection<NotesModel>();
            FilterNotes = new ObservableCollection<NotesModel>();
            fillList();
            //PerformFiltering();
        }
    }

   
}
