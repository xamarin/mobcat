namespace Microsoft.MobCAT.Services
{
    public interface IRuntimeEnvironmentService
    {
        /// <summary>
        /// Gets a value indicating the number of processor cores available.
        /// </summary>
        /// <value>Number of processor cores available.</value>
        uint ProcessorCores { get; }
    }
}