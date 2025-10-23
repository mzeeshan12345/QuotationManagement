using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quotation_Management
{
    #region interfaces
    public interface IMapiMessage
    {
        int reserved { get; set; }
        string subject { get; set; }
        string noteText { get; set; }
        string messageType { get; set; }
        string dateReceived { get; set; }
        string conversationID { get; set; }
        int flags { get; set; }
        IntPtr originator { get; set; }
        int recipientCount { get; set; }
        IntPtr recipients { get; set; }
        int fileCount { get; set; }
        IntPtr files { get; set; }
    }
    public interface IMapiRecipDesc
    {
        int reserved { get; set; }
        int recipientClass { get; set; }
        string name { get; set; }
        string address { get; set; }
        int eIDSize { get; set; }
        IntPtr entryID { get; set; }
    }
    public interface IMapiFileDesc
    {
        int reserved { get; set; }
        int flags { get; set; }
        int position { get; set; }
        string pathName { get; set; }
        string fileName { get; set; }
        IntPtr fileType { get; set; }
    }
    #endregion
    #region ANSI classes
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiMessageA : IMapiMessage
    {
        #region fields
        int m_reserved;
        string m_subject;
        string m_noteText;
        string m_messageType;
        string m_dateReceived;
        string m_conversationID;
        int m_flags;
        IntPtr m_originator;
        int m_recipientCount;
        IntPtr m_recipients;
        int m_fileCount;
        IntPtr m_files;
        #endregion
        public int reserved { get { return m_reserved; } set { m_reserved = value; } }
        public string subject { get { return m_subject; } set { m_subject = value; } }
        public string noteText { get { return m_noteText; } set { m_noteText = value; } }
        public string messageType { get { return m_messageType; } set { m_messageType = value; } }
        public string dateReceived { get { return m_dateReceived; } set { m_dateReceived = value; } }
        public string conversationID { get { return m_conversationID; } set { m_conversationID = value; } }
        public int flags { get { return m_flags; } set { m_flags = value; } }
        public IntPtr originator { get { return m_originator; } set { m_originator = value; } }
        public int recipientCount { get { return m_recipientCount; } set { m_recipientCount = value; } }
        public IntPtr recipients { get { return m_recipients; } set { m_recipients = value; } }
        public int fileCount { get { return m_fileCount; } set { m_fileCount = value; } }
        public IntPtr files { get { return m_files; } set { m_files = value; } }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiRecipDescA : IMapiRecipDesc
    {
        #region fields
        int m_reserved;
        int m_recipientClass;
        string m_name;
        string m_address;
        int m_eIDSize;
        IntPtr m_entryID;
        #endregion
        public int reserved { get { return m_reserved; } set { m_reserved = value; } }
        public int recipientClass { get { return m_recipientClass; } set { m_recipientClass = value; } }
        public string name { get { return m_name; } set { m_name = value; } }
        public string address { get { return m_address; } set { m_address = value; } }
        public int eIDSize { get { return m_eIDSize; } set { m_eIDSize = value; } }
        public IntPtr entryID { get { return m_entryID; } set { m_entryID = value; } }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiFileDescA : IMapiFileDesc
    {
        #region fields
        int m_reserved;
        int m_flags;
        int m_position;
        string m_pathName;
        string m_fileName;
        IntPtr m_fileType;
        #endregion
        public int reserved { get { return m_reserved; } set { m_reserved = value; } }
        public int flags { get { return m_flags; } set { m_flags = value; } }
        public int position { get { return m_position; } set { m_position = value; } }
        public string pathName { get { return m_pathName; } set { m_pathName = value; } }
        public string fileName { get { return m_fileName; } set { m_fileName = value; } }
        public IntPtr fileType { get { return m_fileType; } set { m_fileType = value; } }
    }
    #endregion
    #region Unicode classes
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class MapiMessageW : IMapiMessage
    {
        #region fields
        int m_reserved;
        string m_subject;
        string m_noteText;
        string m_messageType;
        string m_dateReceived;
        string m_conversationID;
        int m_flags;
        IntPtr m_originator;
        int m_recipientCount;
        IntPtr m_recipients;
        int m_fileCount;
        IntPtr m_files;
        #endregion
        public int reserved { get { return m_reserved; } set { m_reserved = value; } }
        public string subject { get { return m_subject; } set { m_subject = value; } }
        public string noteText { get { return m_noteText; } set { m_noteText = value; } }
        public string messageType { get { return m_messageType; } set { m_messageType = value; } }
        public string dateReceived { get { return m_dateReceived; } set { m_dateReceived = value; } }
        public string conversationID { get { return m_conversationID; } set { m_conversationID = value; } }
        public int flags { get { return m_flags; } set { m_flags = value; } }
        public IntPtr originator { get { return m_originator; } set { m_originator = value; } }
        public int recipientCount { get { return m_recipientCount; } set { m_recipientCount = value; } }
        public IntPtr recipients { get { return m_recipients; } set { m_recipients = value; } }
        public int fileCount { get { return m_fileCount; } set { m_fileCount = value; } }
        public IntPtr files { get { return m_files; } set { m_files = value; } }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class MapiRecipDescW : IMapiRecipDesc
    {
        #region fields
        int m_reserved;
        int m_recipientClass;
        string m_name;
        string m_address;
        int m_eIDSize;
        IntPtr m_entryID;
        #endregion
        public int reserved { get { return m_reserved; } set { m_reserved = value; } }
        public int recipientClass { get { return m_recipientClass; } set { m_recipientClass = value; } }
        public string name { get { return m_name; } set { m_name = value; } }
        public string address { get { return m_address; } set { m_address = value; } }
        public int eIDSize { get { return m_eIDSize; } set { m_eIDSize = value; } }
        public IntPtr entryID { get { return m_entryID; } set { m_entryID = value; } }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class MapiFileDescW : IMapiFileDesc
    {
        #region fields
        int m_reserved;
        int m_flags;
        int m_position;
        string m_pathName;
        string m_fileName;
        IntPtr m_fileType;
        #endregion
        public int reserved { get { return m_reserved; } set { m_reserved = value; } }
        public int flags { get { return m_flags; } set { m_flags = value; } }
        public int position { get { return m_position; } set { m_position = value; } }
        public string pathName { get { return m_pathName; } set { m_pathName = value; } }
        public string fileName { get { return m_fileName; } set { m_fileName = value; } }
        public IntPtr fileType { get { return m_fileType; } set { m_fileType = value; } }
    }
    #endregion

}
