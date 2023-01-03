using System.Collections.Generic;
using System.Web.Mvc;

namespace Advogado.Helpers
{
    public class Alert
    {
        public string AlertStyle { get; set; }
        public string Message { get; set; }
    }

    public static class DefaultMessage
    {
        public const string success = "Operação realizada com sucesso!";
        public const string error = "Falha no decorrer da operação!";
    }

    public class NotificationMessage : ActionResult
    {
        private readonly ActionResult _actionResult;
        public string _alertStyle { get; set; }
        public string _message { get; set; }

        public NotificationMessage(ActionResult actionResult, string message, MessageType messageType)
        {
            _actionResult = actionResult;
            _message = message;
            _alertStyle = messageType.ToString();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var alerts = context.Controller.TempData.ContainsKey("Message") ? (List<Alert>)context.Controller.TempData["Message"] : new List<Alert>();
            var alert = new Alert { Message = _message, AlertStyle = _alertStyle };
            alerts.Add(alert);
            context.Controller.TempData["Message"] = alerts;
            _actionResult.ExecuteResult(context);
        }

        public enum MessageType
        {
            success,
            info,
            warning,
            error,
        }
    }

    public static class ActionResultExtensions
    {
        public static ActionResult Success(this ActionResult actionResult, string message = DefaultMessage.success)
        {
            return new NotificationMessage(actionResult, message, NotificationMessage.MessageType.success);
        }

        public static ActionResult Error(this ActionResult actionResult, string message = DefaultMessage.error)
        {
            return new NotificationMessage(actionResult, message, NotificationMessage.MessageType.error);
        }

        public static ActionResult Warning(this ActionResult actionResult, string message)
        {
            return new NotificationMessage(actionResult, message, NotificationMessage.MessageType.warning);
        }

        public static ActionResult Information(this ActionResult actionResult, string message)
        {
            return new NotificationMessage(actionResult, message, NotificationMessage.MessageType.info);
        }
    }
}