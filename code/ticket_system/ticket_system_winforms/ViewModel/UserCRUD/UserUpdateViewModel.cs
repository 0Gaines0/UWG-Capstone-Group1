﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ticket_system_winforms.DAL;
using ticket_system_winforms.Model;

namespace ticket_system_winforms.ViewModel
{
    internal class UserUpdateViewModel : INotifyPropertyChanged
    {
        private UserDAL db = new UserDAL();

        private int id;
        private string currentUserId;
        private string currentUsername;
        private string currentPassword;
        private string newUserId;
        private string newUsername;
        private string newPassword;
        private string errMsg;

        public string CurrentUserID
        {
            get => this.currentUserId;
            set
            {
                this.currentUserId = value;
                this.OnPropertyChanged();
            }
        }
        public string CurrentUsername
        {
            get => this.currentUsername;
            set
            {
                this.currentUsername = value;
                this.OnPropertyChanged();
            }
        }
        public string CurrentPassword
        {
            get => this.currentPassword;
            set
            {
                this.currentPassword = value;
                this.OnPropertyChanged();
            }
        }
        public string NewUserID
        {
            get => this.newUserId;
            set
            {
                this.newUserId = value;
                this.OnPropertyChanged();
            }
        }
        public string NewUsername
        {
            get => this.newUsername;
            set
            {
                this.newUsername = value;
                this.OnPropertyChanged();
            }
        }
        public string NewPassword
        {
            get => this.newPassword;
            set
            {
                this.newPassword = value;
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
        /// Initializes a new instance of the <see cref="UserUpdateViewModel"/> class.
        /// </summary>
        /// <param name="user">The current user.</param>
        /// <precondition>User != null && user.ID > 0</precondition>
        /// <postcondition>this.CurrentUserID == user.ID && this.CurrentUsername == user.Username && this.CurrentPassword == user.Password</postcondition>
        public UserUpdateViewModel(User user)
        {
            if (user == null) {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.ID <= 0) {
                throw new ArgumentException("User ID cannot be less than 1.");
            }

            this.id = user.ID;
            this.CurrentUserID = user.UserID;
            this.CurrentUsername = user.Username;
            this.CurrentPassword = user.Password;
        }

        /// <summary>
        /// Updates the user's information.
        /// </summary>
        /// <precondition>true</precondition>
        /// <postcondition>true</postcondition>
        /// <returns>True if successful; false otherwise.</returns>
        public bool Update()
        {
            try
            {
                this.db.UpdateUser(this.id, this.NewUserID, this.NewUsername, this.NewPassword);
                return true;
            }
            catch (Exception ex)
            {
                this.ErrMsg = ex.Message;
                this.ErrMsg = this.id + this.NewUserID + this.NewUsername + this.NewPassword;
                return false;
            }
        }
    }
}
