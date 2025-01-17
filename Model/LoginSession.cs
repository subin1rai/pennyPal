using System;
using PennyPal.Model;

namespace PennyPal.Model
{
    public class LoginSession
    {
        private User _loggedInUser;
        public bool IsLoggedIn => _loggedInUser != null;

        // Logged-in user property
        public User LoggedInUser
        {
            get => _loggedInUser ?? throw new InvalidOperationException("No user is logged in.");
            set => _loggedInUser = value;
        }
        public string SelectedCurrency { get; set; } = "USD"; 

        // Logout functionality
        public void Logout()
        {
            _loggedInUser = null;
            SelectedCurrency = "USD"; 
        }
    }
}
