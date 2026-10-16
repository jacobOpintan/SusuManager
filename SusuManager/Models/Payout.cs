using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SusuManager.Models
{
    public class Payout
    {
        public string RecipientName { get; }
        public int WeekNumber { get; }
        public decimal Amount { get; }

        public Payout(string recipientName, int weekNumber, decimal amount)
        {
            RecipientName = recipientName;
            WeekNumber = weekNumber;
            Amount = amount;
        }
    }
}