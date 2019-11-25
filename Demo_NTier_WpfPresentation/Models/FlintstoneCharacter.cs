using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Demo_NTier_XmlJsonData;
using System.ComponentModel;

namespace Demo_NTier_XmlJsonData.Models
{
    public class FlintstoneCharacter : ObservableObject
    {
        #region ENUMS

        public enum GenderType { None, Male, Female }

        #endregion

        #region FIELDS

        private int _id;
        private string _firstName;
        private string _lastName;
        private int _age;
        private GenderType _gender;
        private string _imageFileName;
        private string _imageFilePath;
        private string _description;
        private DateTime _hireDate;
        private double _averageAnnualGross;
        private List<GroceryItem> _groceryList;

        #endregion

        #region PROPERTIES

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(FullName));
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public GenderType Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string ImageFileName
        {
            get { return _imageFileName; }
            set
            {
                _imageFileName = value;
                OnPropertyChanged(nameof(ImageFileName));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime HireDate
        {
            get { return _hireDate; }
            set
            {
                _hireDate = value;
                OnPropertyChanged(nameof(HireDate));
            }
        }

        public double AverageAnnualGross
        {
            get { return _averageAnnualGross; }
            set
            {
                _averageAnnualGross = value;
                OnPropertyChanged(nameof(AverageAnnualGross));
            }
        }

        public string FullName
        {
            get { return _firstName + " " + _lastName; }
        }

        public string ImageFilePath
        {
            get { return _imageFilePath; }
            set
            {
                _imageFilePath = value;
                OnPropertyChanged(nameof(ImageFilePath));
            }
        }

        public List<GroceryItem> GroceryList
        {
            get { return _groceryList; }
            set { _groceryList = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public FlintstoneCharacter()
        {

        }

        #endregion

        #region METHODS


        #endregion

        #region EVENTS



        #endregion

    }
}
