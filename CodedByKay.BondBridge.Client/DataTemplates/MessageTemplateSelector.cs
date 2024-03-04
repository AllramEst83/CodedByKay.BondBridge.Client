namespace CodedByKay.BondBridge.Client.DataTemplates
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the text of the message.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the message is from the current user.
        /// True if the message is from the current user, false otherwise.
        /// </summary>
        public bool IsCurrentUser { get; set; }
    }

    /// <summary>
    /// Selects a data template based on the message's origin (current user or other user).
    /// </summary>
    public class MessageTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the template for messages from the current user.
        /// </summary>
        public DataTemplate CurrentUserTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template for messages from other users.
        /// </summary>
        public DataTemplate OtherUserTemplate { get; set; }

        /// <summary>
        /// Selects a template based on the message's origin.
        /// </summary>
        /// <param name="item">The message item for which to select the template.</param>
        /// <param name="container">The container that the template will be applied to.</param>
        /// <returns>The appropriate data template based on whether the message is from the current user or another user.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the item is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the item is not of type Message.</exception>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }

            if (!(item is Message message))
            {
                throw new ArgumentException("Item is not of type Message.", nameof(item));
            }

            return message.IsCurrentUser ? CurrentUserTemplate : OtherUserTemplate;
        }
    }
}
