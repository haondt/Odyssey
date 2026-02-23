using System.Linq.Expressions;
using System.Reflection;

namespace Haondt.Web.UI.Models
{
    public class ExpressionUtils
    {

        public static (MemberInfo memberInfo, string path) GetMemberInfo(LambdaExpression expression)
        {
            var parts = new List<string>();
            MemberInfo? leaf = null;

            Expression? current = expression.Body;

            // Unwrap implicit cast to object (happens with value types)
            if (current is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
                current = unary.Operand;

            while (current is MemberExpression member)
            {
                parts.Insert(0, member.Member.Name);
                leaf ??= member.Member;
                current = member.Expression;
            }

            if (leaf is null)
                throw new ArgumentException("Expression must be a member access expression.");

            var path = string.Join(".", parts);
            return (leaf, path);
        }
    }
}
