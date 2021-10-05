using System;
using System.Collections.Generic;
using System.Linq;
using FOCA.ModifiedComponents;
using MetadataExtractCore.Utilities;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Users {
        public int Id { get; set; }

        public virtual ThreadSafeList<UserItem> Items { get; set; }

        public Users()
        {
            Items = new ThreadSafeList<UserItem>();
        }

        public UserItem AddUniqueItem(string userValue, bool isComputerUser)
        {
            if (!IsValidUser(userValue)) return null;

            var userItem = new UserItem
            {
                Name         = userValue.Trim(),
                IsComputerUser = isComputerUser
            };

            if (!Items.Contains(userItem, new CaseInsensitiveUserItemComparer()))
                Items.Add(userItem);

            return userItem;
        }

        public void AddUniqueItem(string userValue, bool isComputerUser, string comment)
        {
            var userItem = AddUniqueItem(userValue, isComputerUser);
            if (userItem != null)
                userItem.Notes = comment;
        }

        public static bool IsValidUser(string userValue)
        {
            if (string.IsNullOrEmpty(userValue) || userValue.Trim() == string.Empty) return false;

            userValue = userValue.Trim();

            return userValue.Length >= 2 &&
                   Functions.StringContainAnyLetter(userValue);
        }
    }

    [Serializable]
    public class UserItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsComputerUser { get; set; }

        public string Notes { get; set; }
    }

    public class CaseInsensitiveUserItemComparer : EqualityComparer<UserItem>
    {
        public override bool Equals(UserItem x, UserItem y)
        {
            return (new WithoutAcentsStringEqualityComparer()).Equals(x.Name, y.Name);
        }

        public override int GetHashCode(UserItem obj)
        {
            return obj.GetHashCode();
        }
    }
}