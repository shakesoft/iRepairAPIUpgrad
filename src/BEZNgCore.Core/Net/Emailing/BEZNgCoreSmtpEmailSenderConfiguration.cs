using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace BEZNgCore.Net.Emailing;

public class BEZNgCoreSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
{
    public BEZNgCoreSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
    {

    }

    public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
}

