using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Passwords
    {
        public int Id { get; set; }
        public List<PasswordsItem> Items { get; set; }

        public Passwords()
        {
            Items = new List<PasswordsItem>();
        }

        public void AddUniqueItem(PasswordsItem password)
        {
            if (Items.Count(S => S.Password == password.Password && S.Type == password.Type) == 0)
                Items.Add(password);
        }
    }

    [Serializable]
    public class PasswordsItem
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public PasswordsItem() { }

        public PasswordsItem(string password, string type, string source)
        {
            Type = type;
            Password = password;
            Source = source;
        }
    }
}