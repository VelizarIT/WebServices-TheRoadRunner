﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JustRunnerChat.Models;

namespace JustRunnerChat.Controllers
{
    public class BaseApiController : ApiController
    {
        private static Dictionary<string, HttpStatusCode> ErrorToStatusCodes = new Dictionary<string, HttpStatusCode>();

        static BaseApiController()
        {
            ErrorToStatusCodes["ERR_GEN_SVR"] = HttpStatusCode.InternalServerError;
            ErrorToStatusCodes["INV_OP_TURN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["ERR_INV_NUM"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["ERR_INV_USR"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_USRNAME_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_NKNAME_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_CHNAME_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_USRNAME_CHARS"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_NKNAME_CHARS"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_CHNAME_CHARS"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_NICK_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_NICK_CHARS"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["INV_USR_AUTH_LEN"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["ERR_DUP_USR"] = HttpStatusCode.Conflict;
            ErrorToStatusCodes["ERR_DUP_CHNAME"] = HttpStatusCode.Conflict;
            ErrorToStatusCodes["ERR_DUP_NICK"] = HttpStatusCode.Conflict;
            ErrorToStatusCodes["INV_USR_AUTH"] = HttpStatusCode.BadRequest;
            ErrorToStatusCodes["ERR_JOINED_CHANNEL"] = HttpStatusCode.BadRequest;
        }

        public BaseApiController()
        {
        }

        protected HttpResponseMessage PerformOperation(Action action)
        {
            try
            {
                action();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServerErrorException ex)
            {
                return BuildErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                var errCode = "ERR_GEN_SVR";
                return BuildErrorResponse(ex.Message, errCode);
            }
        }

        protected HttpResponseMessage PerformOperation<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (ServerErrorException ex)
            {
                return BuildErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                var errCode = "ERR_GEN_SVR";
                return BuildErrorResponse(ex.Message, errCode);
            }
        }

        private HttpResponseMessage BuildErrorResponse(string message, string errCode)
        {
            var httpError = new HttpError(message);
            httpError["errCode"] = errCode;
            var statusCode = ErrorToStatusCodes[errCode];
            return Request.CreateErrorResponse(statusCode, httpError);
        }
    }
}
