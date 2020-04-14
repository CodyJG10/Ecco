using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Dependencies
{
    public interface INFCReader
    {
        List<string> ReadTag();
    }
}