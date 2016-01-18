using Misakai.Storage;

namespace Demo
{
    /// <summary>
    /// Represents a provider of actors.
    /// </summary>
    public class ActorProvider : EntityCacheProvider<Actor>
    {
        /// <summary>
        /// Constructs a default provider.
        /// </summary>
        public ActorProvider() : base(EntityContext.Default, 1000)
        {
            
        }
        
        /// <summary>
        /// Gets an account by the OAuth id.
        /// </summary>
        /// <param name="provider">The OAuth provider to use.</param>
        /// <param name="oauth">The id provided by OAuth.</param>
        /// <returns>The account retrieved.</returns>
        public Actor FromOAuth(string provider, string oauth)
        {
            // Construct the tag for OAuth
            var tag = provider + ":" + oauth;

            // Get by tag
            var done = this.GetByTag(tag);
            if (done.Success)
                return done.Value;
            return null;
        }
    }


}
