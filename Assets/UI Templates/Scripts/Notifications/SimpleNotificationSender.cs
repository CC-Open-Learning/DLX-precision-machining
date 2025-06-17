using UnityEngine;

namespace RemoteEducation.UI
{
    /// <summary>
    ///     Exposes methods which can be used in serialized UnityEvents to
    ///     display a Notification with the default settings and a custom method
    ///     at one of the three available screen positions.
    /// </summary>
    /// <remarks>
    ///     This component is created for example purposes and should be replaced
    ///     by a more specialized-purpose implementation of Notification sending as 
    ///     needed for the project.
    /// </remarks>
    public class SimpleNotificationSender : MonoBehaviour
    {

        /// <summary>
        ///     Creates a <see cref="Notification"/> in the <see cref="NotificationManager"/>
        ///     using the provided <paramref name="message"/> at the specified <paramref name="position"/>.
        /// </summary>
        /// <remarks>
        ///     All other settings for the <see cref="NotificationData"/> are defaults.
        /// </remarks>
        /// <param name="message"></param>
        /// <param name="position"></param>
        private void SendInfoMessage(string message, NotificationManager.Position position)
        {
            NotificationData data = NotificationManager.DefaultNotificationData;

            data.message = message;

            NotificationManager.CreateNotification(data, position);
        }

        public void SendInfoMessageTop(string message)
        {
            SendInfoMessage(message, NotificationManager.Position.Top);
        }

        public void SendInfoMessageMiddle(string message)
        {
            SendInfoMessage(message, NotificationManager.Position.Middle);
        }

        public void SendInfoMessageBottom(string message)
        {
            SendInfoMessage(message, NotificationManager.Position.Bottom);
        }
    }
}