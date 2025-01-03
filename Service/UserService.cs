using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using PennyPal.Model;

public class UserService
{
    // Paths for storing application data
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
    private static readonly string FilePath = Path.Combine(FolderPath, "users.json");

    // Load all users from the JSON file
    public List<User> LoadUsers()
    {
        if (!File.Exists(FilePath))
        {
            // Return an empty list if the file does not exist
            return new List<User>();
        }

        try
        {
            // Read JSON content from the file
            var json = File.ReadAllText(FilePath);
            // Deserialize JSON into a list of users
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch (JsonException)
        {
            // Handle corrupted JSON files gracefully
            return new List<User>();
        }
        catch (Exception)
        {
            // Handle other potential errors (e.g., file access issues)
            return new List<User>();
        }
    }

    // Save the user list to the JSON file
    public void SaveUsers(List<User> users)
    {
        EnsureFolderExists();

        // Serialize the user list into JSON
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        // Write JSON content to the file
        File.WriteAllText(FilePath, json);
    }

    // Add a new user
    public void AddUser(User newUser)
    {
        var users = LoadUsers();

        // Generate a unique ID for the new user
        newUser.Userid = users.Any() ? users.Max(u => u.Userid) + 1 : 1;

        // Add the new user to the list
        users.Add(newUser);

        // Save the updated list
        SaveUsers(users);
    }

    // Find a user by ID
    public User GetUserById(int id)
    {
        var users = LoadUsers();
        return users.FirstOrDefault(u => u.Userid == id);
    }

    // Update an existing user
    public void UpdateUser(User updatedUser)
    {
        var users = LoadUsers();

        // Find the existing user
        var existingUser = users.FirstOrDefault(u => u.Userid == updatedUser.Userid);
        if (existingUser != null)
        {
            // Update user properties
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.Address = updatedUser.Address;
            existingUser.Transactions = updatedUser.Transactions;
        }

        // Save the updated list
        SaveUsers(users);
    }

    // Delete a user
    public void DeleteUser(int userId)
    {
        var users = LoadUsers();

        // Remove the user with the given ID
        users.RemoveAll(u => u.Userid == userId);

        // Save the updated list
        SaveUsers(users);
    }

    // Utility: Ensure the data folder exists
    private void EnsureFolderExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
    }

    // Hash a password securely
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    // Validate a password against a stored hash
    public bool ValidatePassword(string inputPassword, string storedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedPassword;
    }
}