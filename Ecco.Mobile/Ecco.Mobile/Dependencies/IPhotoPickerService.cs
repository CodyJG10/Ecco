using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Mobile.Dependencies
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}