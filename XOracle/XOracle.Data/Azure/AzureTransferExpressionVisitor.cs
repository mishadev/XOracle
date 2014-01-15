using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using XOracle.Domain.Core;

namespace XOracle.Data.Azure
{
    public class AzureTransferExpression<TAzureEntity, TEntity> : ExpressionVisitor
        where TAzureEntity : TableServiceEntity
        where TEntity : Entity
    {
        private ParameterExpression _newparameter;
        private ParameterExpression _oldparameter;

        public static Expression<Func<TAzureEntity, bool>> Transfer(Expression<Func<TEntity, bool>> filter)
        {
            var oldParam = filter.Parameters.FirstOrDefault(p => p.Type == typeof(TEntity));
            if (oldParam == null || filter.Parameters.Except(new[] { oldParam }).Any())
                throw new InvalidOperationException("lambda is not in correct format");

            var newParam = Expression.Parameter(typeof(TAzureEntity));

            var result = new AzureTransferExpression<TAzureEntity, TEntity>(newParam, oldParam).Visit(filter.Body);

            return Expression.Lambda<Func<TAzureEntity, bool>>(result, newParam);
        }

        private AzureTransferExpression(ParameterExpression newparameter, ParameterExpression oldparameter)
        {
            _newparameter = newparameter;
            _oldparameter = oldparameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == _oldparameter)
                return _newparameter;

            return base.VisitParameter(node);
        }

        private bool _isPartitionKeySet = false;
        private bool _isRowKey = false;

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == MemberTypes.Property)
            {
                if (node.Member.DeclaringType.IsAssignableFrom(typeof(TEntity)))
                {
                    var propertyName = node.Member.Name;
                    var otherMember = typeof(TAzureEntity).GetProperty(propertyName);

                    var partitionRowKey = (RowKeyAttribute)otherMember.GetCustomAttributes(typeof(RowKeyAttribute), false).SingleOrDefault();
                    if (partitionRowKey != null)
                    {
                        otherMember = typeof(TAzureEntity).GetProperty("RowKey");
                        this._isRowKey = true;
                    }
                    else
                    {
                        var partitionKeyAttribute = (PartitionKeyAttribute)otherMember.GetCustomAttributes(typeof(PartitionKeyAttribute), false).SingleOrDefault();
                        if (partitionKeyAttribute != null)
                        {
                            otherMember = typeof(TAzureEntity).GetProperty("PartitionKey");
                            this._isPartitionKeySet = true;
                        }
                    }

                    var memberExpression = Expression.Property(Visit(node.Expression), otherMember);
                    return memberExpression;
                }
                else if (this._isPartitionKeySet || this._isRowKey)
                {
                    var propertyName = node.Member.Name;
                    var otherMember = typeof(TAzureEntity).GetProperty(propertyName);
                    var type = otherMember.PropertyType;

                    if (type.IsValueType)
                    {
                        return ToString(node, type);
                    }
                }
            }
            else if (node.Member.MemberType == MemberTypes.Field && (this._isPartitionKeySet || this._isRowKey))
	        {
                var fieldName = node.Member.Name;
                var otherMember = node.Member.DeclaringType.GetField(fieldName);
                var type = otherMember.FieldType;

                if (type.IsValueType)
                {
                    return ToString(node, type);
                }
	        }

            return base.VisitMember(node);
        }

        private static Expression ToString(Expression instance, Type type)
        {
            var toString = type.GetMethod("ToString", new Type[] { });
            var memberExpression = Expression.Call(instance, toString);
            return memberExpression;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                var memberExpression = Expression.Equal(Visit(node.Left), Visit(node.Right));

                this._isPartitionKeySet = false;
                this._isRowKey = false;

                return memberExpression;
            }

            return base.VisitBinary(node);
        }
    }
}
