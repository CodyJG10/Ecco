using Ecco.Api;
using Ecco.Entities.Company;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecco.Mobile.Util
{
    public static class TemplateUtil
    {
        public static async Task<ImageSource> LoadImageSource(Entities.Card card, IDatabaseManager db, IStorageManager storage)
        {
            var template = await db.GetTemplate(card.TemplateId);
            var templateImageStream = storage.GetTemplate(template.FileName);
            return ImageSource.FromStream(() => new MemoryStream(templateImageStream.ToArray()));
        }

        public static async Task<ImageSource> LoadImageSource(Company company, IDatabaseManager db, IStorageManager storage)
        {
            var template = await db.GetTemplate(company.TemplateId);
            var templateImageStream = storage.GetTemplate(template.FileName);
            return ImageSource.FromStream(() => new MemoryStream(templateImageStream.ToArray()));
        }
    }
}