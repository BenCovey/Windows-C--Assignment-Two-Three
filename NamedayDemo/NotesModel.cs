using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedayDemo
{
    public class NotesModel
    {
        public string Title {
            get; set; 
        }
        public string Content {
            get; set;
        }
        public int ID
        {
            get;
        }

        //public IEnumerable<string> Names { get; }
        // IEnumerable<string> names
        public NotesModel(int num, string title, string content)
        {
            ID = num;
            Title = title;
            Content = content;
        }
        
        //public string NamesAsString => string.Join(", ", Names);
    }
}
