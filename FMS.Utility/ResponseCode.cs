namespace FMS.Utility
{
    public class ResponseCode
    {
        public enum Status : int
        {
            OK = 200, //The request was successful and the server has returned the requested data.

            Created = 201, //The request was successful and a new resource has been created as a result.

            NoContent = 204, // The request was successful but there is no data to return.

            MovedPermanently = 301,

            Found = 302,

            BadRequest = 400, // The request was malformed or invalid.

            Unauthorized = 401, //The request requires authentication and the client is not authenticated.

            Forbidden = 403, //The client is authenticated but does not have access to the requested resource.

            NotFound = 404, //The requested resource could not be found

            RequestTimeout = 408,

            TooManyRequests = 429,

            InternalServerError = 500, //An error occurred on the server.
        }
    }
}
