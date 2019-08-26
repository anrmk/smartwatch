namespace Core.Enums {
    public enum DeviceStatusEnum {
        /// <summary>
        /// Sign in was seccessful
        /// </summary>
        Success = 0,
        /// <summary>
        /// Device is locked out
        /// </summary>
        LockedOut = 1,

        /// <summary>
        /// Sign in requires additional verification (i.e. two factor)
        /// </summary>
        RequiresVerification = 2,

        /// <summary>
        /// Sign in failed
        /// </summary>
        Failure = 3
    }
}
