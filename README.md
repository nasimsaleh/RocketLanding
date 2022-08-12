# RocketLanding

This library allows Rockets to check for landing location by providing x,y coordinates.
Platfrom starting coordinates are (5,5). The whole landing area is 100*100. Platfrom width and height shouldn't exceed 95 otherwise exception is thrown.


Basic Usage

```
{
  //create a landing platform
  LandingPlatform platform = new LandingPlatform();
  
  try{
    //set platform dimensions
    platform.SetPlatformDimensions(100,100);
    //throws PlatformOutOfLandingAreaException Exception, because platform becomes outside landing area. 
  }
  catch(PlatformOutOfLandingAreaException ex){
  }
  //set platfrom dimensions
  platform.SetPlatformDimensions(30,40);
  
  //check location
  var result = platform.CheckLocation(10,10);   
  //result = 'ok for landing'
  
  
   //check adjacent location
  var result = platform.CheckLocation(10,11);
  //result = 'clash'
  
  //check out of platform location
  result = platform.CheckLocation(4,15);
  
 
}
```
