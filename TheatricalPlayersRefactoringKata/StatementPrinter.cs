using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}{Environment.NewLine}";
            var cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];

                var amount = calculatePlayAmount(play.Type, perf.Audience);
                
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(amount / 100), perf.Audience);
                totalAmount += amount;
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }
        
        private int calculatePlayAmount(string playType, int perfAudience)
        {
            switch (playType) 
            {
                case "tragedy":
                    return calculateTragedyAmount(perfAudience);
                case "comedy":
                    return calculateComedyAmount(perfAudience);
                default:
                    throw new Exception("unknown type: " + playType);
            }
        }

        private int calculateTragedyAmount(int perfAudience)
        {
            int amount = 40000;
            if (perfAudience > 30) {
                amount += 1000 * (perfAudience - 30);
            }

            return amount;
        }

        private int calculateComedyAmount(int perfAudience)
        {
            int amount = 30000;
            if (perfAudience > 20) {
                amount += 10000 + 500 * (perfAudience - 20);
            }
            return amount + 300 * perfAudience;
        }
    }

}
