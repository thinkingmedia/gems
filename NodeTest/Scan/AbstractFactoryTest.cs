using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Scan.Variable;

namespace NodeTest.Scan
{
    public abstract class AbstractFactoryTest
    {
        protected static string[] Paragraphs = new string[]{
            "The red fox jumped {0} over the house.",
            "#fs38e 3#83r4 nasd\t\n{0}\n\t @#3in 3h38#,3.940.",
            "{0} this old house.",
            "Where are we going {0}"
        };

        protected virtual string GetValue(VariableMatch pMatch)
        {
            throw new NotImplementedException();
        }

        protected void MustMatch<T>(IFactoryMatch pFactory, string pStr, T pValue)
        {
            VariableMatch matched = pFactory.Create(1, pStr) as VariableMatch;
            Assert.IsNotNull(matched, pStr);
            Assert.AreEqual(pValue.ToString(), GetValue(matched));
        }

        protected void MustFail(IFactoryMatch pFactory, string pStr)
        {
            VariableMatch matched = pFactory.Create(1, pStr);
            Assert.IsNull(matched, pStr);
        }

        protected void MatchParagraphs<T>(IFactoryMatch pFactory, string pStr, T pValue, bool pMatch)
        {
            if (pMatch)
            {
                MustMatch<T>(pFactory, pStr, pValue);
                foreach (string paragraph in AbstractFactoryTest.Paragraphs)
                {
                    MustMatch<T>(pFactory, string.Format(paragraph, pStr), pValue);
                }
            }
            else
            {
                MustFail(pFactory, pStr);
                foreach (string paragraph in AbstractFactoryTest.Paragraphs)
                {
                    MustFail(pFactory, string.Format(paragraph, pStr));
                }
            }
        }
    }
}
