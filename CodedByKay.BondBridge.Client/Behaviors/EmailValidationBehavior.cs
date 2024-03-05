using System.ComponentModel.DataAnnotations;

namespace CodedByKay.BondBridge.Client.Behaviors
{
    public class EmailValidationBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool?), typeof(EmailValidationBehavior), false, BindingMode.TwoWay);

        public bool? IsValid
        {
            get => (bool?)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            BindingContext = bindable.BindingContext;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.TextChanged += OnEntryTextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            BindingContext = ((BindableObject)sender).BindingContext;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.BindingContextChanged -= OnBindingContextChanged;
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        /*
        var regexPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*\.[a-zA-Z]{2,6}$";
        var isValid = Regex.IsMatch(email, regexPattern);
        */
        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is Entry entry)
            {
                var email = entry.Text;

                IsValid = new EmailAddressAttribute().IsValid(email);
            }
        }
    }
}
