using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Controls
{
    public class SortAlphabetically : IComparer<GroupResult>
    {
        public int Compare(GroupResult x, GroupResult y)
        {
            if (x.Count > y.Count)
            {
                //GroupResult y is stacked into top of the group i.e., Ascending.
                //GroupResult x is stacked at the bottom of the group i.e., Descending.
                return 1;
            }
            else if (x.Count < y.Count)
            {
                //GroupResult x is stacked into top of the group i.e., Ascending.
                //GroupResult y is stacked at the bottom of the group i.e., Descending.
                return -1;
            }

            return 0;
        }
    }
}