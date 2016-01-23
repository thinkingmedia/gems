using System.Collections.Generic;

namespace DataSources.Query
{
    /// <summary>
    /// The SQL join types.
    /// </summary>
    public enum eJOIN
    {
        NONE,
        INNER,
        LEFT,
        RIGHT,
        LEFT_OUTER,
        RIGHT_OUTER
    }

    public interface iJoinable
    {
        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        iQueryBuilder Join(eJOIN pJoinType, bool pAuto, Model pModel, IEnumerable<string> pFields);

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        iQueryBuilder Join(eJOIN pJoinType, string pTable, IEnumerable<string> pFields);

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        iQueryBuilder LeftJoin(Model pModel, params string[] pFields);

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        iQueryBuilder LeftJoin(string pTable, params string[] pFields);

        /// <summary>
        /// Starts a JOIN ON condition chain
        /// </summary>
        iCondition On();

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        iQueryBuilder RightJoin(string pTable, params string[] pFields);

        /// <summary>
        /// Adds a model to the current list of joined tables. The join
        /// type has to be the same.
        /// </summary>
        iQueryBuilder RightJoin(Model pModel, params string[] pFields);
    }
}