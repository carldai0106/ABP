using System.Web.Mvc;

namespace Abp.Web.Mvc.Controllers
{
    /// <summary>
    /// Types of message
    /// </summary>
    public enum MessageTypes
    {
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Information
        /// </summary>
        Information
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ControllerExtension
    {
        /// <summary>
        /// Output error messages to ModelState
        /// </summary>
        /// <param name="controller">Current Controller</param>
        public static void OutputModelMessage(this Controller controller)
        {
            if (controller.TempData["ModelState"] != null)
            {
                controller.ViewBag.HasMessage = true;
                var dics = controller.TempData["ModelState"] as ModelStateDictionary;
                if (dics != null)
                    foreach (var item in dics)
                    {
                        if (item.Value != null)
                        {
                            foreach (var error in item.Value.Errors)
                            {
                                controller.ModelState.AddModelError(item.Key, error.ErrorMessage);
                            }
                        }
                    }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        public static void InputModelError(this Controller controller, MessageTypes type = MessageTypes.Error)
        {
            controller.TempData["MessageType"] = type == MessageTypes.Error ? "error" : "";
            controller.TempData["ModelState"] = controller.ModelState;
        }

        /// <summary>
        /// Add error message to ModelState
        /// </summary>
        /// <param name="controller">Current Controller</param>
        /// <param name="message">Message</param>
        /// <param name="type"></param>
        public static void AddModelMessage(this Controller controller, string message, MessageTypes type = MessageTypes.Error)
        {
            controller.ModelState.AddModelError("", message);
            controller.InputModelError(type);
        }

        /// <summary>
        /// Add error message to ModelState
        /// </summary>
        /// <param name="controller">Current Controller</param>
        /// <param name="key">The Key</param>
        /// <param name="message">Message</param>
        /// <param name="type"></param>
        public static void AddModelMessage(this Controller controller, string key, string message, MessageTypes type = MessageTypes.Error)
        {
            controller.ModelState.AddModelError(key, message);
            controller.InputModelError(type);
        }
    }
}
