using MailKit.Security;
using MimeKit;
using System;
using System.Windows.Controls;

namespace Diary4CuratorFullEdition.Auxiliary.Classes
{
	class SenderEmail
	{
		public void SendCode(TextBox email)
		{
			Random rnd = new Random();

			SenderEmailVariables.code = rnd.Next(10000, 99999);

			MimeMessage mm = new MimeMessage();
			mm.From.Add(new MailboxAddress("", "diary4curator@gmail.com"));
			mm.To.Add(new MailboxAddress("", email.Text));
			mm.Subject = "Код для смены пароля от Diary4Curator";

			BodyBuilder bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = $"Код для смены пароля: {SenderEmailVariables.code}";
			mm.Body = bodyBuilder.ToMessageBody();

			using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
			{
				smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				smtpClient.Authenticate("diary4curator@gmail.com", "rfdmoksbopavpgcx");
				smtpClient.Send(mm);
				smtpClient.Disconnect(true);
			}
		}
	}
}
