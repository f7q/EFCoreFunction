﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Query.Sql.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Expressions.Internal
{
    /// <summary>
    ///     Represents a SQL FullTextSerch expression.
    /// </summary>
    public class FTSExpression : Expression
    {
        /// <summary>
        ///     Creates a new instance of FullTextSrcheExpression.
        /// </summary>
        public FTSExpression([NotNull] Expression column, [NotNull] Expression param)
        {
            Check.NotNull(column, nameof(column));
            Check.NotNull(param, nameof(param));

            Column = column;
            Param = param;
        }

        /// <summary>
        ///     Gets the Column.
        /// </summary>
        /// <value>
        ///     The match expression.
        /// </value>
        public virtual Expression Column { get; }

        /// <summary>
        ///     Gets the SQL Param.
        /// </summary>
        /// <value>
        ///     The pattern to match.
        /// </value>
        public virtual Expression Param { get; }

        /// <summary>
        ///     Returns the node type of this <see cref="Expression" />. (Inherited from <see cref="Expression" />.)
        /// </summary>
        /// <returns>The <see cref="ExpressionType" /> that represents this expression.</returns>
        public override ExpressionType NodeType => ExpressionType.Extension;

        /// <summary>
        ///     Gets the static type of the expression that this <see cref="Expression" /> represents. (Inherited from <see cref="Expression" />.)
        /// </summary>
        /// <returns>The <see cref="Type" /> that represents the static type of the expression.</returns>
        public override Type Type => typeof(bool);
        /// <summary>
        ///     Dispatches to the specific visit method for this node type.
        /// </summary>
        protected override Expression Accept(ExpressionVisitor visitor)
        {
            Check.NotNull(visitor, nameof(visitor));

            var specificVisitor = visitor as MyNpgsqlQuerySqlGenerator;

            return specificVisitor != null
                ? specificVisitor.VisitFTS(this)
                : base.Accept(visitor);
        }

        /// <summary>
        ///     Tests if this object is considered equal to another.
        /// </summary>
        /// <param name="obj"> The object to compare with the current object. </param>
        /// <returns>
        ///     true if the objects are considered equal, false if they are not.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((FTSExpression)obj);
        }

        private bool Equals(FTSExpression other)
            => Equals(Column, other.Column)
               && Equals(Param, other.Param);
        /// <summary>
        ///     Reduces the node and then calls the <see cref="ExpressionVisitor.Visit(System.Linq.Expressions.Expression)" /> method passing the
        ///     reduced expression.
        ///     Throws an exception if the node isn't reducible.
        /// </summary>
        /// <param name="visitor"> An instance of <see cref="ExpressionVisitor" />. </param>
        /// <returns> The expression being visited, or an expression which should replace it in the tree. </returns>
        /// <remarks>
        ///     Override this method to provide logic to walk the node's children.
        ///     A typical implementation will call visitor.Visit on each of its
        ///     children, and if any of them change, should return a new copy of
        ///     itself with the modified children.
        /// </remarks>
        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var newColumnExpression = visitor.Visit(Column);
            var newParamExpression = visitor.Visit(Param);

            return newColumnExpression != Column
                   || newParamExpression != Param
                ? new FTSExpression(newColumnExpression, newParamExpression)
                : this;
        }
        /// <summary>
        ///     Returns a hash code for this object.
        /// </summary>
        /// <returns>
        ///     A hash code for this object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.GetHashCode();
                hashCode = (hashCode * 397) ^ Column.GetHashCode();
                hashCode = (hashCode * 397) ^ (Param?.GetHashCode() ?? 0);

                return hashCode;
            }
        }

        /// <summary>
        ///     Creates a <see cref="string" /> representation of the Expression.
        /// </summary>
        /// <returns>A <see cref="string" /> representation of the Expression.</returns>
        public override string ToString() => $"{Column} &@~ {Param}";
    }
}
