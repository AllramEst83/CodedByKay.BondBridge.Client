namespace CodedByKay.BondBridge.Client.Behaviors
{
    public class PasswordValidationBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool?), typeof(PasswordValidationBehavior), false, BindingMode.TwoWay);

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

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                var password = entry.Text;
                IsValid = password.Length >= 6 && IsValidPassword(password);
            }
        }

        private bool IsValidPassword(string value)
        {
            foreach (char c in value)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
