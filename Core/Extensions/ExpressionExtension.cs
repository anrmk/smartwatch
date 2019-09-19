using System;
using System.Linq.Expressions;

namespace Core.Extensions {
    public class ExpressionExtension {
        public static Func<TSource, string> GetFunc<TSource>(string propertyName) {
            return GetExpression<TSource>(propertyName).Compile();  //only need compiled expression
        }

        public static Expression<Func<TSource, string>> GetExpression<TSource>(string propertyName) {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(string));
            return Expression.Lambda<Func<TSource, string>>(conversion, param);
        }
    }


}
