using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderDeliveryTracker.Models
{
    public enum LoginOutput
    {
        UserDoesNotExist,
        InvalidPassword,
        Error,
        Success
    }

    public enum CreateUserOutput
    {
        UserAlreadyExist,
        Error,
        Success = 3
    }

}