using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticDemo
{
    public abstract class ISupportedQuery
    {
        public string getName()
        {
            return this.GetType().Name;
        }
        public abstract string getQueryTerm();
    }

    public class SocialSecurityNumber : ISupportedQuery
    {
        public override string getQueryTerm()
        {
            return @"\b(?!000|666)[0-8][0-9]{2}-(?!00)[0-9]{2}-(?!0000)[0-9]{4}\b";
        }
    }

    public class CreditCardNumber : ISupportedQuery
    {
        public override string getQueryTerm()
        {
            return @"^(?:(4[0-9]{12}(?:[0-9]{3})?)|(5[1-5][0-9]{14})|(6(?:011|5[0-9]{2})[0-9]{12})|(3[47][0-9]{13})|(3(?:0[0-5]|[68][0-9])[0-9]{11})|((?:2131|1800|35[0-9]{3})[0-9]{11}))$";
        }
    }
}
