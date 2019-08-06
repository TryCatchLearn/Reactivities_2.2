namespace Domain
{
    public class UserFollowing
    {
        public string ObserverId { get; set; }
        public virtual AppUser Observer { get; set; }
        public string TargetId { get; set; }
        public virtual AppUser Target { get; set; }
    }
}