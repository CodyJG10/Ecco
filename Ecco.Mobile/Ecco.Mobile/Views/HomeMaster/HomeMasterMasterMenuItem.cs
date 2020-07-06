using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Mobile.Views.HomeMaster
{

    public class HomeMasterMasterMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Action OnClicked;
    }
}