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
        private bool _showAddButton = true;

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
                if (_detailedViewCharacter == value)
                {
                    return;
                }
                _detailedViewCharacter = value;
                OnPropertyChanged("DetailedViewCharacter");
            }
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

        public bool ShowAddButton
        {
            get { return _showAddButton; }
            set
            {
                _showAddButton = value;
                OnPropertyChanged(nameof(ShowAddButton));
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
                    _selectedCharacter = _characters[0];
                    UpdateDetailedViewCharacterToSelected();
                }
            }
        }

        private void OnEditCharacter()
        {
            if (_selectedCharacter != null)
            {
                _operationStatus = OperationStatus.EDIT;
                IsEditingAdding = true;
                ShowAddButton = false;
                UpdateDetailedViewCharacterToSelected();
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
            ShowAddButton = false;
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
                    }
                    break;
                default:
                    break;
            }

            ResetDetailedViewCharacter();
            IsEditingAdding = false;
            ShowAddButton = true;
            _operationStatus = OperationStatus.NONE;
        }

        private void OnCancelCharacter()
        {
            ResetDetailedViewCharacter();
            _operationStatus = OperationStatus.NONE;
            IsEditingAdding = false;
            ShowAddButton = true;
        }

        private void UpdateDetailedViewCharacterToSelected()
        {
            _detailedViewCharacter = new FlintstoneCharacter();
            _detailedViewCharacter.Id = _selectedCharacter.Id;
            _detailedViewCharacter.FirstName = _selectedCharacter.FirstName;
            _detailedViewCharacter.LastName = _selectedCharacter.LastName;
            _detailedViewCharacter.Age = _selectedCharacter.Age;
            _detailedViewCharacter.Gender = _selectedCharacter.Gender;
            _detailedViewCharacter.AverageAnnualGross = _selectedCharacter.AverageAnnualGross;
            _detailedViewCharacter.HireDate = _selectedCharacter.HireDate;
            _detailedViewCharacter.Description = _selectedCharacter.Description;
            _detailedViewCharacter.ImageFileName = _selectedCharacter.ImageFileName;
            _detailedViewCharacter.ImageFilePath = _selectedCharacter.ImageFilePath;
            OnPropertyChanged("DetailedViewCharacter");
        }

        private void ResetDetailedViewCharacter()
        {
            _detailedViewCharacter = new FlintstoneCharacter();
            _detailedViewCharacter.FirstName = "";
            _detailedViewCharacter.LastName = "";
            _detailedViewCharacter.Age = 0;
            _detailedViewCharacter.Gender = FlintstoneCharacter.GenderType.None;
            _detailedViewCharacter.AverageAnnualGross = 0;
            _detailedViewCharacter.HireDate = DateTime.Today;
            _detailedViewCharacter.Description = "";
            _detailedViewCharacter.ImageFileName = "";
            _detailedViewCharacter.ImageFilePath = "";
            OnPropertyChanged("DetailedViewCharacter");
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
