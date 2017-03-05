using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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

        public void update()
        {
            PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("NoteModel"));
        }

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
                    new PropertyChangedEventArgs("NotesTemplate"));
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("NoteModel"));
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

                    //FilterNotes.Add(new NotesModel(id, title, text));
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

        public static void AddNewNote(string title, string content)
        {
            int Id = Convert.ToInt32(NoteModel.LongCount()) + 1;
            NoteModel.Add(new NotesModel(Id, title, content));
            //title = title + ".txt";
            SaveNew(title, content);
            
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
            //refillList();


        }

        public async static void refillList()
        {
            NoteModel.Clear();
            fillList();

        }

        private void PerformFiltering()
        {
            string _filter = "";
            if (_filter == null)
                _filter = "";

            string Filter = null;
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



        public CheckCommand CheckCommand { get; }

        public MainPageData()
        {
            CheckCommand v = new CheckCommand(this);
            NoteModel = new ObservableCollection<NotesModel>();
            //for (int i = 1; i <= 5; i++)
            //{
            //    string Title = "Title goes here " + i;
            //    string Body = "Body goes here " + i;
            //    NoteModel.Add(new NotesModel(id,Title, Body));
            //    id++;
            //}
            
            fillList();



        }
    }

   
}
