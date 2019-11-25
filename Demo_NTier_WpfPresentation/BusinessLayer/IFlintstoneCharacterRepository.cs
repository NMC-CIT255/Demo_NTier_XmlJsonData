using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_XmlJsonData.Models;

namespace Demo_NTier_XmlJsonData.BusinessLayer
{
    public interface IFlintstoneCharacterRepository
    {
        IEnumerable<FlintstoneCharacter> GetAll();
        FlintstoneCharacter GetById(int id);
        void Add(FlintstoneCharacter character);
        void Update(FlintstoneCharacter character);
        void Delete(int id);
    }
}
