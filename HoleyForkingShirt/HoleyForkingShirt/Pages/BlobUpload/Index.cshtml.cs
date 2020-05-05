using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HoleyForkingShirt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace HoleyForkingShirt.Pages.BlobUpload
{
    public class IndexModel : PageModel
    {
        public Blob Blob { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty]
        public string Name { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            Blob = new Blob(configuration);
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var path = Path.GetTempFileName();

            using(var stream = System.IO.File.Create(path))
            {
                await Image.CopyToAsync(stream);
            }

            await Blob.UploadFileAsync("products", Name, path);

            var blob = await Blob.GetBlobAsync(Name, "products");
            var uri = blob.Uri;

            return Page();
        }
    }
}
