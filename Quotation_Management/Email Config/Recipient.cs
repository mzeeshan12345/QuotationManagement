using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotation_Management
{
    public enum RecipientFieldType {
		TO = 1,
		CC = 2,
		BCC = 3
	};


    public class Recipient
    {
        #region Fields & Properties
        const RecipientFieldType defaultFieldType = RecipientFieldType.TO;
        const string defaultAddressPrefix = "SMTP:";
        string address, contactName;
        string prefix = defaultAddressPrefix;
        RecipientFieldType type = defaultFieldType;
        public string ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
        public RecipientFieldType FieldType
        {
            get { return type; }
            set { type = value; }
        }
        #endregion
        #region Constructors
        public Recipient()
            : this("")
        {
        }
        public Recipient(string address)
            : this(address, "")
        {
        }
        public Recipient(string address, RecipientFieldType type)
            : this(address, "", defaultAddressPrefix, type)
        {
        }
        public Recipient(string address, string recipientName)
            : this(address, recipientName, defaultAddressPrefix)
        {
        }
        public Recipient(string address, string recipientName, RecipientFieldType type)
            : this(address, recipientName, defaultAddressPrefix, type)
        {
        }
        public Recipient(string address, string recipientName, string prefix)
            : this(address, recipientName, prefix, RecipientFieldType.TO)
        {
        }
        public Recipient(string address, string recipientName, string prefix, RecipientFieldType type)
        {
            this.address = address;
            this.contactName = recipientName;
            this.prefix = prefix;
            this.type = type;
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return EmptyNameAndAddress ? "Recipient" : string.Format("{0}{1}", EmptyName ? "" : contactName + " ", EmptyAddress ? "" : "<" + address + ">");
        }
        internal void CorrectData()
        {
            if (!String.IsNullOrEmpty(address) && !HasRecipientPrefix)
                address = prefix + address;
            if (String.IsNullOrEmpty(contactName))
                contactName = HasRecipientPrefix ? address.Substring(prefix.Length) : address;
        }
        internal bool EmptyNameAndAddress { get { return EmptyName && EmptyAddress; } }
        bool EmptyName { get { return string.IsNullOrEmpty(contactName); } }
        bool EmptyAddress { get { return string.IsNullOrEmpty(address); } }
        bool HasRecipientPrefix
        {
            get { return address.IndexOf(prefix) == 0; }
        }
        #endregion
    }
    [ListBindable(BindableSupport.No)]
    public class RecipientCollection : Collection<Recipient>
    {
        public RecipientCollection()
        {
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Recipient this[string recipientAddress]
        {
            get { return GetByAddress(recipientAddress); }
        }
        internal Recipient GetByAddress(string recipientAddress)
        {
            return Items.FirstOrDefault(recipient => recipient.Address == recipientAddress);
        }
        public void AddRange(Recipient[] items)
        {
            foreach (Recipient item in items)
                if (!item.EmptyNameAndAddress)
                    this.Add(item);
        }
        internal void CopyFrom(RecipientCollection source)
        {
            this.Clear();
            this.AddRange(source.ToArray());
        }
        protected Recipient[] ToArray()
        {
            Recipient[] items = new Recipient[this.Count];
            this.CopyTo(items, 0);
            return items;
        }
    }

}
