using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReadsWebApi.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Books = new List<BookViewModel>();
        }

        public string Id { get; set; }
        public List<BookViewModel> Books { get; set; }
    }

    public class BookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Shelf { get; set; }
        public List<string> Authors { get; set; }
    }
}
