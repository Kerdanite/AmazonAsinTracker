# AmazonAsinTracker
Track amazon article review by asin code

## Exercise purpose

https://www.notion.so/Backend-Home-Project-5501b0773ec048b289703f7666875141

## How to test solution
Launch  AmazonAsinTrackerCron and AmazonAsinTrackerApi simultanously
After that, use the endpoint provided by the WebApi to track products, and after that to get product tracked reviews (just wait 1 minute between tracking and retreive reviews)

## TODO list to go from POC to Live
A lot of trade-offs have been made to try to respect a time constraint of the exercise, so to go from POC to live, the next things have to be taken into account :

  * Specialize the infrastructure part between the Cron and API Needs and adapt the DI for each.
  * Add for the WebApi Authorization to the endpoint to have an authentified user that use endpoints
  * Persist wich user ask the tracking
  * In a DDD point of view, it should be the user who do the action of trackings
  * The file storage provider is ok for POC purpose, but we have to choose a correct storage provider implementation for live (mongoDb, ...).
  * We currently don't know wich user ask the tracking, so i cannot alert the user when the track is finish, implement that feature with the correct business behaviour (alert by mail, a signalR endpoint..)
  * In the current solution, i retreive only the first 10 review of a product. It could be interesting to implement a system that retreive each review incrementally
  * The current WebApi endpoint send only the 10 more recent reviews, we can add some filtering option (QueryOptions) to allow user to build their own query.
  * There is currently no log or error management, add some logging and error management
  * I currently parse and persist only Title, date, and score of a review, we can persist more informations of the review (author, content, Verified Purchase, number of usefull votes, images ...)
