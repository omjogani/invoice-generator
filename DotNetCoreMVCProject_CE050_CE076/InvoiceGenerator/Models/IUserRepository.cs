using System.Collections;
using System.Collections.Generic;

namespace InvoiceGenerator.Models
{
    public interface IUserRepository
    {
        
        Users GetUserFromId(int Id);
        Users GetUserEmailPassword(string Email,string password);
        IEnumerable<Users> GetAllUsers();
        Users Add(Users user);
        Users Update(Users user);
        Users Delete(int Id);

    }
}
