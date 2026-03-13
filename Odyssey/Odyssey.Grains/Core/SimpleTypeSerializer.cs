namespace Odyssey.Grains.Core
{
    public class SimpleTypeSerializer
    {
        public static string TypeToString(Type type)
        {
            if (type.IsGenericType)
            {
                if (type.ContainsGenericParameters)
                    return $"{GetBaseTypeName(type)}<{string.Join("", Enumerable.Range(0, type.GetGenericArguments().Length - 1).Select(_ => ','))}>";

                var genericArguments = type.GetGenericArguments();

                var genericArgsString = string.Join(',', genericArguments.Select(TypeToString));
                return $"{GetBaseTypeName(type)}<{genericArgsString}>";
            }

            return GetBaseTypeName(type);
        }
        private static string GetBaseTypeName(Type type)
        {
            return type.Name.Split('`')[0];
        }
    }
}
