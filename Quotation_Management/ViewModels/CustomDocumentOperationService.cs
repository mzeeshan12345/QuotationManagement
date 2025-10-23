using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpress.XtraReports.Web.WebDocumentViewer.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class CustomDocumentOperationService : DocumentOperationService
    {
        public override bool CanPerformOperation(DocumentOperationRequest request)
        {
            return true;
        }

        public override DocumentOperationResponse PerformOperation(DocumentOperationRequest request, PrintingSystemBase initialPrintingSystem, PrintingSystemBase printingSystemWithEditingFields)
        {
            using (var stream = new MemoryStream())
            {
                printingSystemWithEditingFields.ExportToPdf(stream);
                stream.Position = 0;
                var mailAddress = new MailAddress(request.CustomData);
                var recipients = new MailAddressCollection() { mailAddress };
                var attachment = new Attachment(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                return SendEmail(recipients, "Subject is Here!", "PFA", attachment);
            }
        }

        DocumentOperationResponse SendEmail(MailAddressCollection recipients, string subject, string messageBody, Attachment attachment)
        {
            string SmtpHost = "in-v3.mailjet.com";
            int SmtpPort = 587;
            if (string.IsNullOrEmpty(SmtpHost) || SmtpPort == -1)
            {
                return new DocumentOperationResponse { Message = "Please configure the SMTP server settings." };
            }

            string SmtpUserName = "*****";
            string SmtpUserPassword = "123456789";
            string SmtpFrom = "donotreply@domain.com";
            string SmtpFromDisplayName = "Quotation";
            using (var smtpClient = new SmtpClient(SmtpHost, SmtpPort))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                if (!string.IsNullOrEmpty(SmtpUserName))
                {
                    smtpClient.Credentials = new NetworkCredential(SmtpUserName, SmtpUserPassword);
                }

                using (var message = new MailMessage())
                {
                    message.Subject = subject.Replace("\r", "").Replace("\n", "");
                    message.IsBodyHtml = true;
                    message.Body = messageBody;
                    message.From = new MailAddress(SmtpFrom, SmtpFromDisplayName);

                    foreach (var item in recipients)
                    {
                        message.To.Add("");
                    }

                    try
                    {
                        if (attachment != null)
                        {
                            message.Attachments.Add(attachment);
                        }
                        smtpClient.Send(message);
                        return new DocumentOperationResponse
                        {
                            Succeeded = true,
                            Message = "Mail was sent successfully"
                        };
                    }
                    catch (SmtpException e)
                    {
                        return new DocumentOperationResponse
                        {
                            Message = "Sending an email message failed."
                        };
                    }
                    finally
                    {
                        message.Attachments.Clear();
                    }
                }
            }
        }

        protected string RemoveNewLineSymbols(string value)
        {
            return value;
        }
    }
}