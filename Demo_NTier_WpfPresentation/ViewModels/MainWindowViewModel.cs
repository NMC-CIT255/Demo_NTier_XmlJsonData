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

        public ICommand SortListByAgeCommand
        {
            get { return new DelegateCommand(OnSortListByAge); }
        }

        public ICommand SortCharacterListCommand
        {
            get { return new RelayCommand(new Action<object>(OnSortCharacterList)); }
        }

        public ICommand DeleteCharacterCommand
        {
            get { return new DelegateCommand(OnDeleteCharacter); }
        }

        public ICommand EditCharacterCommand
        {
            get { return new DelegateCommand(OnEditCharacter); }
        }

        public ICommand SaveCharacterCommand
        {
            get { return new DelegateCommand(OnSaveCharacter); }
        }

        public ICommand AddCharacterCommand
        {
            get { return new DelegateCommand(OnAddCharacter); }
        }

        public ICommand CancelCharacterCommand
        {
            get { return new DelegateCommand(OnCancelCharacter); }
        }

        public ICommand ViewCharacterCommand
        {
            get { return new DelegateCommand(OnViewCharacter); }
        }

        public ICommand QuitApplicationCommand
        {
            get { return new DelegateCommand(OnQuitApplication); }
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
                _selectedCharacter = value;
                OnPropertyChanged("SelectedCharacter");
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

        #endregion

        #region CONSTRUCTORS

        public MainWindowViewModel(FlintstoneCharacterBusiness fcBusiness)
        {
            _fcBusiness = fcBusiness;
            _characters = new ObservableCollection<FlintstoneCharacter>(fcBusiness.AllFlintstoneCharacters());
            UpdateImagePath();

            //SortCharacterListCommand = new RelayCommand(new Action<object>(OnSortCharacterList));
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
                        //
                        // update character in persistence
                        //
                        _fcBusiness.UpdateFlintstoneCharacter(DetailedViewCharacter);

                        //
                        // update character in list - update view
                        _characters.Remove(characterToUpdate);
                        _characters.Add(DetailedViewCharacter);
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


        private void OnSortCharacterList(object obj)
        {
            string sortType = obj.ToString();
            switch (_sortType)
            {
                case "Age":

                    break;

                default:
                    break;
            }
            Characters = new ObservableCollection<FlintstoneCharacter>(Characters.OrderBy(c => c.Age));
        }

        #endregion

        #region EVENTS


        #endregion


    }
}
