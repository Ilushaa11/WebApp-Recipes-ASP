using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace courseA4.Services
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement
    {
        public CustomAuthorizeAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles); 
        }
    }

    public class CustomAuthorizeHandler : AuthorizationHandler<CustomAuthorizeAttribute>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizeAttribute requirement)
        {
            var userRoles = context.User?.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            // Если роли пользователя совпадают с требованиями, то авторизация прошла успешно
            if (userRoles != null && userRoles.Any(role => requirement.Roles.Contains(role)))
            {
                context.Succeed(requirement); // Успешная авторизация
            }
            else
            {
                context.Fail(); // Неудачная авторизация
            }

            return Task.CompletedTask; // Завершаем задачу
        }
    }
}