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
            else if (node.Member.MemberType == MemberTypes.Field && (this._isPartitionKeySet || this._isRowKey))
	        {
                var fieldName = node.Member.Name;
                var origMember = node.Member.DeclaringType.GetField(fieldName);
                if (origMember.FieldType == typeof(Guid))
                {
                    Expression expression = node;
                    string format = "d";
                    Type type = typeof(Guid);

                    /*if (this._isPartitionKeySet)
                    {
                        expression = Expression.Call(node, origMember.FieldType.GetMethod("GetHashCode")); // int GetHashCode();
                        format = AzureEntityFactory.SELF_ID_PARTIOTION_FORMAT;
                        type = typeof(int);
                    }*/

                    var formatExpression = Expression.Constant(format);
                    var toString = type.GetMethod("ToString", new[] { typeof(string) }); // (Guid || int).ToString("format");

                    var memberExpression = Expression.Call(expression, toString, formatExpression);
                    return memberExpression;
                }
	        }

            return base.VisitMember(node);
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
