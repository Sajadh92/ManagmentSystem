using Infrastructure.AppException;
using Infrastructure.Cache;
using Infrastructure.Service;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Twilio;

public interface ITwilio
{
    Task<int> SendOTPCodeToPhoneNo(string PhoneNo);
    void VerifyOTPCodeSendToPhoneNo(string PhoneNo, string OTPCode, bool RemoveIfVerified);
    void RemoveFromCache(string PhoneNo);
}

public class TwilioService : ITwilio, ISingleton
{
    private readonly IAppMemoryCache _cache;

    public TwilioService(IAppMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<int> SendOTPCodeToPhoneNo(string PhoneNo)
    {
        int seconds_to_wait = _cache.Get<int?>($"WAITING_LIST_{PhoneNo}") ?? 0;

        if (seconds_to_wait > 0)
        {
            throw new LogicException($"WaitingFor[{seconds_to_wait}]SecondsThenTryAgain");
        }

        string OTPCode = new Random().Next(1000, 9999).ToString();

        TwilioClient.Init("TwilioAccountSid", "TwilioAuthToken");

        MessageResource _response = await MessageResource.CreateAsync
        (
            body: $"رمز التحقق الخاص بك هو {OTPCode}",
            from: new global::Twilio.Types.PhoneNumber($"whatsapp:TwilioPhone"), 
            to: new global::Twilio.Types.PhoneNumber($"whatsapp:+964{PhoneNo}")
        );

        bool _isDelivered = _response.Status == MessageResource.StatusEnum.Queued;

        if (!_isDelivered)
        {
            throw new LogicException("Error");
        }

        _cache.Set($"OTP_{PhoneNo}", OTPCode, (int)TimeSpan.FromMinutes(15).TotalSeconds);

        int send_attempts_count = _cache.Get<int?>($"OTP_ATTEMPTS_{PhoneNo}") ?? 1;

        int total_seconds;

        switch (send_attempts_count)
        {
            case 1:
                total_seconds = (int)TimeSpan.FromMinutes(1).TotalSeconds;
                break;
            case 2:
                total_seconds = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                break;
            case 3:
                total_seconds = (int)TimeSpan.FromMinutes(10).TotalSeconds;
                break;
            default:
                total_seconds = (int)TimeSpan.FromDays(1).TotalSeconds;
                break;
        }

        _cache.Set($"WAITING_LIST_{PhoneNo}", total_seconds, total_seconds);

        if (send_attempts_count <= 3)
        {
            _cache.Set($"OTP_ATTEMPTS_{PhoneNo}", send_attempts_count + 1, (int)TimeSpan.FromHours(24).TotalSeconds);
        }
        else 
        {
            _cache.Remove($"OTP_ATTEMPTS_{PhoneNo}");
        }

        return total_seconds;
    }

    public void VerifyOTPCodeSendToPhoneNo(string PhoneNo, string OTPCode, bool RemoveIfVerified)
    {
        string? code = _cache.Get<string>($"OTP_{PhoneNo}");

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new LogicException("PhoneNoNotRegistered");
        }

        if (!OTPCode.Equals(code))
        {
            throw new LogicException("InvalidOTPCode");
        }

        if (RemoveIfVerified)
        {
            RemoveFromCache(PhoneNo); 
        }
    }

    public void RemoveFromCache(string PhoneNo)
    {
        _cache.Remove($"OTP_{PhoneNo}");
        _cache.Remove($"WAITING_LIST_{PhoneNo}");
        _cache.Remove($"OTP_ATTEMPTS_{PhoneNo}");
    }
}
