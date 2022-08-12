
using Zartis.RocketLanding;
namespace RocketLanding.Test;


public class LandingPlatformTest
{
    [Fact]
    public void LandingPlatform_DimensionsOutOfRange_ThrowsException()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();

        //act and assert
        Assert.Throws<PlatformOutOfLandingAreaException>(() => platform.SetPlatformDimensions(100,100));
    }

    [Fact]
    public void LandingPlatform_CheckOutsidePlatformAfterSettingDimension_ReturnsOutOfPlatform()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();
        int width = 20;
        int height = 20;
        int x = 26, y = 15;
        var expected = CheckResult.OutOfPlatform;

        //act
        platform.SetPlatformDimensions(width,height);
        var result = platform.CheckLocation(x,y);
        //assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void LandingPlatform_CheckEdgeOfPlatformAfterSettingDimension_ReturnsOkForLanding()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();
        int width = 20;
        int height = 30;
        int x = 25, y = 35;
        var expected = CheckResult.OkForLanding;

        //act
        platform.SetPlatformDimensions(width,height);
        var result = platform.CheckLocation(x,y);
        //assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void LandingPlatform_CheckAlreadyCheckedLocation_ReturnsClash()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();

        int x = 5, y = 5;
        string expected1 = CheckResult.OkForLanding;
        string expected2 = CheckResult.Clash;
        //act
        var result1 = platform.CheckLocation(x,y);
        var result2 = platform.CheckLocation(x,y);

        //assert
        Assert.Multiple(
        ()=> Assert.Equal(expected1, result1), 
        ()=> Assert.Equal(expected2, result2)
        );    
    }

    [Fact]
    public void LandingPlatform_CheckAdjacentToAlreadyCheckedLocation_ReturnsClash()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();

        int x = 5, y = 5;
        int x1 = 6, y1= 6;
        string expected1 = CheckResult.OkForLanding;
        string expected2 = CheckResult.Clash;
        //act
        var result1 = platform.CheckLocation(x,y);
        var result2 = platform.CheckLocation(x1,y1);

        //assert
        Assert.Multiple(
        ()=> Assert.Equal(expected1, result1), 
        ()=> Assert.Equal(expected2, result2)
        );    
    } 
    [Fact]
    public void LandingPlatform_CheckIfLastCheckedIsUpdated_ReturnsOkForLanding()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();
        int x = 5, y = 5;
        int x1 = 10, y1= 10;
        int x2 = 5, y2 = 5;
        string expected = CheckResult.OkForLanding;
        
        //act
        var result = platform.CheckLocation(x,y);
        var result1 = platform.CheckLocation(x1,y1);
        var result2 = platform.CheckLocation(x2,y2);

        //assert        
        Assert.Equal(expected, result2);       
            
    } 

    [Fact]
    public void LandingPlatform_ParallelCheckAdjacentPositions_ReturnsOneOkForLanding()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();
        var tasks = new List<Task>();
        int clashCount = 0;
        int okForLandingCount = 0 ;
        object clashLock = new object();
        object okForLandingLock = new object();
        var data = new List<Point>{
            new Point(5,5),
            new Point(5,6),
            new Point(6,5),
            new Point(6,6),
           
        };
       var expectedOk = 1;
       var expectedClash =3; 

       //act
        Parallel.ForEach(data, point =>
            {
                var result = platform.CheckLocation(point.X,point.Y);
                if(result == CheckResult.Clash){
                    lock(clashLock){
                        clashCount++;
                    }
                }else if(result == CheckResult.OkForLanding){
                    lock(okForLandingLock){
                        okForLandingCount++;
                    }
                }
            }); 

        //assert
        Assert.Multiple(
            ()=> Assert.Equal(expectedOk, okForLandingCount), 
            ()=> Assert.Equal(expectedClash, clashCount)
        );
    }
    [Fact]
    public void LandingPlatform_ParallelCheckNotAdjacentPositions_ReturnsAllOkForLanding()
    {
        //arrange
        LandingPlatform platform = new LandingPlatform();
        var tasks = new List<Task>();
        int okCount = 0 ;
        object okForLandingLock = new object();
        var data = new List<Point>{
            new Point(5,5),
            new Point(5,7),
            new Point(7,5),
            new Point(7,7),
           
        };
       var expected = 4;

       //act
        Parallel.ForEach(data, point =>
            {
                var result = platform.CheckLocation(point.X,point.Y);
                if(result == CheckResult.OkForLanding){
                    lock(okForLandingLock){
                        okCount++;
                    }
                }
            }); 
        //assert
        Assert.Equal(expected, okCount);
        
    }

   


    internal class Point{
        public int X {get;set;}
        public int Y {get;set;}
        public Point(int x, int y){
            X = x;
            Y = y;
        }
    } 
}