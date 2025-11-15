namespace MuhasebeWepApp.Models;
using System.Linq;

public class BankSelectionViewModel
{
    public IEnumerable<Bank> Banks { get; init; } = Enumerable.Empty<Bank>();
    public string? SelectedBankId { get; set; }
}