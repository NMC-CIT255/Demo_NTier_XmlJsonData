using Demo_NTier_XmlJsonData.BusinessLayer;
using Demo_NTier_XmlJsonData.Models;
using Demo_NTier_XmlJsonData.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Demo_NTier_XmlJsonData;

namespace Demo_NTier_WpfPresentation.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        #region COMMANDS

        public ICommand DeleteCharacterCommand
        {
            get { return new DelegateCommand(OnDeleteCharacter); }
        }

        public ICommand QuitApplicationCommand
        {
            get { return new DelegateCommand(OnQuitApplication); }
        }

        public ICommand SortListByAgeCommand
        {
            get { return new DelegateCommand(OnSortListByAge); }
        }

        #endregion

        #region ENUMS



        #endregion

        #region FIELDS

        private ObservableCollection<FlintstoneCharacter> _characters;
        private FlintstoneCharacter _selectedCharacter;
        private FlintstoneCharacterBusiness _fcBusiness;

        #endregion

        #region PROPERTIES

        public ObservableCollection<FlintstoneCharacter> Characters
        {
            get { return _characters; }
            set { _characters = value; }
        }

        public FlintstoneCharacter SelectedCharacter
        {
            get { return _selectedCharacter; }
            set
            {
                if (_selectedCharacter == value)
                {
                    return;
                }
                _selectedCharacter = value;
                OnPropertyChanged("SelectedCharacter");
            }
        }

        #endregion

        #region CONSTRUCTORS

        public MainWindowViewModel(FlintstoneCharacterBusiness fcBusiness)
        {
            _fcBusiness = fcBusiness;
            _characters = new ObservableCollection<FlintstoneCharacter>(fcBusiness.AllFlintstoneCharacters());
            UpdateImagePath();
        }

        #endregion

        #region METHODS

        private void UpdateImagePath()
        {
            foreach (var character in _characters)
            {
                character.ImageFilePath = DataConfig.ImagePath + character.ImageFileName;
            }
        }

        private void OnDeleteCharacter()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"Are you sure you want to delete {_selectedCharacter.FullName}?", "Delete Character", System.Windows.MessageBoxButton.OKCancel);

            if (messageBoxResult == MessageBoxResult.OK)
            {
                _characters.Remove(_selectedCharacter);
            }
        }

        private void OnQuitApplication()
        {
            //
            // call a house keeping method in the business class
            //
            System.Environment.Exit(1);
        }

        private void OnSortListByAge()
        {
            _characters = new ObservableCollection<FlintstoneCharacter>(_characters.OrderBy(c => c.Age));
        }

        #endregion

        #region EVENTS


        #endregion


    }
}
