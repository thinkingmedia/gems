using System.Collections.Generic;
using Common.Events;
using DataSources.Exceptions;

namespace DataSources.Query
{
    public class Joinable : AbstractQueryFeature, iJoinable
    {
        /// <summary>
        /// The JOIN ON conditions
        /// </summary>
        public readonly iCondition Where;

        /// <summary>
        /// List of tables to join.
        /// </summary>
        public readonly List<JoinTable> JoinTables;

        /// <summary>
        /// The current join type.
        /// </summary>
        public eJOIN Type;

        /// <summary>
        /// Constructor
        /// </summary>
        public Joinable(iQueryBuilder pQuery)
            : base(pQuery)
        {
            Where = new Conditions(pQuery);
            Type = eJOIN.LEFT;
            JoinTables = new List<JoinTable>();
        }

        /// <summary>
        /// True if there are tables join.
        /// </summary>
        public bool isJoin()
        {
            return JoinTables.Count != 0;
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder Join(eJOIN pJoinType, bool pAuto, Model pModel, IEnumerable<string> pFields)
        {
            Join(pJoinType, pModel.Settings.Table, pFields);

            if (!pAuto)
            {
                return Query();
            }

            string foreignKey = string.Format("{0}_id", Inflector.Underscore(Query().Model().Settings.Alias));
            string id = Query().Model().Settings.PrimaryKey;

            Where.Expression(id, string.Format("= {0}", pModel.Field(foreignKey)));

            return Query();
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder Join(eJOIN pJoinType, string pTable, IEnumerable<string> pFields)
        {
            if (Query().Type() == QueryBuilder.eTYPE.INSERT)
            {
                throw new ModelException("Cannot JOIN on an INSERT statement.");
            }
            if (Type == eJOIN.NONE)
            {
                Type = pJoinType;
            }
            if (Type != pJoinType)
            {
                throw new ModelException("JOIN type already set for query as {0} JOIN", Type);
            }

            JoinTables.Add(new JoinTable(Query(), pTable, pFields));

            return Query();
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder LeftJoin(Model pModel, params string[] pFields)
        {
            return Join(eJOIN.LEFT, true, pModel, pFields);
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder LeftJoin(string pTable, params string[] pFields)
        {
            return Join(eJOIN.LEFT, pTable, pFields);
        }

        /// <summary>
        /// Starts a JOIN ON condition chain
        /// </summary>
        public iCondition On()
        {
            return Where;
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder RightJoin(string pTable, params string[] pFields)
        {
            return Join(eJOIN.RIGHT, pTable, pFields);
        }

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        public iQueryBuilder RightJoin(Model pModel, params string[] pFields)
        {
            return Join(eJOIN.RIGHT, true, pModel, pFields);
        }
    }
}