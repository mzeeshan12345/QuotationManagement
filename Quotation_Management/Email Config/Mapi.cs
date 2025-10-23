using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Quotation_Management {
    public class MAPI
    {
        #region static
        const int MAPI_LOGON_UI = 0x00000001;
        const int MAPI_DIALOG = 0x8;
        const int MAPI_DIALOG_MODELESS = 0x4 | MAPI_DIALOG;
        const int MAPI_USER_ABORT = 1;
        const int MAPI_E_LOGON_FAILURE = 3;
        [DllImport("MAPI32.DLL", EntryPoint = "MAPISendMail", ExactSpelling = true, CharSet = CharSet.Ansi)]
        static extern int MAPISendMailA(IntPtr session, IntPtr uiParam, MapiMessageA message, int flags, int reserved);
        [DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
        static extern int MAPILogon(IntPtr hwnd, string profileName, string password, int flags, int reserved, ref IntPtr session);
        [DllImport("MAPI32.DLL", CharSet = CharSet.Ansi)]
        static extern int MAPILogoff(IntPtr session, IntPtr hwnd, int flags, int reserved);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern uint GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, uint cchBuffer);
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        static IntPtr OffsetPtr(IntPtr ptr, Type structureType, int offset)
        {
            return ptr + offset * Marshal.SizeOf(structureType);
        }
        public static void SendMail(IntPtr handle, string[] files, string mailSubject, string mailBody, RecipientCollection recipients)
        {
            MAPI mapi = UnicodeSupported ? new MAPIUnicode() : new MAPI();
            mapi.SendInternal(handle, files, mailSubject, mailBody, recipients);
        }
        #endregion
        string[] files;
        string subject = String.Empty;
        string body = String.Empty;
        IntPtr handle = IntPtr.Zero;
        IntPtr session = IntPtr.Zero;
        int error;
        RecipientCollection recipients = new RecipientCollection();
        public static bool UseLogon;
        static bool AllowModelessDialog { get { return false; } }
        static bool? unicodeSupported;
        static bool UnicodeSupported
        {
            get
            {
                if (!unicodeSupported.HasValue)
                {
                    IntPtr mapiLib = LoadLibrary("MAPI32.DLL");
                    if (mapiLib != IntPtr.Zero)
                    {
                        IntPtr addr = GetProcAddress(mapiLib, "MAPISendMailW");
                        unicodeSupported = addr != IntPtr.Zero;
                    }
                    else
                        unicodeSupported = false;
                }
                return unicodeSupported.Value;
            }
        }
        public MAPI(IntPtr handle, string[] files, string mailSubject, string mailBody, RecipientCollection recipients)
        {
            SendInternal(handle, files, mailSubject, mailBody, recipients);
        }
        internal MAPI()
        {
        }
        void SendInternal(IntPtr handle, string[] files, string mailSubject, string mailBody, RecipientCollection recipients)
        {
            this.files = files;
            this.subject = mailSubject;
            this.body = mailBody;
            this.recipients = recipients;
            if (UseLogon)
            {
                SendUsingLogon(handle);
                return;
            }
            IMapiMessage msg = CreateMessage();
            error = MAPISendMail(session, handle, msg, GetDialogMode(), 0);
            if (error != 0 && error != MAPI_USER_ABORT)
                MAPISendMail(session, handle, msg, GetDialogMode() | MAPI_LOGON_UI, 0);
            DisposeMessage(msg);
        }
        void SendUsingLogon(IntPtr handle)
        {
            if (Logon(handle))
            {
                IMapiMessage msg = CreateMessage();
                error = MAPISendMail(session, handle, msg, GetDialogMode(), 0);
                Logoff();
                DisposeMessage(msg);
            }
        }
        static int GetDialogMode()
        {
            return AllowModelessDialog ? MAPI_DIALOG_MODELESS : MAPI_DIALOG;
        }
        protected virtual int MAPISendMail(IntPtr session, IntPtr handle, IMapiMessage msg, int flags, int reserved)
        {
            return MAPISendMailA(session, handle, (MapiMessageA)msg, flags, reserved);
        }
        IMapiMessage CreateMessage()
        {
            IMapiMessage msg = CreateMapiMessageInstance();
            msg.subject = subject;
            msg.noteText = body;
            if (files.Length > 0)
            {
                msg.fileCount = files.Length;
                msg.files = GetFilesDesc();
            }
            if (recipients.Count > 0)
            {
                msg.recipientCount = recipients.Count;
                msg.recipients = GetRecipDesc();
            }
            return msg;
        }
        IntPtr GetFilesDesc()
        {
            IntPtr ptra = AllocMemory(GetMapiFileDescType(), files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                IMapiFileDesc fileDesc = CreateMapiFileDescInstance();
                fileDesc.position = -1;
                fileDesc.fileName = Path.GetFileName(files[i]);
                fileDesc.pathName = GetShortPathName(files[i]);
                Marshal.StructureToPtr(fileDesc, OffsetPtr(ptra, GetMapiFileDescType(), i), false);
            }
            return ptra;
        }
        string GetShortPathName(string path)
        {
            StringBuilder sb = new StringBuilder();
            uint count = GetShortPathName(path, sb, 0);
            if (count == 0)
                return "";
            sb.Capacity = (int)count;
            GetShortPathName(path, sb, count);
            return sb.ToString();
        }
        IntPtr GetRecipDesc()
        {
            IntPtr ptra = AllocMemory(GetMapiRecipDescType(), recipients.Count);
            for (int i = 0; i < recipients.Count; i++)
            {
                Recipient item = recipients[i];
                IMapiRecipDesc recipDesc = CreateMapiRecipDescInstance();
                recipDesc.reserved = 0;
                recipDesc.recipientClass = (int)item.FieldType;
                recipDesc.name = item.ContactName;
                recipDesc.address = item.Address;
                recipDesc.eIDSize = 0;
                recipDesc.entryID = IntPtr.Zero;
                Marshal.StructureToPtr(recipDesc, OffsetPtr(ptra, GetMapiRecipDescType(), i), false);
            }
            return ptra;
        }
        IntPtr AllocMemory(Type structureType, int count)
        {
            return Marshal.AllocHGlobal(Marshal.SizeOf(structureType) * count);
        }
        void Logoff()
        {
            if (session != IntPtr.Zero)
            {
                error = MAPILogoff(session, handle, 0, 0);
                session = IntPtr.Zero;
            }
        }
        bool Logon(IntPtr hwnd)
        {
            this.handle = hwnd;
            error = MAPILogon(hwnd, null, null, 0, 0, ref session);
            if (error != 0)
                error = MAPILogon(hwnd, null, null, MAPI_LOGON_UI, 0, ref session);
            return error == 0;
        }
        void DisposeMessage(IMapiMessage msg)
        {
            FreeMemory(msg.files, GetMapiFileDescType(), files.Length);
            FreeMemory(msg.recipients, GetMapiRecipDescType(), recipients.Count);
            msg = null;
        }
        void FreeMemory(IntPtr ptr, Type structureType, int count)
        {
            if (ptr != IntPtr.Zero)
            {
                for (int i = 0; i < count; i++)
                {
                    Marshal.DestroyStructure(OffsetPtr(ptr, structureType, i), structureType);
                }
                Marshal.FreeHGlobal(ptr);
            }
        }
        protected virtual IMapiMessage CreateMapiMessageInstance()
        {
            return new MapiMessageA();
        }
        protected virtual IMapiFileDesc CreateMapiFileDescInstance()
        {
            return new MapiFileDescA();
        }
        protected virtual IMapiRecipDesc CreateMapiRecipDescInstance()
        {
            return new MapiRecipDescA();
        }
        protected virtual Type GetMapiFileDescType()
        {
            return typeof(MapiFileDescA);
        }
        protected virtual Type GetMapiRecipDescType()
        {
            return typeof(MapiRecipDescA);
        }
    }
    [System.Security.SecuritySafeCritical]
    class MAPIUnicode : MAPI
    {
        [DllImport("MAPI32.DLL", EntryPoint = "MAPISendMailW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        static extern int MAPISendMailW(IntPtr session, IntPtr uiParam, MapiMessageW message, int flags, int reserved);
        protected override int MAPISendMail(IntPtr session, IntPtr handle, IMapiMessage msg, int flags, int reserved)
        {
            return MAPISendMailW(session, handle, (MapiMessageW)msg, flags, reserved);
        }
        protected override IMapiMessage CreateMapiMessageInstance()
        {
            return new MapiMessageW();
        }
        protected override IMapiFileDesc CreateMapiFileDescInstance()
        {
            return new MapiFileDescW();
        }
        protected override IMapiRecipDesc CreateMapiRecipDescInstance()
        {
            return new MapiRecipDescW();
        }
        protected override Type GetMapiFileDescType()
        {
            return typeof(MapiFileDescW);
        }
        protected override Type GetMapiRecipDescType()
        {
            return typeof(MapiRecipDescW);
        }
    }
}