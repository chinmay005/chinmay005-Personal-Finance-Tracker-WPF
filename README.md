# Personal Finance Tracker

A simple yet powerful WPF desktop application to track personal expenses and income using SQLite. Built with modern C# and .NET technologies.

## Features

✨ **Core Functionality**
- ✅ Add income and expense transactions with ease
- ✅ View all transactions in a searchable, sortable data grid
- ✅ Real-time summary dashboard (Total Income, Total Expenses, Balance)
- ✅ Automatic categorization of transactions
- ✅ Notes field for transaction details

🛡️ **Reliability**
- ✅ Input validation for all fields
- ✅ Comprehensive error handling
- ✅ Error logging to `logs.txt` for debugging
- ✅ SQLite database persistence

💅 **User Interface**
- ✅ Clean, modern WPF design
- ✅ Alternating row colors in data grid for readability
- ✅ Currency formatting for all monetary values
- ✅ Color-coded summary (Income: Green, Expenses: Red, Balance: Blue)
- ✅ Responsive form with quick-add transaction capability

## Tech Stack

- **Language**: C# 12
- **Framework**: .NET 10.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Database**: SQLite with System.Data.SQLite
- **IDE**: Visual Studio Code / Visual Studio

## Project Structure

```
PersonalFinanceTracker/
├── Data/
│   └── DatabaseHelper.cs       # Database operations & models
├── MainWindow.xaml             # UI layout
├── MainWindow.xaml.cs          # Event handlers & business logic
├── App.xaml                    # Application configuration
├── App.xaml.cs                 # Application startup
└── PersonalFinanceTracker.csproj
```

## Installation & Setup

### Prerequisites
- .NET 10.0 SDK or higher
- Windows OS (for WPF)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/chinmay005/Personal-Finance-Tracker-WPF.git
   cd chinmay005-Personal-Finance-Tracker-WPF/PersonalFinanceTracker
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

## Usage

### Adding a Transaction

1. Select a **date** using the date picker
2. Choose a **category** from the dropdown (Income categories: Salary, Bonus, Investment, Gift, Other Income | Expense categories: Food, Rent, Transport, Entertainment, Utilities, Healthcare, Other)
3. Enter the **amount** (positive number only)
4. *(Optional)* Add **notes** for transaction details
5. Click **"Add Transaction"** button

### Viewing Transactions

- All transactions are displayed in the data grid below the form
- Transactions are sorted by date (newest first)
- The grid shows: ID, Date, Category, Amount, Notes

### Summary Dashboard

The bottom section displays:
- **Total Income**: Sum of all income transactions
- **Total Expenses**: Sum of all expense transactions
- **Balance**: Income minus Expenses

## Key Features Explained

### Input Validation
- Amount must be a positive decimal number
- Date must be selected
- Category must be chosen
- Shows validation messages if any field is invalid

### Error Handling
- All database operations are wrapped in try-catch blocks
- Errors are logged to `logs.txt` in the application directory
- User-friendly error messages are displayed
- Application continues to function even if an error occurs

### Data Persistence
- Uses SQLite database (`finance.db`)
- Automatically creates the database on first run
- Transactions are stored permanently

## Database Schema

### Transactions Table
```sql
CREATE TABLE Transactions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    Category TEXT NOT NULL,
    Amount REAL NOT NULL,
    Notes TEXT
)
```

## Screenshots

*(Coming soon - UI polishing in progress)*

## Future Enhancements

- [ ] Edit and delete transactions
- [ ] Category management and custom categories
- [ ] Charts and graphs for expense visualization
- [ ] Monthly/yearly summaries
- [ ] Budget planning and alerts
- [ ] Data export (CSV, PDF)
- [ ] Multi-user support
- [ ] Cloud synchronization

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

**Chinmay** - [GitHub Profile](https://github.com/chinmay005)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

If you encounter any issues, please:
1. Check the `logs.txt` file for error details
2. Open an issue on GitHub with a detailed description
3. Include steps to reproduce the problem

## Changelog

### Version 1.0.0 (Current)
- Initial release
- Core transaction management functionality
- SQLite database integration
- Input validation and error handling
- Modern UI with WPF

---

Made with ❤️ for personal finance management
