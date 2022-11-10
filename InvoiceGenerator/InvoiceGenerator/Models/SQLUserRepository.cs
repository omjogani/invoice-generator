﻿using InvoiceGenerator.Models;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceGenerator.Models
{

    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        public SQLUserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Users Add(Users user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public Users Delete(int Id)
        {
            Users users = context.Users.Find(Id);
            if(users != null)
            {
                context.Users.Remove(users);
                context.SaveChanges();
            }
            return users;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return context.Users;
        }

        public Users GetUserFromId(int Id)
        {
            return context.Users.FirstOrDefault(m => m.Id == Id);
        }
        public Users GetUserEmailPassword(string email,string password)
        {
            return context.Users.FirstOrDefault(m => m.Email == email && m.Password == password);
        }
        public Users Update(Users user)
        {
            var updateduser = context.Users.Attach(user);
            updateduser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return user;
        }
    }
}
