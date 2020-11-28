using AngularProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Services
{
    public interface IUcommentService
    {
        User Authenticate(string username, string password);
        Task<User> PostUser(User user);
    }
}
