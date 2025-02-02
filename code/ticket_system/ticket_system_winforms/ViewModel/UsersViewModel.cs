using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ticket_system_winforms.DAL;
using ticket_system_winforms.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ticket_system_winforms.ViewModel
{
    internal class UsersViewModel : INotifyPropertyChanged
    {
        private UserDAL db = new UserDAL();

        private IList<User> users;
        private User selectedUser;
        public IList<User> Users
        {
            get => this.users;
            set
            {
                this.users = value;
                this.OnPropertyChanged();
            }
        }
        public User SelectedUser
        {
            get => this.selectedUser;
            set
            {
                this.selectedUser = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                this.SelectedUser = this.Users[0];
            }
            catch (Exception ex)
            {
                this.Users = new List<User>();
                this.users.Add(new User(1, "User1ID", "Username1", "Password1"));
                this.users.Add(new User(2, "User2ID", "Username2", "Password2"));
                this.users.Add(new User(3, "User3ID", "Username3", "Password3"));
                this.SelectedUser = this.Users[0];
            }
        }
    }
}
