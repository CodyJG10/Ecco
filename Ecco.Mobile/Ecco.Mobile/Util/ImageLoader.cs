using Ecco.Api;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecco.Mobile.Util
{
    public static class ImageLoader
    {
        public static ImageSource LoadCardImage(Entities.Card card, string username)
        {
            IStorageManager storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            var imageStream = storage.GetCard(username, card.CardTitle);
            return ImageSource.FromStream(() => new MemoryStream(imageStream.ToArray()));
        }
    }
}