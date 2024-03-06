using CodedByKay.BondBridge.Client.Models;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;

namespace CodedByKay.BondBridge.Client.Managers
{
    /// <summary>
    /// Manages the user session and provides access to session data.
    /// </summary>
    public class UserSessionManager : INotifyPropertyChanged
    {
        private static UserSessionManager? _instance;
        private UserSessionData _sessionData = new();
        
        /// <summary>
        /// Gets the singleton instance of the UserSessionManager.
        /// </summary>
        public static UserSessionManager Instance => _instance ??= new UserSessionManager();

        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get => _sessionData.Roles.Any();
            private set
            {
                if (value != IsAuthenticated)
                {
                    OnPropertyChanged(nameof(IsAuthenticated));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user has admin access.
        /// </summary>
        public bool IsAdmin => _sessionData.Roles.Contains("admin_access");

        private UserSessionManager() { }

        /// <summary>
        /// Updates the user session using the provided JWT token.
        /// </summary>
        /// <param name="jwtToken">The JWT token to extract user data from.</param>
        public void UpdateSession(string jwtToken)
        {
            _sessionData = ExtractUserDataFromToken(jwtToken);

            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsAdmin));
        }

        /// <summary>
        /// Clears the current user session.
        /// </summary>
        public void ClearSession()
        {
            _sessionData = new UserSessionData();

            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsAdmin));
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Extracts user data from a JWT token.
        /// </summary>
        /// <param name="token">The JWT token containing user data.</param>
        /// <returns>The user session data extracted from the token.</returns>
        private UserSessionData ExtractUserDataFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var roles = jwtToken.Claims
                        .Where(c => c.Type == "rol" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        .SelectMany(c => c.Value.Split(','))
                        .ToList();

            var userIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var userId = jwtToken.Claims
                        .FirstOrDefault(c => c.Type == userIdClaimType)?.Value;

            var username = jwtToken.Claims
                          .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (userId == null)
            {
                throw new InvalidOperationException("UserId cannot be null in the JWT token.");
            }

            if (username == null)
            {
                throw new InvalidOperationException("Username cannot be null in the JWT token.");
            }

            return new UserSessionData
            {
                Roles = roles,
                UserId = userId,
                Username = username
            };
        }

    }
}
