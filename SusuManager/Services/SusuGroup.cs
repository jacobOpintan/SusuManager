using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SusuManager.Models;

namespace SusuManager.Services
{
    public class SusuGroup
    {
        private readonly List<Member> _members = new();
        private readonly List<Payout> _payoutHistory= new();
        private readonly Dictionary<string, decimal> _contributions= new(StringComparer.OrdinalIgnoreCase);

        private bool _hasStarted ;


        public decimal WeeklyContribution{get;}
        public int CurrentWeek {get; private set;} = 1;
        public bool IsComplete {get; private set;}


        public IReadOnlyList<Member> Members => _members.AsReadOnly();
        public IReadOnlyList<Payout> PayoutHistory => _payoutHistory.AsReadOnly();

        public SusuGroup(decimal weeklyContribution){
            if (weeklyContribution <=0 || decimal.Round(weeklyContribution,2)!= weeklyContribution)
            {
                throw new ArgumentException("The weekly contribution must be a positve and have no more than two decimal places.", nameof(weeklyContribution));
            }
            WeeklyContribution = weeklyContribution;
        }

        // Aadd a member to the susu group
        public void AddMember(string name)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("The susu group is complete. No more members can be added.");
            }

            if (_hasStarted)
            {
                throw new InvalidOperationException("Members cannot be added after the susu contributions has started.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Member name cannot be empty.",nameof(name));
            }
            name =name.Trim();

            //checking if the member already exists
            if (_members.Any(m=> string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("This member already exists.",nameof(name));
            
            }
            
            _members.Add(new Member(name,_members.Count + 1));

        }

        //record a member's contribution for the current week
        public void RecordContribution(string memberName, decimal amount)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("The susu group is complete.");
            }
            if (_members.Count == 0)
            {
                throw new InvalidOperationException("Add at least one member before recording contributions");

            }
            if (string.IsNullOrWhiteSpace(memberName))
            {
                throw new ArgumentException("Member name cannot be empty.", nameof(memberName));
            }

            if (amount<=0 || decimal.Round(amount, 2) != amount)
            {
                throw new ArgumentException("The payment must be positive and have no more than two decimal places",nameof(amount));
            }

            //retrieve the member from the list
            var member= _members.FirstOrDefault(m=> string.Equals(m.Name,memberName.Trim(),StringComparison.OrdinalIgnoreCase));

            if(member is null)
            {
                throw new ArgumentException("Member not found.", nameof(memberName));

            }

            decimal alreadyPaid = _contributions.GetValueOrDefault(member.Name);
            decimal amountOwed= WeeklyContribution - alreadyPaid;

            if (amount > amountOwed)
            {
                throw new ArgumentException($"Payment exceeds the outstanding balance of {amountOwed:F2} GHS.",nameof(amount));
            }

            _contributions[member.Name]= alreadyPaid +amount;
            _hasStarted = true;
        }

        //get outsanding balance for a member
        public Dictionary<string,decimal> GetOutstandingBalances()
        {
            return _members.Select(
                m=> new
                {
                    m.Name,
                    Balance= WeeklyContribution -_contributions.GetValueOrDefault(m.Name)
                }
            ).Where(x => x.Balance>0)
            .ToDictionary(x =>x.Name, x=> x.Balance);
        }

        public List<string> GetPaidInFullMembers()
        {
            return _members.Where(m =>_contributions.GetValueOrDefault(m.Name)== WeeklyContribution)
            .Select(m=>m.Name).ToList();
        }

        public bool CloseWeek()
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("The susu group is already complete");
            }
            
            if (_members.Count == 0)
            {
                throw new InvalidOperationException ("Add members before closing a week");
            }

            if (GetOutstandingBalances().Count > 0)
            {
                return false;
            }

            int recipientIndex =_payoutHistory.Count;
            Member recipient =_members[recipientIndex];

            decimal pot =WeeklyContribution *_members.Count;

            _payoutHistory.Add(new Payout(recipient.Name,CurrentWeek,pot));
            _contributions.Clear();

            if(_payoutHistory.Count == _members.Count)
            {
                IsComplete =true;
            }else
            {
                CurrentWeek++;
            }

            return true;
        }

    }
}