using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public class TicketSearchStatus
    {
        public const String SearchingStatus = "Searching";
        public const String FirstPageStatus = "First Page Loaded";
        public const String HoldingTicket = "Finding Ticket";
        public const String CaptchaPageStatus = "Captcha Page Loaded";
        public const String ByPassingSolveMediaCaptcha = "Captcha Page Loaded";
        public const String RefreshPageStatus = "Refresh Page Loaded";
        public const String QueuePageStatus = "Queue Page Loaded";
        public const String RefreshingQueuePageStatus = "Refreshing Queue Page";
        public const String WaitingRoomStatus = "Waiting Page Loaded";
        public const String FoundPageStatus = "Ξ§ Ticket Found !!! §Ξ";
        public const String DeliveryPageStatus = "Delivery Page Loaded";
        public const String SigninPageStatus = "Sign in Page Loaded";
        public const String PaymentPageStatus = "Payment Page Loaded";
        public const String VerificationPageStatus = "Verification Page Loaded";
        public const String ConfirmationPagetStatus = "Confirmation Page Loaded";
        public const String CreatePageStatus = "Create Page Loaded";
        public const String FoundCriteriaDoesNotMatch = "Ticket Criteria Unmatched";
        public const String RetryingStatus = "Retrying...";
        public const String ResolvingCaptchaStatus = "Solving Captcha";
        public const String ResolvingCFCaptchaStatus = "Solving CF Captcha";
        public const String CaptchaPollStatus = "Captcha polling...";
        public const String ManualCaptchaStatus = "Enter Captcha Manually";
        public const String CaptchaResolvedStatus = "Captcha Resolved";
        public const String ErrorStatus = "Error";
        public const String StopStatus = "Stopped";
        public const String RestartingStatus = "Restarting";
        public const String StoppingStatus = "Stopping";
        public const String PausingRequest = "Pausing...";
        public const String ChangeParameterRequest = "Select Event Date";
        public const String MoreInfoTicketOutDated = "Ticket maybe expired or out dated. Please review.";
        public const String MoreInfoParamterNotMatch = "Current parameter does not match. Please review.";
        public const String MoreInfoPriceLevelNotMatch = "Provided price level does not match. Please review.";
        public const String MoreInfoEventNotAvaiable = "Event is not currently available. Please review.";
        public const String MoreInfoTicketTypeStringNotMatch = "Ticket Type String does not match.";
        public const String MoreInfoSectionStringNotMatch = "Section does not match.";
        public const String MoreInfoLocationStringNotMatch = "Location does not match.";
        public const String MoreInfoAdditionalStringNotMatch = "Additional does not match.";
        public const String MoreInfoQuantityNotMatch = "Quantity does not match.";
        public const String MoreInfoHoldTimeExpired = "Ticket hold time has expired.";
        public const String MoreInfoTicketTypePasswordNotExist = "Ticket Type Password does not exist in the event page.";
        public const String MoreInfoSiteUnavailable = "Unavailable due to routine maintenance.";
        public const String MoreInfoAutoCaptchaError = "Auto-captcha error: ";
        public const String MoreInfoCriteriaDoesNotMatch = "Ticket Found, criteria unmatched.";
        public const String MoreInfoDeliveryOptionDoesNotMatch = "Delivery option does not match.";
        public const String MoreInfoAccountNotAvailable = "No account available for autobuying.";
        public const String MoreInfoAccountIncorrect = "Invalid username or password.";
        public const String MoreInfoTicketBought = "Ticket bought!";
        public const String MoreInfoNoCreditCardInfoMatch = "No credit card info match.";
        public const String MoreInfoBuyingFailed = "Buying failed. ";
        public const String MoreInfoBuyingInProgress = "Buying in progress.";
        public const String MoreInfoProxyNotAvaiable = "Proxy is not available.";
        public const String MoreInfoNoPaymentInfoDefined = "Payment information does not found.";
        public const String BuyingInProgress = "Buying in Progress";
        public const String MoreInfoTicketSoldOut = "Ticket is Sold Out. Please review.";
        public const String MoreInfoNoTicketTypeString = "There isn't any Ticket Type in this Event";
    }

    public class Notification
    {
        public const String CSAlert = "Kindly select any Captcha Services, selected service rights has been taken. Contact Admin.";
    }
}
