namespace Domain.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg="Invalid email or password") : base(msg)
        {
            
        }
    }
}
