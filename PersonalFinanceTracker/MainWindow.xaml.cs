using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PersonalFinanceTracker.Data;

namespace PersonalFinanceTracker
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper? db;
        private const string LogFile = "logs.txt";
        private int? editingTransactionId = null;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                // Initialize DatePicker and ComboBox
                DateInput.SelectedDate = DateTime.Now;
                CategoryInput.SelectedIndex = 0;
                
                db = new DatabaseHelper();
                LoadData();
                AddLog("✅ Application started successfully");
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
                    AddLog("❌ Error: Database not initialized");
                    return;
                }

                // Validate inputs
                if (!ValidateInputs())
                    return;

                var date = DateInput.SelectedDate ?? DateTime.Now;
                var category = (CategoryInput.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Other";
                var amount = decimal.Parse(AmountInput.Text.Trim());
                var notes = NotesInput.Text.Trim();

                if (editingTransactionId.HasValue)
                {
                    // Update existing transaction
                    db.UpdateTransaction(editingTransactionId.Value, date, category, amount, notes);
                    AddLog($"✏️ Updated [{editingTransactionId.Value}] {date:yyyy-MM-dd} | {category} | ₹{amount:F2} | {notes}");
                    editingTransactionId = null;
                }
                else
                {
                    // Add new transaction
                    db.AddTransaction(date, category, amount, notes);
                    AddLog($"✅ Added | {date:yyyy-MM-dd} | {category} | ₹{amount:F2} | {notes}");
                }
                
                // Clear inputs on success
                DateInput.SelectedDate = DateTime.Now;
                CategoryInput.SelectedIndex = 0;
                AmountInput.Text = string.Empty;
                NotesInput.Text = string.Empty;
                AddButton.Content = "➕ Add Transaction";

                LoadData();
            }
            catch (Exception ex)
            {
                LogError(ex);
                AddLog($"❌ Error: {ex.Message}");
            }
        }

        private void EditTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.DataContext is Transaction transaction)
                {
                    // Populate form with transaction data
                    DateInput.SelectedDate = transaction.Date;
                    CategoryInput.SelectedItem = CategoryInput.Items.Cast<ComboBoxItem>()
                        .FirstOrDefault(item => item.Content.ToString() == transaction.Category) ?? CategoryInput.Items[0];
                    AmountInput.Text = transaction.Amount.ToString();
                    NotesInput.Text = transaction.Notes;
                    
                    editingTransactionId = transaction.Id;
                    AddButton.Content = "💾 Update Transaction";
                    AddLog($"📝 Editing transaction [{transaction.Id}] - Make changes and click Update");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                AddLog($"❌ Error editing transaction: {ex.Message}");
            }
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.DataContext is Transaction transaction && db != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"Are you sure you want to delete this transaction?\n\nDate: {transaction.Date:yyyy-MM-dd}\nCategory: {transaction.Category}\nAmount: ₹{transaction.Amount:F2}",
                        "Delete Transaction",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.DeleteTransaction(transaction.Id);
                        AddLog($"🗑️ Deleted [{transaction.Id}] {transaction.Date:yyyy-MM-dd} | {transaction.Category} | ₹{transaction.Amount:F2}");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                AddLog($"❌ Error deleting transaction: {ex.Message}");
            }
        }

        private bool ValidateInputs()
        {
            // Validate date
            if (DateInput.SelectedDate == null)
            {
                AddLog("⚠️ Validation: Please select a valid date");
                DateInput.Focus();
                return false;
            }

            // Validate category
            if (CategoryInput.SelectedItem == null)
            {
                AddLog("⚠️ Validation: Please select a category");
                CategoryInput.Focus();
                return false;
            }

            // Validate amount
            var amountText = AmountInput.Text.Trim();
            if (string.IsNullOrEmpty(amountText))
            {
                AddLog("⚠️ Validation: Please enter an amount");
                AmountInput.Focus();
                return false;
            }

            if (!decimal.TryParse(amountText, out decimal amount))
            {
                AddLog("⚠️ Validation: Please enter a valid number for amount");
                AmountInput.Focus();
                return false;
            }

            if (amount <= 0)
            {
                AddLog("⚠️ Validation: Amount must be greater than 0");
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
                IncomeLabel.Text = $"Income: ₹{summary.TotalIncome:F2}";
                ExpenseLabel.Text = $"Expenses: ₹{summary.TotalExpenses:F2}";
                BalanceLabel.Text = $"Balance: ₹{summary.Balance:F2}";
            }
            catch (Exception ex)
            {
                LogError(ex);
                AddLog($"❌ Error loading transactions: {ex.Message}");
            }
        }

        private void AddLog(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}\n";
                LogTextBox.AppendText(logEntry);
                LogTextBox.ScrollToEnd();
            }
            catch
            {
                // Silently fail if logging fails
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