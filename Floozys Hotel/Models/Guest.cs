using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floozys_Hotel.Models
{
    public class Guest
    {
        /* Auto-properties are used to simplify the code and improve readability
         * when no extra logic is needed in the getter or setter.
         * They provide encapsulation without explicit backing fields
         * and make it easy to add logic later if requirements change.
         */
        public int GuestID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string PassportNumber { get; set; }

        public Guest()
        {

        }

        public Guest(string firstName, string lastName, string email, string phoneNumber, string country, string passportNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Country = country;
            PassportNumber = passportNumber;
        }

        /* Validation for the data that is required in roombooking.
         * Validation is collected in this method instead of in each property-setter.
         * This approach is preferred in WPF/MVVM because:
         * - It allows the object to be constructed and updated freely (e.g. via data binding) without throwing exceptions.
         * - All validation errors can be collected and shown at once, instead of stopping at the first error.
         * - It separates domain logic (validation rules) from UI logic, making the model reusable and easier to maintain.
         */
        public List<string> Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(FirstName))
                errors.Add("First name is required.");
            if (string.IsNullOrWhiteSpace(LastName))
                errors.Add("Last name is required.");
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@") || !Email.Contains("."))
                errors.Add("A valid email is required.");
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                errors.Add("Phone number is required.");
            if (string.IsNullOrWhiteSpace(Country))
                errors.Add("Country is required.");
            return errors;
        }
    }
}
