using System;
using System.ComponentModel.DataAnnotations;
using tba.Core.Utilities;

namespace tba.Core.Entities
{
    /// <summary>
    /// Entities have 
    ///     Identity - 
    ///     Soft Delete - 
    ///     Audit Behavior - This behavior captures changes on insert, update, and delete
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Database generated identifier 
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// IsDeleted is true when the entity has been deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Create an Audit entity to store the current state of this entity
        /// This happens during a change (aka action)
        /// </summary>
        /// <param name="userId">the user requesting the action</param>
        /// <param name="timestamp">date/time of action</param>
        /// <param name="action">the type of action</param>
        /// <returns>an Audit entity</returns>
        public Audit ToAuditEntity(long userId, long timestamp, AuditActionType action)
        {
            string s;
            try
            {
                s = Serialization.Serialize(this);
            }
            catch (Exception)
            {
                s = string.Empty;
            }
            return new Audit
            {
                UserId = userId,
                EntityId = Id,
                Action = action,
                Timestamp = timestamp,
                SerializedEntity = s
            };
        }

        #region Equality Checks
        public override bool Equals(object entity)
        {
            if (!(entity is Entity))
            {
                return false;
            }

            return (this == (Entity)entity);
        }

        public static bool operator ==(Entity e1, Entity e2)
        {
            if ((object)e1 == null && (object)e2 == null)
            {
                return true;
            }

            if ((object)e1 == null || (object)e2 == null)
            {
                return false;
            }

            return e1.Id == e2.Id;
        }

        public static bool operator !=(Entity e1, Entity e2)
        {
            return (!(e1 == e2));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion

        public class Audit
        {
            /// <summary>
            /// Database generated identifier 
            /// </summary>
            [Required]
            public long Id { get; set; }

            /// <summary>
            /// The user making the change (aka the action)
            /// </summary>
            [Required]
            public long UserId { get; set; }

            /// <summary>
            /// The id of the entity that is changing (aka the action)
            /// </summary>
            [Required]
            public long EntityId { get; set; }

            /// <summary>
            /// A unix timestamp capturing the date/time of the action
            /// </summary>
            [Required]
            public long Timestamp { get; set; }

            /// <summary>
            /// The type of action 
            /// </summary>
            [Required]
            public AuditActionType Action { get; set; }

            /// <summary>
            /// A serialized representation of the entity at the time of the change.
            /// </summary>
            public string SerializedEntity { get; set; }

        }

        public enum AuditActionType
        {
            Insert,         
            Update,          
            Delete          
        }
    }
}