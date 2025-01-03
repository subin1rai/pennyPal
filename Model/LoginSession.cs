using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PennyPal.Model;


public class LoginSession
{
private User _loggedInUser;

public bool IsLoggedIn => _loggedInUser != null;
    public User LoggedInUser
    {
        get => _loggedInUser ?? throw new InvalidOperationException("No user is logged in.");
        set => _loggedInUser = value;
    }

    public void Logout()
    {
        _loggedInUser = null;
    }


}