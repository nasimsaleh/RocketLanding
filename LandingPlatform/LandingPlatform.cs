
namespace Zartis.RocketLanding;
public class LandingPlatform
{
    private int _platformWidth = 10;
    private int _platformHeight = 10;

    private int _landingAreaWidth = 100;
    private int _landingAreaHeight = 100;

    private int _startX = 5;
    private int _startY = 5;

    private int _lastCheckedX = -2;
    private int _lastCheckedY = -2;

    private readonly object lockingObject = new object();

    public LandingPlatform()
    {

    }

    public LandingPlatform(int width, int height)
    {
        SetPlatformDimensions(width, height);
    }

    /// <summary>
    ///  Function to set dimensions of the platform. If dimensions are out of range of the landing area, an expection is thrown.
    /// </summary>
    /// <param name="width">width of platform</param>
    /// <param name="height">height of platform</param>
    /// <exception cref="PlatformOutOfLandingAreaException"></exception>
    public void SetPlatformDimensions(int width, int height)
    {

        //check width and height
        if (width <= 0 || height <= 0 || width > _landingAreaWidth - _startX || height > _landingAreaHeight - _startY)
        {
            throw new PlatformOutOfLandingAreaException(
                $"Out of range Width and/height Height. Width shoud be in the range(1,{_landingAreaWidth - _startX}) and Height in the range(1,{_landingAreaHeight - _startY})"
                );
        }
        _platformWidth = width;
        _platformHeight = height;

    }

    /// <summary>
    ///  function to check if the passed location parameter is available for landing. 
    ///  a location is 'ok for landing' if it is inside the platform and not adjacent to the last checked location
    /// </summary>
    /// <param name="x">the x point of the location</param>
    /// <param name="y">the y point of the location</param>
    /// <returns>'out of platform' , 'clash' , 'ok for landing' </returns>
    public string CheckLocation(int x, int y)
    {
        //use lock to prevent paralell access to last checked position.
        lock (lockingObject)
        {
            //check if the landing is outside the platform
            //this line could be moved outside the lock, however moving it inside ensures that caller directly
            // tries to hold the lock before any processing. Otherwise, later caller might reach the lock statement before
            if (x < _startX || x > _startX + _platformWidth || y < _startY || y > _startY + _platformHeight)
            {
                return CheckResult.OutOfPlatform;
            }


            //check if it is a clash position (last checked and its surroundings)
            if (x >= _lastCheckedX - 1 && x <= _lastCheckedX + 1 && y >= _lastCheckedY - 1 && y <= _lastCheckedY + 1)
            {
                return CheckResult.Clash;
            }

            //if not out of platform and not clashing positing, return 'ok for landing' and update last checked location.        
            _lastCheckedX = x;
            _lastCheckedY = y;
            return CheckResult.OkForLanding;
        }


    }
}

