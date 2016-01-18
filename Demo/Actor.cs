using Misakai.Storage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo
{
    /// <summary>
    /// Represents a person or an organization.
    /// </summary>
    [Table("dbo.actor")]
    public class Actor : Entity
    {
        /// <summary>
        /// Gets or sets the name of the person/organization.
        /// </summary>
        [Column("name")]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the e-mail address of the person/organization.
        /// </summary>
        [Column("email")]
        public string EMail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL of the photo/avatar image of the person/organization.
        /// </summary>
        [Column("avatar")]
        public string Avatar
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the OAuth provider used for this actor.
        /// </summary>
        [Column("provider")]
        public string Provider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user's bio.
        /// </summary>
        [Column("bio")]
        public string Bio
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user's location.
        /// </summary>
        [Column("location")]
        public string Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user's language.
        /// </summary>
        [Column("language")]
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user's website.
        /// </summary>
        [Column("website")]
        public string Website
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated contract.
        /// </summary>
        [Column("contract")]
        public int ContractId
        {
            get;
            set;
        }
    }

}
