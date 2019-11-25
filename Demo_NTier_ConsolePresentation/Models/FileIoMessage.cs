using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_XmlJsonData.Models
{
    public enum FileIoMessage
    {
        None,
        Complete,
        FileAccessError,
        RecordNotFound,
        NoRecordsFound
    }
}
