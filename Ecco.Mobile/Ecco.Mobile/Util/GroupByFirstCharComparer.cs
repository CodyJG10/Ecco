using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Util
{
    public class GroupByFirstCharComparer : IComparer<GroupResult>
    {
        public int Compare(GroupResult x, GroupResult y)
        {
            var result = string.Compare(x.Key.ToString(), y.Key.ToString());
            return result;
        }
    }
}
