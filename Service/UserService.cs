using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PennyPal.Model;

public class UserService
{
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
    private static readonly string FilePath = Path.Combine(FolderPath, "users.json");

    public List<User> LoadUsers()
    {
        if (!File.Exists(FilePath))
        {
            return new List<User>();
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        catch (JsonException)
        {
            return new List<User>();
        }
        catch (Exception)
        {
            return new List<User>();
        }
    }

    public void SaveUsers(List<User> users)
    {
        EnsureFolderExists();
        var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void AddUser(User newUser)
    {
        var users = LoadUsers();
        newUser.Userid = users.Any() ? users.Max(u => u.Userid) + 1 : 1;

        // Set default currency if not provided
        if (string.IsNullOrEmpty(newUser.PreferredCurrency))
        {
            newUser.PreferredCurrency = "USD";
        }

        users.Add(newUser);
        SaveUsers(users);
    }

    public User GetUserById(int id)
    {
        var users = LoadUsers();
        return users.FirstOrDefault(u => u.Userid == id);
    }

    public void UpdateUser(User updatedUser)
    {
        var users = LoadUsers();
        var existingUser = users.FirstOrDefault(u => u.Userid == updatedUser.Userid);
        if (existingUser != null)
        {
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.Transactions = updatedUser.Transactions;
            existingUser.Debts = updatedUser.Debts;

            // Update PreferredCurrency
            existingUser.PreferredCurrency = updatedUser.PreferredCurrency;
        }

        SaveUsers(users);
    }

    public void DeleteUser(int userId)
    {
        var users = LoadUsers();
        users.RemoveAll(u => u.Userid == userId);
        SaveUsers(users);
    }

    private void EnsureFolderExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool ValidatePassword(string inputPassword, string storedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedPassword;
    }

    public string ExportUsers()
    {
        try
        {
            // Define the path for export file on the desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string exportFilePath = Path.Combine(desktopPath, "ExportedUsers.json");

            // Load the users and serialize to a JSON file
            var users = LoadUsers();
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(exportFilePath, json);

            return exportFilePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error exporting users: {ex.Message}", ex);
        }
    }
    public string ExportTransactionsToExcel(List<Transactions> transactions)
    {
        try
        {
            // Define the desktop path and filename
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Transactions.xlsx");

            // Create a new Excel package
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required for EPPlus
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transactions");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Date";
                worksheet.Cells[1, 3].Value = "Debit";
                worksheet.Cells[1, 4].Value = "Credit";
                worksheet.Cells[1, 5].Value = "Description";
                worksheet.Cells[1, 6].Value = "Tags";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                }

                // Populate data
                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    worksheet.Cells[i + 2, 1].Value = transaction.Id;
                    worksheet.Cells[i + 2, 2].Value = transaction.Date.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 3].Value = transaction.Debit;
                    worksheet.Cells[i + 2, 4].Value = transaction.Credit;
                    worksheet.Cells[i + 2, 5].Value = transaction.Description;
                    worksheet.Cells[i + 2, 6].Value = string.Join(", ", transaction.Tags.Select(t => t.tagName));
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Save to file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }

            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error exporting transactions: {ex.Message}");
        }
    }
}
