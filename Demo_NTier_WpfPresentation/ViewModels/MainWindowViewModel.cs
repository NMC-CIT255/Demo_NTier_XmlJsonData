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
        private enum OperationStatus
        {
            NONE,
            VIEW,
            ADD,
            EDIT,
            DELETE
        }


        #region COMMANDS

        public ICommand SortCharactersListCommand
        {
            get { return new RelayCommand(new Action<object>(OnSortCharactersList)); }
        }

        public ICommand SearchCharactersListCommand
        {
            get { return new RelayCommand(OnSearchCharactersList); }
        }

        public ICommand AgeFilterCharactersListCommand
        {
            get { return new RelayCommand(OnAgeFilterCharactersList); }
        }

        public ICommand ResetCharactersListCommand
        {
            get { return new RelayCommand(OnResetCharactersList); }
        }

        public ICommand DeleteCharacterCommand
        {
            get { return new RelayCommand(OnDeleteCharacter); }
        }

        public ICommand EditCharacterCommand
        {
            get { return new RelayCommand(OnEditCharacter); }
        }

        public ICommand SaveCharacterCommand
        {
            get { return new RelayCommand(OnSaveCharacter); }
        }

        public ICommand AddCharacterCommand
        {
            get { return new RelayCommand(OnAddCharacter); }
        }

        public ICommand CancelCharacterCommand
        {
            get { return new RelayCommand(OnCancelCharacter); }
        }

        public ICommand ViewCharacterCommand
        {
            get { return new RelayCommand(OnViewCharacter); }
        }

        public ICommand QuitApplicationCommand
        {
            get { return new RelayCommand(OnQuitApplication); }
        }

        #endregion

        #region ENUMS



        #endregion

        #region FIELDS

        private ObservableCollection<FlintstoneCharacter> _characters;
        private FlintstoneCharacter _selectedCharacter;
        private FlintstoneCharacter _detailedViewCharacter;
        private FlintstoneCharacterBusiness _fcBusiness;
        private OperationStatus _operationStatus = OperationStatus.NONE;

        private bool _isEditingAdding = false;
        private bool _showAddEditDeleteButtons = true;

        private string _sortType;
        private string _searhText;
        private string _minAgeText;
        private string _maxAgeText;


        #endregion

        #region PROPERTIES

        public ObservableCollection<FlintstoneCharacter> Characters
        {
            get { return _characters; }
            set
            {
                _characters = value;
                OnPropertyChanged(nameof(Characters));
            }
        }

        public FlintstoneCharacter DetailedViewCharacter
        {
            get { return _detailedViewCharacter; }
            set
            {
                if (value != null)
                {
                    _detailedViewCharacter = value;
                    OnPropertyChanged("DetailedViewCharacter");
                }
            }
        }

        public FlintstoneCharacter SelectedCharacter
        {
            get { return _selectedCharacter; }
            set
            {
                if (value != null)
                {
                    _selectedCharacter = value;
                    OnPropertyChanged("SelectedCharacter");
                    UpdateDetailedViewCharacterToSelected();
                }
            }
        }

        public bool IsEditingAdding
        {
            get { return _isEditingAdding; }
            set
            {
                _isEditingAdding = value;
                OnPropertyChanged(nameof(IsEditingAdding));
            }
        }

        public bool ShowAddEditDeleteButtons
        {
            get { return _showAddEditDeleteButtons; }
            set
            {
                _showAddEditDeleteButtons = value;
                OnPropertyChanged(nameof(ShowAddEditDeleteButtons));
            }
        }

        public string SortType
        {
            get { return _sortType; }
            set { _sortType = value; }
        }

        public string SearchText
        {
            get { return _searhText; }
            set
            {
                _searhText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }


        public string MaxAgeText
        {
            get { return _maxAgeText; }
            set
            {
                _maxAgeText = value;
                OnPropertyChanged(nameof(MaxAgeText));
            }
        }


        public string MinAgeText
        {
            get { return _minAgeText; }
            set
            {
                _minAgeText = value;
                OnPropertyChanged(nameof(MinAgeText));
            }
        }

        #endregion

        #region CONSTRUCTORS

        public MainWindowViewModel(FlintstoneCharacterBusiness fcBusiness)
        {
            _fcBusiness = fcBusiness;
            _characters = new ObservableCollection<FlintstoneCharacter>(_fcBusiness.AllFlintstoneCharacters());
            UpdateImagePath();

            //
            // set SelectedCharacter property to first in list
            //
            _selectedCharacter = _characters[0];
            UpdateDetailedViewCharacterToSelected();
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
            if (_selectedCharacter != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"Are you sure you want to delete {_selectedCharacter.FullName}?", "Delete Character", System.Windows.MessageBoxButton.OKCancel);

                if (messageBoxResult == MessageBoxResult.OK)
                {
                    //
                    // delete character from persistence
                    //
                    _fcBusiness.DeleteFlintstoneCharacter(SelectedCharacter.Id);

                    //
                    // remove character from list - update view
                    //
                    _characters.Remove(_selectedCharacter);

                    //
                    // set SelectedCharacter property to first in list
                    //
                    SelectedCharacter = _characters[0];
                }
            }
        }

        private void OnEditCharacter()
        {
            if (_selectedCharacter != null)
            {
                _operationStatus = OperationStatus.EDIT;
                IsEditingAdding = true;
                ShowAddEditDeleteButtons = false;
            }
        }

        private void OnViewCharacter()
        {
            if (_selectedCharacter != null)
            {
                _operationStatus = OperationStatus.VIEW;
                UpdateDetailedViewCharacterToSelected();
            }
        }

        private void OnAddCharacter()
        {
            ResetDetailedViewCharacter();
            IsEditingAdding = true;
            ShowAddEditDeleteButtons = false;
            _operationStatus = OperationStatus.ADD;
        }

        private void OnSaveCharacter()
        {
            switch (_operationStatus)
            {
                case OperationStatus.ADD:
                    if (_detailedViewCharacter != null)
                    {
                        //
                        // add character to persistence
                        //
                        _fcBusiness.AddFlintstoneCharacter(_detailedViewCharacter);

                        //
                        // add character to list - update view
                        //
                        _characters.Add(DetailedViewCharacter);

                        //
                        // set SelectedCharacter property to the new character
                        //
                        SelectedCharacter = DetailedViewCharacter;
                    }
                    break;

                case OperationStatus.EDIT:
                    FlintstoneCharacter characterToUpdate = _characters.FirstOrDefault(c => c.Id == SelectedCharacter.Id);

                    if (characterToUpdate != null)
                    {
                        FlintstoneCharacter updatedCharacter = DetailedViewCharacter;

                        //
                        // update character in persistence
                        //
                        _fcBusiness.UpdateFlintstoneCharacter(updatedCharacter);

                        //
                        // update character in list - update view
                        _characters.Remove(characterToUpdate);
                        _characters.Add(updatedCharacter);

                        //
                        // set SelectedCharacter property to updated character
                        //
                        SelectedCharacter = updatedCharacter;
                    }
                    break;

                default:

                    break;
            }

            IsEditingAdding = false;
            ShowAddEditDeleteButtons = true;
            _operationStatus = OperationStatus.NONE;
        }

        private void OnCancelCharacter()
        {
            if (_operationStatus == OperationStatus.ADD)
            {
                SelectedCharacter = _characters[0];
            }
            _operationStatus = OperationStatus.NONE;
            IsEditingAdding = false;
            ShowAddEditDeleteButtons = true;
        }

        private void UpdateDetailedViewCharacterToSelected()
        {
            FlintstoneCharacter tempCharacter = new FlintstoneCharacter();
            tempCharacter.Id = _selectedCharacter.Id;
            tempCharacter.FirstName = _selectedCharacter.FirstName;
            tempCharacter.LastName = _selectedCharacter.LastName;
            tempCharacter.Age = _selectedCharacter.Age;
            tempCharacter.Gender = _selectedCharacter.Gender;
            tempCharacter.AverageAnnualGross = _selectedCharacter.AverageAnnualGross;
            tempCharacter.HireDate = _selectedCharacter.HireDate;
            tempCharacter.Description = _selectedCharacter.Description;
            tempCharacter.ImageFileName = _selectedCharacter.ImageFileName;
            tempCharacter.ImageFilePath = _selectedCharacter.ImageFilePath;
            DetailedViewCharacter = tempCharacter;
        }

        private void ResetDetailedViewCharacter()
        {
            DetailedViewCharacter = new FlintstoneCharacter();
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
            Characters = new ObservableCollection<FlintstoneCharacter>(_characters.OrderBy(c => c.Age));
        }

        private void OnSortCharactersList(object obj)
        {
            string sortType = obj.ToString();
            switch (sortType)
            {
                case "Age":
                    Characters = new ObservableCollection<FlintstoneCharacter>(Characters.OrderBy(c => c.Age));
                    break;

                case "LastName":
                    Characters = new ObservableCollection<FlintstoneCharacter>(Characters.OrderBy(c => c.LastName));
                    break;

                default:
                    break;
            }
        }

        private void OnSearchCharactersList()
        {
            //
            // reset age filters
            //
            MinAgeText = "";
            MaxAgeText = "";

            //
            // reset to full list before search
            //
            _characters = new ObservableCollection<FlintstoneCharacter>(_fcBusiness.AllFlintstoneCharacters());
            UpdateImagePath();

            Characters = new ObservableCollection<FlintstoneCharacter>(_characters.Where(c => c.LastName.ToLower().Contains(_searhText)));
        }

        private void OnAgeFilterCharactersList()
        {
            //
            // reset search text box
            //
            SearchText = "";

            if (int.TryParse(MinAgeText, out int minAge) && int.TryParse(MaxAgeText, out int maxAge))
            {
                //
                // reset to full list before search
                //
                _characters = new ObservableCollection<FlintstoneCharacter>(_fcBusiness.AllFlintstoneCharacters());
                UpdateImagePath();

                Characters = new ObservableCollection<FlintstoneCharacter>(_characters.Where(c => c.Age >= minAge && c.Age <= maxAge));
            }
        }

        private void OnResetCharactersList()
        {
            //
            // reset search and filter text boxes
            //
            SearchText = "";
            MinAgeText = "";
            MaxAgeText = "";

            //
            // reset to full list 
            //
            _characters = new ObservableCollection<FlintstoneCharacter>(_fcBusiness.AllFlintstoneCharacters());
            UpdateImagePath();

            Characters = _characters;
        }

        #endregion

        #region EVENTS


        #endregion


    }
}
