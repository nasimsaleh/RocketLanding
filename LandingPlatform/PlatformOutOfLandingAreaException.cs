
namespace Zartis.RocketLanding;

public class PlatformOutOfLandingAreaException : Exception{
    public PlatformOutOfLandingAreaException() { }

    public PlatformOutOfLandingAreaException(string message)
        : base(message) { }

    public PlatformOutOfLandingAreaException(string message, Exception inner)
        : base(message, inner) { }
}