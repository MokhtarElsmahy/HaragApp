using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class uploadFile 
    {
        public List<IFormFile> Files { get; set; }//for api post
    }
}
