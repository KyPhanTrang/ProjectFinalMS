using Microsoft.AspNetCore.Http;
using MISA.WorkShiftManagement.Core.Exceptions;
using MISA.WorkShiftManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WorkShiftManagement.Core.Middleware
{
    public class MISAErrorExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public MISAErrorExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Chặn request từ trên xuống để xử lý
                await _next(context);
                // Bắt exception dưới đẩy lên
                //await context.Response.WriteAsync("This is custom Middleware (down to up).");
            }
            catch (ValidateException ex)
            {
                // Khai báo và gán dữ liệu trả về client
                var res = new ErrorResponseResult();
                res.Code = "400";
                res.Message = ex.Message;
                res.Data = ex.Data;

                // Trả response về cho client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                var json = System.Text.Json.JsonSerializer.Serialize(res);
                await context.Response.WriteAsync(json);
            }
            catch (DataAccessException ex)
            {
                // Khai báo và gán dữ liệu trả về client
                var res = new ErrorResponseResult();
                res.Code = "500";
                res.Message = ex.Message;

                // Trả response về cho client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var json = System.Text.Json.JsonSerializer.Serialize(res);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                // Khai báo và gán dữ liệu trả về client
                var res = new ErrorResponseResult();
                res.Code = "500";
                res.Message = ex.Message;

                // Trả response về cho client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var json = System.Text.Json.JsonSerializer.Serialize(res);
                await context.Response.WriteAsync(json);
            }
        }
    }
}