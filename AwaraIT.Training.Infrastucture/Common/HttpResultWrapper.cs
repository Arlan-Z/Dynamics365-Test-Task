using System;
using System.Net;
using System.Text.Json;

namespace AwaraIT.Training.Infrastucture.Common
{
    public struct Void
    {
        public static implicit operator Void(string value)
        {
            return new Void();
        }
    }

    public class HttpResultWrapper<T>
    {
        public static HttpResultWrapper<T> Ok()
        {
            return new HttpResultWrapper<T> { HttpStatus = HttpStatusCode.OK };
        }

        public static HttpResultWrapper<T> Ok(string responseText)
        {
            return new HttpResultWrapper<T>(responseText, HttpStatusCode.OK);
        }

        public static HttpResultWrapper<T> Ok(T value)
        {
            return new HttpResultWrapper<T> { Result = value, HttpStatus = HttpStatusCode.OK };
        }

        public static HttpResultWrapper<T> Fail(HttpStatusCode status, string responseText)
        {
            return new HttpResultWrapper<T>(responseText, status);
        }

        public HttpResultWrapper()
        { }

        public HttpResultWrapper(string responseText, HttpStatusCode httpStatus)
        {
            ResponseText = responseText;
            HttpStatus = httpStatus;

            if (!string.IsNullOrEmpty(ResponseText))
            {
                if (Success)
                {
                    if (typeof(T) == typeof(string))
                    {
                        Result = (T)Convert.ChangeType(ResponseText, typeof(T));
                    }
                    else if (typeof(T) == typeof(Void))
                    {
                        Result = (T)(object)new Void();
                    }
                    else
                    {
                        Result = JsonSerializer.Deserialize<T>(ResponseText);
                    }
                }
            }
        }

        public T Result { get; protected set; }

        public string ResponseText { get; protected set; }

        public HttpStatusCode HttpStatus { get; protected set; }

        public bool Success
        {
            get
            {
                return (int)HttpStatus >= 200 && (int)HttpStatus <= 299;
            }
        }
    }
}
