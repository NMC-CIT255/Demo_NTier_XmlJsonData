using Demo_NTier_XmlJsonData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_XmlJsonData.DataAccessLayer
{
    public interface IDataService
    {
        IEnumerable<FlintstoneCharacter> ReadAll();
        void WriteAll(IEnumerable<FlintstoneCharacter> characters);
    }
}
