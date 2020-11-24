using System;
using System.Collections.Generic;
using System.Text;

namespace ChatClient
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public User(int id, string username)
        {
            Id = id;
            Username = username;
        }
        public User()
        {
                
        }
    }
}
