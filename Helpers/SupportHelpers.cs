using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Squabble.Helpers
{
    public static class SupportHelpers
    {
        public static int FindIdFromToken(IEnumerable<Claim> claims)
        {
            return (from claim in claims where claim.Type.Equals("account_id") select int.Parse(claim.Value)).FirstOrDefault();
        }

        public static string GetMethodContextName()
        {
            var name = new StackTrace().GetFrame(1).GetMethod().GetMethodContextName();
            return name;
        }

        public static string GetMethodContextName(this MethodBase method)
        {
            if (method.DeclaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
            {
                var generatedType = method.DeclaringType;
                var originalType = generatedType.DeclaringType;
                var foundMethod = originalType.GetMethods(
                      BindingFlags.Instance | BindingFlags.Static
                    | BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.DeclaredOnly)
                    .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                return foundMethod.DeclaringType.Name + "." + foundMethod.Name;
            }
            else
            {
                return method.DeclaringType.Name + "." + method.Name;
            }
        }
    }
}
