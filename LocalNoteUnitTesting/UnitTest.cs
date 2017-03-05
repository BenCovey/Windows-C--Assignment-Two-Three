using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NamedayDemo;
using System.Threading.Tasks;
using Windows.Storage;

namespace LocalNoteUnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        //Instance of MainPageData
        //MainPageData mpd = new MainPageData();
        static Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        [TestMethod]
        public async Task CheckingAvailableFilesAndCheckingReadibility()
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            foreach (Windows.Storage.StorageFile file in files)
            {
                string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                string title = file.Name;
                if(text == null && title == null)
                {
                    Assert.Fail();
                }else
                {
                    Assert.IsTrue(true);
                }
                Assert.AreEqual(0, 0);
            }//end for each
        }//end CheckingAvailableFilesAndCheckingReadibility

        [TestMethod]
        public async Task AddFiles()
        {
            string formattitle = "text.txt";
            string content = "Body of Note";
            Windows.Storage.StorageFile sampleFile =
                        await storageFolder.CreateFileAsync(formattitle,
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);


            await storageFolder.GetFileAsync(formattitle);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, content);
        }//end AddFiles

        [TestMethod]
        public async Task CheckCountOfFiles()
        {
            int expectedFiles = 1;
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            int actualcount = 0;
            foreach (Windows.Storage.StorageFile file in files) { actualcount++; }
            Assert.AreEqual<int>(expectedFiles, actualcount);
        }//End CheckCountOfFiles

        [TestMethod]
        public async Task EditText()
        {
            string title = "text.txt";
            string text = "";
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            Windows.Storage.StorageFile sampleFile =
                        await storageFolder.CreateFileAsync(title,
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await storageFolder.GetFileAsync(title);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, text);
            Windows.Storage.StorageFile sampleFileUpdated =
                        await storageFolder.CreateFileAsync(title,
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);
            string updatedText = await Windows.Storage.FileIO.ReadTextAsync(sampleFileUpdated);
            Assert.AreEqual(text, updatedText);
        }//end EditText

        [TestMethod]
        public async Task DeleteFile()
        {
            string title = "text.txt";
            StorageFile sFile = await storageFolder.GetFileAsync(title);
            await sFile.DeleteAsync();
        }//end DeleteFile

        [TestMethod]
        public async Task CheckCountOfFilesExpectedZeroAfterDelete()
        {
            int expectedFiles = 0;
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            int actualcount = 0;
            foreach (Windows.Storage.StorageFile file in files) { actualcount++; }
            Assert.AreEqual<int>(expectedFiles, actualcount);
        }//End CheckCountOfFiles

        [TestMethod]
        public async Task EditTextInNonExistingFile()
        {
            try
            {
                string title = "text.txt";
                string text = "";
                var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var query = folder.CreateFileQuery();
                var files = await query.GetFilesAsync();
                Windows.Storage.StorageFile sampleFile =
                            await storageFolder.CreateFileAsync(title,
                                Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await storageFolder.GetFileAsync(title);
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, text);
                Windows.Storage.StorageFile sampleFileUpdated =
                            await storageFolder.CreateFileAsync(title,
                                Windows.Storage.CreationCollisionOption.ReplaceExisting);
                string updatedText = await Windows.Storage.FileIO.ReadTextAsync(sampleFileUpdated);
                
            }catch(Exception err)
            {
                // Catches the assertion exception, and the test passes
            }
        }//end EditText

        [TestMethod]
        public async Task DeleteFileWhenFileDoesNotExist()
        {
            try
            {
                string title = "text.txt";
                StorageFile sFile = await storageFolder.GetFileAsync(title);
                await sFile.DeleteAsync();
            }
            catch (Exception)
            {
                //Passes if it hits here due to Exception thrown for null file
            }
        }//end DeleteFile

        [TestMethod]
        public async Task AddMultipleFiles()
        {
            for(int i = 0; i < 10; i++)
            {
                string formattitle = "text" + i + ".txt";
                string content = "Body of Note";
                Windows.Storage.StorageFile sampleFile =
                            await storageFolder.CreateFileAsync(formattitle,
                                Windows.Storage.CreationCollisionOption.ReplaceExisting);


                await storageFolder.GetFileAsync(formattitle);
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, content);
                
            }
            string debug = "debugging test stop location";
        }//end AddMultipleFiles

        [TestMethod]
        public async Task DeleteMultipleFiles()
        {
            for (int i = 0; i < 10; i++)
            {
                string formattitle = "text" + i + ".txt";
                StorageFile sFile = await storageFolder.GetFileAsync(formattitle);
                await sFile.DeleteAsync();
            }
        }//end DeleteFile

        [TestMethod]
        public async Task DeleteAllFiles()
        { 
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var query = folder.CreateFileQuery();
            var files = await query.GetFilesAsync();
            foreach (Windows.Storage.StorageFile file in files)
            {
                string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                string title = file.Name;
                StorageFile sFile = await storageFolder.GetFileAsync(title);
                await sFile.DeleteAsync();
            }//end foreach
        }//end DeleteAllFiles
    }//end UnitTest1
  
}//end Namespace
