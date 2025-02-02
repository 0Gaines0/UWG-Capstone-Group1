using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_system_winforms.DAL;
using ticket_system_winforms.Model;
using ticket_system_winforms.View.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ticket_system_winforms.ViewModel
{
    internal class UsersViewModel : INotifyPropertyChanged
    {
        private UsersDAL db = new UsersDAL();

        private IList<User> users;
        private int selectedUserIndex;
        private bool hasSelectedUser;
        public IList<User> Users
        {
            get => this.users;
            set
            {
                this.users = value;
                this.OnPropertyChanged();
            }
        }
        public int SelectedUserIndex
        {
            get => this.selectedUserIndex;
            set
            {
                this.selectedUserIndex = value;
                this.HasSelectedUser = (value >= 0 && value < this.Users.Count);
                this.OnPropertyChanged();
            }
        }
        public bool HasSelectedUser
        {
            get => this.hasSelectedUser;
            private set
            {
                this.hasSelectedUser = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UsersViewModel()
        {
            this.SelectedUserIndex = -1;
        }

        /// <summary>
        /// Gets the list of users from the database and sets properties accordingly.
        /// </summary>
        /// <precondition>true</precondition>
        /// <precondition>true</precondition>
        public void GetUsers()
        {
            try
            {
                this.Users = this.db.RetrieveAllUsers();
            }
            catch (Exception ex)
            {
                Form alert = new AlertDialog("Error", ex.ToString());
                alert.ShowDialog();
            }
            finally {
                this.SelectedUserIndex = this.SelectedUserIndex;
            }
        }

        /// <summary>
        /// Deletes the currently selected user from the database and updates the user list.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        public void DeleteUser()
        {
            this.db.DeleteUser(this.Users[this.SelectedUserIndex].ID);
            this.GetUsers();
        }
    }
}
