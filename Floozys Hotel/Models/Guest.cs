using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floozys_Hotel.Models
{
    public class Guest
    {
        private int _GuestID;
        private string _FirstName;
        private string _LastName;
        private string _Email;
        private string _Phone;
        private string _Country;
        private string _PassportNumber;

        public int GuestID 
        { 
            get { return _GuestID; } 
            set { _GuestID = value; } 
        }

        public string FirstName 
        { 
            get { return _FirstName; } 
            set { _FirstName = value; } 
        }

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        public string PassportNumber
        {
            get { return _PassportNumber; }
            set { _PassportNumber = value; }
        }
    }
}
