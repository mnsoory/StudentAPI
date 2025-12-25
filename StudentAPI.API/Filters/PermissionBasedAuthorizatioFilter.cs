using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentAPI.API.Attributes;
using System.Security.Claims;

namespace StudentAPI.API.Filters
{
    public class PermissionBasedAuthorizatioFilter : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var attribute = context.ActionDescriptor
                .EndpointMetadata
                .FirstOrDefault(x => x is CheckPermissionAttribute);

            if (attribute != null)
            {
                var userIdentity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (userIdentity == null || !userIdentity.IsAuthenticated)
                {
                    context.Result = new ObjectResult(new
                    {
                        Message = "You are not authorized. Please login first."
                    })
                    { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    var attributePermission = ((CheckPermissionAttribute)attribute).Permission;
                    var userPermissions = context.HttpContext.User.FindAll("Permission");
                    bool hasPermission = userPermissions.Any(x => x.Value == attributePermission.ToString());

                    if (!hasPermission)
                    {
                        context.Result = new ObjectResult(new
                        {
                            Message = $"Access Denied. You do not have the required permission"
                        })
                        { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
