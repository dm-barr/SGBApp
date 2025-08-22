public class Dashboard
{
    public int TotalBooks { get; set; }
    public int TotalStudents { get; set; }
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public decimal TotalFines { get; set; }


    public Dictionary<string, int> LoansByStatus { get; set; } = new();
    public List<(string BookTitle, int Count)> TopBooks { get; set; } = new();
    public List<(string StudentName, int Count)> TopStudents { get; set; } = new();
    public Dictionary<string, decimal> FinesByDay { get; set; } = new();
    public (int Available, int Borrowed) BooksDistribution { get; set; }
}
