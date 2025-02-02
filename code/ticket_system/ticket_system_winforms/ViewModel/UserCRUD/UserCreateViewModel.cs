using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ticket_system_winforms.DAL;

namespace ticket_system_winforms
{
    public class UserCreateViewModel : INotifyPropertyChanged
    {
        private UserDAL db = new UserDAL();

        private string userId;
        private string username;
        private string password;
        private string errMsg;
        public string UserID {
            get => this.userId;
            set {
                this.userId = value;
                this.OnPropertyChanged();
            }
        }
        public string Username
        {
            get => this.username;
            set
            {
                this.username = value;
                this.OnPropertyChanged();
            }
        }
        public string Password
        {
            get => this.password;
            set
            {
                this.password = value;
                this.OnPropertyChanged();
            }
        }
        public string ErrMsg
        {
            get => this.errMsg;
            set
            {
                this.errMsg = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a new User with the entered information.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <returns>True if successful, false otherwise.</returns>
        public bool Create()
        {
            try
            {
                this.db.CreateUser(this.UserID, this.Username, this.Password);
                this.ErrMsg = "Success"; //TODO remove this
                return true;
            }
            catch (Exception ex)
            {
                this.errMsg = ex.Message;
                this.ErrMsg = this.UserID + this.Username + this.Password; //TODO remove this
                return false;
            }
        }
    }
}
