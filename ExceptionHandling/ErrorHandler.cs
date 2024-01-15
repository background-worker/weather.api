using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ExceptionHandling
{
    public static class ErrorHandler
    {
        public static void ResolveError(HttpStatusCode httpStatusCode)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    {
                        throw new NotFoundException("Weather description for the City you are looking for could not be found.");
                    }
                case HttpStatusCode.BadRequest:
                    {
                        throw new BadRequestException("The request is malformed. Please check the query parameters.");
                    }
                case HttpStatusCode.Unauthorized:
                    {
                        throw new UnauthorizedException("Please enter a valid Api Key");
                    }
            }

        }
    }
}
