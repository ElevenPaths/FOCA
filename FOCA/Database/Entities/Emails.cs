using System;
using System.Collections.Generic;
using System.Linq;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Emails
    {
        public int Id { get; set; }
        public List<EmailsItem> Items { get; set; }

        public Emails()
        {
            Items = new List<EmailsItem>();
        }

        public void AddUniqueItem(string emailValue)
        {
            if (string.IsNullOrEmpty(emailValue) || emailValue.Trim() == string.Empty) return;

            var emailItem = new EmailsItem();
            emailItem.Mail = emailValue.Trim();

            if (!Items.Contains(emailItem, new CaseInsensitiveEmailItemComparer<EmailsItem>()))
                Items.Add(emailItem);
        }
    }

    [Serializable]
    public class EmailsItem
    {
        public int Id { get; set; }
        public string Mail { get; set; }
    }

    public class CaseInsensitiveEmailItemComparer<T> : EqualityComparer<EmailsItem>
    {
        public override bool Equals(EmailsItem x, EmailsItem y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x.Mail.Trim(), y.Mail.Trim());
        }

        public override int GetHashCode(EmailsItem obj)
        {
            return obj.GetHashCode();
        }
    }
}