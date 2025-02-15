using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;
public interface IBlobStorageService
{
    Task<string> UploadImageAsync(IFormFile imageFile);
}
