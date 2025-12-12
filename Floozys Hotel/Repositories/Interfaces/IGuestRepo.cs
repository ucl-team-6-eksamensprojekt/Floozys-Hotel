using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Floozys_Hotel.Models;

namespace Floozys_Hotel.Repositories.Interfaces
{
    public interface IGuestRepo
    {
        /* CRUD operations for guest to separate guest management logic from business logic.
         * This contract ensures a consistent interface for handling guest data,
         * supporting maintainability, testability, and clear separation of concerns in the application.
         */
        int AddGuest(Guest guest);
        Guest GetByID (int id);
        List<Guest> GetAll();
        List<Guest> GetAllByName(string name);
        void UpdateGuest(Guest guest);
        void DeleteGuest(int id);         
    }
}
