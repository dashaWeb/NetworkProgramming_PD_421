using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using MimeKit.Utils;
using System.IO;
using System.Text;


internal class Program
{
    const string username = "dashakonopelko@gmail.com";
    const string password = "xghi oumb cuze ddcw";
    const string otheruser = "velax12199@idoidraw.com";

    private static void Main(string[] args)
    {
        /////////// Send Mails (SMTP)
        /* MimeMessage message = new();
         message.From.Add(new MailboxAddress("Dasha", username));
         message.To.Add(new MailboxAddress("User", otheruser));

         message.Subject = "Добрий вечір, ми з України. How you doin";
         message.Importance = MessageImportance.High;

         *//*message.Body = new TextPart("plain")
         {
             Text = @"Hey Alice,

             What are you up to this weekend? Monica is throwing one of her parties on
             Saturday and I was hoping you could make it.

             Will you be my +1?

             -- Joey
             "
         };*/
        /*var body = new TextPart("plain")
        {
            Text = @"Hey Alice,

            What are you up to this weekend? Monica is throwing one of her parties on
            Saturday and I was hoping you could make it.

            Will you be my +1?

            -- Joey
            "
        };
        var path = @"D:\Dasha\Programming\Gropus\NetworkProgramming_PD_421-master\06_SendMessage_Smtp\SendMessage_Smtp\bin\Debug\net8.0-windows7.0\img.avif";
        // create an image attachment for the file located at path
        var attachment = new MimePart("image", "gif")
        {
            Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(path)
        };

        // now create the multipart/mixed container to hold the message text and the
        // image attachment
        var multipart = new Multipart("mixed");
        multipart.Add(body);
        multipart.Add(attachment);

        // now set the multipart/mixed as the message body
        message.Body = multipart;*//*

        var builder = new BodyBuilder();

        // Set the plain-text version of the message text
        builder.TextBody = @"Hey Alice,

            What are you up to this weekend? Monica is throwing one of her parties on
            Saturday and I was hoping you could make it.

            Will you be my +1?

            -- Joey
            ";

        // In order to reference selfie.jpg from the html text, we'll need to add it
        // to builder.LinkedResources and then use its Content-Id value in the img src.
        var image = builder.LinkedResources.Add(@"D:\Dasha\Programming\Gropus\NetworkProgramming_PD_421-master\06_SendMessage_Smtp\SendMessage_Smtp\bin\Debug\net8.0-windows7.0\img.avif");
        image.ContentId = MimeUtils.GenerateMessageId();

        // Set the html version of the message text
        builder.HtmlBody = string.Format($@"<p style = 'color:red;'>Hey Alice,<br>
                <p>What are you up to this weekend? Monica is throwing one of her parties on
                Saturday and I was hoping you could make it.<br>
                <p>Will you be my +1?<br>
                <p>-- Joey<br>
                <center><img src='cid:{image.ContentId}'></center>");

        // We may also want to attach a calendar event for Monica's party...
        builder.Attachments.Add(@"D:\Dasha\Programming\Gropus\NetworkProgramming_PD_421-master\06_SendMessage_Smtp\SendMessage_Smtp\bin\Debug\net8.0-windows7.0\text.txt");

        // Now we just need to set the message body and we're done
        message.Body = builder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);
            client.Send(message);
        }*/

        //////// Get Mails (IMAP)
        Console.OutputEncoding = Encoding.UTF8;
        using(var client = new ImapClient())
        {
            client.Connect("imap.gmail.com",993,MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);
            foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
            {
                Console.WriteLine(item.Name);
            }
            client.GetFolder(MailKit.SpecialFolder.Sent).Open(MailKit.FolderAccess.ReadWrite);
            var folder = client.GetFolder(MailKit.SpecialFolder.Sent);
            var id = folder.Search(SearchQuery.All)[folder.Search(SearchQuery.All).Count - 1];
            var message = folder.GetMessage(id);
            Console.WriteLine($"{message.Date} {message.Subject}");

            //folder.MoveTo(id, client.GetFolder(MailKit.SpecialFolder.Trash));
            folder.AddFlags(id, MessageFlags.Deleted, true);
            folder.Expunge();

            /*foreach (var item in folder.Search(SearchQuery.All))
            {
                var message = folder.GetMessage(item);
                Console.WriteLine($"{message.Date} {message.Subject}");
            }*/
            client.Inbox.Open(MailKit.FolderAccess.ReadWrite);
            var inbox = client.Inbox.Search(SearchQuery.All);
            foreach (var item in inbox)
            {
                var m = client.Inbox.GetMessage(item);
                Console.WriteLine($"{m.Date} {m.Subject} \n ");
            }
        }

        
    }
}