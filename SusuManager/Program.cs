using System.Globalization;
using SusuManager.Services;

Console.WriteLine("**=== Susu Savings Group Manager ===**");
Console.WriteLine();

//amount to be contributed weekly
decimal weeklyAmount;

while (true)
{
    Console.WriteLine("Enter the fixed weekly contributionin GHS:");

    if (decimal.TryParse(
        Console.ReadLine(),
        NumberStyles.Number,
        CultureInfo.InvariantCulture,
        out weeklyAmount) && weeklyAmount>0
        && decimal.Round(weeklyAmount,2)== weeklyAmount
    ){
        break;
    }

    Console.WriteLine("Please enter a positive amount with no more than two decimal places.");
}

var group = new SusuGroup(weeklyAmount);
Console.WriteLine($"\nGroup created. Weekly contribution: GHS {group.WeeklyContribution:F2}");

bool running = true;

while (running)
{
    Console.WriteLine("\n--- Main Menu ---");
    Console.WriteLine("1. Add Member");
    Console.WriteLine("2. Record Contribution");
    Console.WriteLine("3. Close current Week");
    Console.WriteLine(" 4. View Report");
    Console.WriteLine("5. Exit");
    Console.WriteLine("Choose an option (1-5):");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Console.WriteLine("Enter member's name:");
                string? name = Console.ReadLine();

                group.AddMember(name ?? string.Empty);
                Console.WriteLine($"Member '{name}' added successfully.");
                break;
            case "2":
                Console.WriteLine("Enter member's name or ID:");
                string? memberName=Console.ReadLine();

                Console.WriteLine("Enter payment amount in GHS:");
                if (!decimal.TryParse(
                        Console.ReadLine(),
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out decimal amount)
                )
                {
                    Console.WriteLine("Invalid payment amount.");
                    break;
                }
                group.RecordContribution(memberName ?? string.Empty, amount);
                Console.WriteLine($"Payment recorde : GHS{amount:F2}");
                break;

            case "3":
                if (group.CloseWeek())
                {
                    var payout = group.PayoutHistory[^1];

                    Console.WriteLine($"Week {payout.WeekNumber} closed successfully");

                    Console.WriteLine($"{payout.RecipientName} recieved GHS {payout.Amount:F2}.");

                    if (group.IsComplete)
                    {
                        Console.WriteLine("The Susu cycle is complete!");
                    }else
                    {
                        Console.WriteLine($"Week {group.CurrentWeek} has now started. ");

                    }
                }
                else
                {
                    Console.WriteLine($"Week {group.CurrentWeek} cannot close. Outstanding balances :");

                    foreach (var entry in group.GetOutstandingBalances())
                    {
                        Console.WriteLine($"- {entry.Key}: GHS {entry.Value:F2}");
                    }
                }
                break;

            case "4":
                Console.WriteLine($"\nCurrent week: {group.CurrentWeek}");
                Console.WriteLine(
                    $"Weekly contribution: GHS {group.WeeklyContribution:F2}");

                Console.WriteLine("\nMember payment status:");

                foreach (var member in group.Members)
                {
                    decimal owed = group.GetOutstandingBalances()
                        .GetValueOrDefault(member.Name);

                    if (owed == 0)
                    {
                        Console.WriteLine(
                            $"- {member.Name}: Paid in full");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"- {member.Name}: Owes GHS {owed:F2}");
                    }
                }

                Console.WriteLine("\nPayout history:");

                if (group.PayoutHistory.Count == 0)
                {
                    Console.WriteLine("No payouts recorded yet.");
                }
                else
                {
                    foreach (var payout in group.PayoutHistory)
                    {
                        Console.WriteLine(
                            $"- Week {payout.WeekNumber}: " +
                            $"{payout.RecipientName} received GHS {payout.Amount:F2}");
                    }
                }

                if (group.IsComplete)
                {
                    Console.WriteLine("\nThe susu cycle is complete.");
                }

                break;
            case "5":
                running = false;
                Console.WriteLine("Goodbye!");
                break;

            default:
                Console.WriteLine("Invalid option. Choose 1 to 5.");
                break;
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Invalid input: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Operation not allowed: {ex.Message}");
    }

}
    

