using System;
using System.IO;
using System.Windows;
using PersonalFinanceTracker.Data;

namespace PersonalFinanceTracker
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper? db;
        private const string LogFile = "logs.txt";

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                db = new DatabaseHelper();
                LoadData();
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Error initializing application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (db == null)
                {
                    MessageBox.Show("Database not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate inputs
                if (!ValidateInputs())
                    return;

                var date = DateInput.SelectedDate ?? DateTime.Now;
                var category = (CategoryInput.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "Other";
                var amount = decimal.Parse(AmountInput.Text.Trim());
                var notes = NotesInput.Text.Trim();

                db.AddTransaction(date, category, amount, notes);
                
                // Clear inputs on success
                DateInput.SelectedDate = DateTime.Now;
                CategoryInput.SelectedIndex = 0;
                AmountInput.Text = string.Empty;
                NotesInput.Text = string.Empty;

                LoadData();
                MessageBox.Show("Transaction added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Error saving transaction. Please check your input and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            // Validate date
            if (DateInput.SelectedDate == null)
            {
                MessageBox.Show("Please select a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                DateInput.Focus();
                return false;
            }

            // Validate category
            if (CategoryInput.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CategoryInput.Focus();
                return false;
            }

            // Validate amount
            var amountText = AmountInput.Text.Trim();
            if (string.IsNullOrEmpty(amountText))
            {
                MessageBox.Show("Please enter an amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountInput.Focus();
                return false;
            }

            if (!decimal.TryParse(amountText, out decimal amount))
            {
                MessageBox.Show("Please enter a valid number for the amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountInput.Focus();
                return false;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount (must be greater than 0).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                AmountInput.Focus();
                return false;
            }

            return true;
        }

        private void LoadData()
        {
            try
            {
                if (db == null)
                    return;

                var transactions = db.GetTransactions();
                TransactionsGrid.ItemsSource = transactions;
                
                var summary = db.GetSummary();
                IncomeLabel.Text = $"Income: ${summary.TotalIncome:F2}";
                ExpenseLabel.Text = $"Expenses: ${summary.TotalExpenses:F2}";
                BalanceLabel.Text = $"Balance: ${summary.Balance:F2}";
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Error loading transactions.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogError(Exception ex)
        {
            try
            {
                var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}\n\n";
                File.AppendAllText(LogFile, logMessage);
            }
            catch
            {
                // Silently fail if logging fails
            }
        }
    }
}