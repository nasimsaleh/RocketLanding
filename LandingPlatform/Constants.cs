
namespace Zartis.RocketLanding;
public  class CheckResult{
   
    public readonly IEnumerable<string> List = new List<string>{
        OkForLanding,
        Clash,
        OutOfPlatform
    };
    public  const string OkForLanding = "ok for landing";
    public  const string Clash = "clash";
    public  const string OutOfPlatform = "out of platform";
}